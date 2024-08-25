using CardActions;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;

public class ItemAndCardsEditorWindow : EditorWindow
{
    private List<Item> _items = new List<Item>();
    private List<CardsSO> _cards = new();
    private string _selectedAdressableType;

    private ListView m_LeftPane;
    private VisualElement m_RightPane;
    [SerializeField] private int m_SelectedIndex = -1;

    [MenuItem("Examples/ItemAndCardsEditorWindow")]
    public static void ShowExample()
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

        Button button = new Button();
        button.text = "New item";
        m_LeftPane.hierarchy.Add(button);
    }

    private void OnItemSelectionChange(IEnumerable<object> selectedItems)
    {
        m_RightPane.Clear();

        var enumerator = selectedItems.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var splitViewCards = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
            m_RightPane.Add(splitViewCards);

            var spriteImage = new Image();
            spriteImage.scaleMode = ScaleMode.ScaleToFit;
            if (_selectedAdressableType == "Items")
            {
                VisualElement upper = new VisualElement();
                VisualElement lower = new VisualElement();
                

                splitViewCards.Add(upper);
                splitViewCards.Add(lower);

                var selectedItem = enumerator.Current as Item;
                if (selectedItem == null) return;

                spriteImage.sprite = selectedItem.icon;
                upper.Add(spriteImage);

                string name = selectedItem.name; 
                TextField t = new TextField("name");
                t.SetValueWithoutNotify(name);
                upper.Add(t);

                string desc = selectedItem.description;
                TextField t2 = new TextField("description");
                t2.multiline = true;
                t2.SetValueWithoutNotify(desc);
                upper.Add(t2);
                
                EnumField enumField = new EnumField("item type",selectedItem.itemType);
                upper.Add(enumField);

                List<CardsSO> cards = new List<CardsSO>(selectedItem.cards);



            }
            else if(_selectedAdressableType == "Cards")
            {
                var selectedItem = enumerator.Current as CardsSO;
                if (selectedItem == null) return;
                spriteImage.sprite = Resources.Load<Sprite>(selectedItem.spritePath);
                spriteImage.transform.scale *= 0.1f;
                m_RightPane.Add(spriteImage);
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
}
