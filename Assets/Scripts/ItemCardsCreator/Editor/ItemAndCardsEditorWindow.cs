using CardActions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;
using Toggle = UnityEngine.UIElements.Toggle;
using UnityEngine.ResourceManagement.ResourceProviders;

public class ItemAndCardsEditorWindow : EditorWindow
{
    private List<Item> _items = new List<Item>();
    private List<CardsSO> _cards = new();
    private string _selectedAdressableType;

    private ListView m_LeftPane;
    private VisualElement m_RightPane;
    [SerializeField] private int m_SelectedIndex = -1;

    private List<IModifiable> _rightPaneWindows = new List<IModifiable>();

    [MenuItem("Window/2023_13_Diploma/Item And Cards Editor Window")]
    public static void ShowWindow()
    {
        ItemAndCardsEditorWindow wnd = GetWindow<ItemAndCardsEditorWindow>();
        wnd.titleContent = new GUIContent("ItemAndCardsEditorWindow");

        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }


    public void CreateGUI()
    {
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        rootVisualElement.Add(splitView);

        m_LeftPane = new ListView();
        splitView.Add(m_LeftPane);
        m_RightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
        splitView.Add(m_RightPane);

        List<string> options = new() { "Items", "Cards" };

        PopupField<string> popupField = new PopupField<string>("Type", options,options.Count);
        m_LeftPane.hierarchy.Add(popupField);
         
        popupField.RegisterValueChangedCallback(OnAddressablesSelectionChange);

        m_LeftPane.selectionChanged += OnItemSelectionChange;

        m_LeftPane.selectedIndex = m_SelectedIndex;
        m_LeftPane.selectionChanged += (items) => { m_SelectedIndex = m_LeftPane.selectedIndex; };

        Button createNewButton = new(CreateNew);
        createNewButton.text = "New";
        m_LeftPane.hierarchy.Add(createNewButton);

        Button saveButton = new(Save);
        saveButton.text = "Save changes";
        m_LeftPane.hierarchy.Add(saveButton);

        Button importJsonButton = new(ImportJson);
        importJsonButton.text = "Import json";
        m_LeftPane.hierarchy.Add(importJsonButton);

        Button exportJsonButton = new(ExportJson);
        exportJsonButton.text = "Export json";
        m_LeftPane.hierarchy.Add(exportJsonButton);
    }

    private void OnItemSelectionChange(IEnumerable<object> selectedItems)
    {
        m_RightPane.Clear();
        _rightPaneWindows.Clear();

        var enumerator = selectedItems.GetEnumerator();
        if (enumerator.MoveNext())
        {
            
            var spriteImage = new Image();
            spriteImage.scaleMode = ScaleMode.ScaleToFit;
            if (_selectedAdressableType == "Items")
            {
                var splitViewCards = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
                m_RightPane.Add(splitViewCards);
                VisualElement lower = new VisualElement();
                
                var selectedItem = enumerator.Current as Item;
                if (selectedItem == null) return;

                spriteImage.sprite = selectedItem.icon;
                string name = selectedItem.itemName;
                string desc = selectedItem.description;
                
                List<CardsSO> cards = new List<CardsSO>(selectedItem.cards);
                var itemCreatorWindow = new ItemCreatorWindow(selectedItem, name, desc, selectedItem.itemType, spriteImage.sprite);
                splitViewCards.Add(itemCreatorWindow);
                _rightPaneWindows.Add(itemCreatorWindow);

                splitViewCards.Add(lower);

                foreach(var card in cards)
                {
                    lower.style.flexDirection = FlexDirection.Row;
                    lower.style.justifyContent = Justify.Center;

                    var cardCreatorWindow = new CardCreatorWindow(card,selectedItem);
                    lower.Add(cardCreatorWindow);
                    _rightPaneWindows.Add(cardCreatorWindow);
                }

            }
            else if(_selectedAdressableType == "Cards")
            {
                var selectedItem = enumerator.Current as CardsSO;
                if (selectedItem == null) return;
                var cardCreatorWindow = new CardCreatorWindow(selectedItem);
                m_RightPane.Add(cardCreatorWindow);
                _rightPaneWindows.Add(cardCreatorWindow);
            }
                        
        }
    }

    private void OnAddressablesSelectionChange(ChangeEvent<string> evt)
    {
        List<string> options = new() { "Items", "Cards" };
        List<AddressablesTags> selected = new();

        _selectedAdressableType = evt.newValue;

        if (evt.newValue == "Items")
        {
            selected = new() { AddressablesTags.Item };
        }
        else if (evt.newValue == "Cards")
        {
            selected = new() { AddressablesTags.AttackCard, AddressablesTags.DefenceCard, AddressablesTags.MovementCard };
        }


        var addressables = AddressablesUtilities.LoadItems(selected.ToArray());
        _items.Clear();
        _cards.Clear();

        foreach (var item in addressables)
        {
            if (evt.newValue == "Items")
            {
                _items.Add(item as Item);
            }
            else if(evt.newValue == "Cards")
            {
                _cards.Add(item as CardsSO);
            }
        }

        if (evt.newValue == "Items")
        {
            m_LeftPane.Clear();
            m_LeftPane.makeItem = () => new Label();
            m_LeftPane.bindItem = (item, index) => { (item as Label).text = _items[index].name; };
            m_LeftPane.itemsSource = _items;
        }
        else if (evt.newValue == "Cards")
        {
            m_LeftPane.Clear();
            m_LeftPane.makeItem = () => new Label();
            m_LeftPane.bindItem = (item, index) => { (item as Label).text = _cards[index].name; };
            _cards = _cards.OrderBy(x => x.name).ToList();
            m_LeftPane.itemsSource = _cards;
        }

    }

    private void CreateNew()
    {
        m_RightPane.Clear();
        _rightPaneWindows.Clear();

        if (_selectedAdressableType == "Items")
        {
            var splitViewCards = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
            m_RightPane.Add(splitViewCards);
            
            VisualElement lower = new VisualElement();

            List<CardsSO> cards = new List<CardsSO>();

            var itemCreatorWindow = new ItemCreatorWindow();
            splitViewCards.Add(itemCreatorWindow);
            _rightPaneWindows.Add(itemCreatorWindow);

            splitViewCards.Add(lower);

            for(int i = 0; i < 3; i++)
            {
                lower.style.flexDirection = FlexDirection.Row;
                lower.style.justifyContent = Justify.Center;
                var cardCreatorWindow = new CardCreatorWindow();
                lower.Add(cardCreatorWindow);
                _rightPaneWindows.Add(cardCreatorWindow);
            }
        }
        if(_selectedAdressableType == "Cards")
        {
            var window = new CardCreatorWindow();
            m_RightPane.Add(window);
            _rightPaneWindows.Add(window);
        }
    }

    private void ImportJson()
    {
        Deserialization deserialization = new();
        deserialization.Import();
        
    }

    private void ExportJson()
    {

    }

    private void Save()
    {
        foreach (IModifiable window in _rightPaneWindows)
        {
            window.Save();
        }

    }

}
