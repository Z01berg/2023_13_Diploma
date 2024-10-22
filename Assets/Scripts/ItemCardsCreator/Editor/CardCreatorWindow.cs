using CardActions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEngine.AddressableAssets;

public class CardCreatorWindow : ScrollView, IModifiable
{
    private CardsSO _cardReference;
    private CardsSO _savedCardReference;
    private Item _itemReference;

    private PopupField<CardsSO> _cards;
    private IntegerField _idField;
    private EnumField _cardTypeField;
    private IntegerField _rangeField;
    private Toggle _isActiveField;
    private SliderInt _cardQualityField;
    private TextField _titleField;
    private TextField _descriptionField;
    private IntegerField _costField;
    private IntegerField _damageField;
    private IntegerField _moveField;
    private Image _background;
    private Image _banner;

    private string _currentBackgroundPath = "Graphics/CardBackgrounds/AttackCards/Attack II";
    private string _currentBannerPath;

    public CardCreatorWindow()
    {
        _cardReference = ScriptableObject.CreateInstance<CardsSO>();

        _cardReference.backgroundPath = "Graphics/CardBackgrounds/AttackCards/Attack";
        _cardReference.spritePath = $"Graphics/CardSprites/AttackCards/name";

        CreateFields();
    }

    public CardCreatorWindow(CardsSO cardReference, Item itemReference = null)
    {
        _itemReference = itemReference;

        CreateFields();

        _cardReference = cardReference;
        _savedCardReference = cardReference;
        _currentBackgroundPath = _cardReference.backgroundPath;
        PopulateFields();

    }

    private void CardSelectionChanged(ChangeEvent<CardsSO> evt)
    {

        _cardReference = evt.newValue;

        PopulateFields();
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void Save()
    {
        if (_itemReference != null)
        {
            _itemReference.cards.Remove(_savedCardReference);
            _itemReference.cards.Add(_cardReference);
            foreach (var element in parent.Children())
            {
                element.MarkDirtyRepaint();
            }
        }

        _cardReference.id = _idField.value;
        _cardReference.type = (CardType)_cardTypeField.value;
        _cardReference.range = _rangeField.value;
        _cardReference.isActive = _isActiveField.value;
        _cardReference.cardQuality = _cardQualityField.value;
        _cardReference.title = _titleField.value;
        _cardReference.description = _descriptionField.value;
        _cardReference.cost = _costField.value;
        _cardReference.damage = _damageField.value;
        _cardReference.move = _moveField.value;
        _cardReference.backgroundPath = _currentBackgroundPath;
        _cardReference.spritePath = _currentBannerPath;
        _cardReference.name = _titleField.value;

        if(_savedCardReference == null)
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(CardsSO), null);
            List<CardsSO> all_cards_list = new();
            
            foreach (var guid in guids)
            {
                string p = AssetDatabase.GUIDToAssetPath(guid);

                all_cards_list.Add(AssetDatabase.LoadAssetAtPath<CardsSO>(p));
            }

            all_cards_list = all_cards_list.Where(x => x.type == _cardReference.type).ToList();

            var path = "";
            var group = "";
            var tag = "";
            if (_cardReference.type == CardType.Defense)
            {
                path = "Assets/ScriptableObjectAssets/Card/Defence/";
                group = "DefenceCardsGroup";
                tag = "DefenceCard";
            }
            else if (_cardReference.type == CardType.Attack) 
            {
                path = "Assets/ScriptableObjectAssets/Card/Attack/";
                group = "AttackCardsGroup";
                tag = "AttackCard";
            }
            else if (_cardReference.type == CardType.Curse)
            {
                path = "Assets/ScriptableObjectAssets/Card/Curse/";
                group = "CurseCardsGroup";
                tag = "CurseCard";
            }
            else if (_cardReference.type == CardType.Movement)
            {
                path = "Assets/ScriptableObjectAssets/Card/Move/";
                group = "MovementCardsGroup";
                tag = "MovementCard";
            }

            var maxid = all_cards_list.Max(x => x.id);
            _cardReference.id = (maxid + 1);
            _idField.value = _cardReference.id;

            AssetDatabase.CreateAsset(_cardReference, path + _cardReference.title + ".asset");
            AssetDatabase.SaveAssets();

            AssignAsAddressable(_cardReference, group, tag, _cardReference.cardQuality == 0);

            AssetDatabase.Refresh();

        }

        _savedCardReference = _cardReference;

        EditorUtility.SetDirty(_cardReference);

    }

    private void CreateFields()
    {
        if (_itemReference != null)
        {
            _cards = new PopupField<CardsSO>();
            _cards.RegisterValueChangedCallback(CardSelectionChanged);
            Add(_cards);
        }

        _idField = new IntegerField("id");
        _idField.isReadOnly = true;
        Add(_idField);

        _cardTypeField = new("card type", CardType.Movement);
        _cardTypeField.RegisterValueChangedCallback(CardTypeChanged);
        Add(_cardTypeField);

        _rangeField = new IntegerField("range");
        Add(_rangeField);

        _isActiveField = new("is active");
        Add(_isActiveField);

        _cardQualityField = new SliderInt("card quality", 0, 3);
        _cardQualityField.RegisterValueChangedCallback(CardQualityChanged);
        Add(_cardQualityField);

        _titleField = new TextField("title");
        _titleField.RegisterValueChangedCallback(CardNameChanged);
        Add(_titleField);

        _descriptionField = new TextField("description");
        _descriptionField.multiline = true;
        _descriptionField.style.whiteSpace = WhiteSpace.Normal;
        _descriptionField.style.height = new StyleLength(StyleKeyword.Auto);
        Add(_descriptionField);

        _costField = new IntegerField("cost");
        Add(_costField);

        _damageField = new IntegerField("damage");
        Add(_damageField);

        _moveField = new IntegerField("move");
        Add(_moveField);

        _background = new Image();
        _background.scaleMode = ScaleMode.ScaleToFit;
        _background.style.height = 200;
        Add(_background);

        _banner = new Image();
        Button editBannerButton = new(PickCardBanner);
        editBannerButton.text = "select banner";
        Add(editBannerButton);

        _banner.scaleMode = ScaleMode.ScaleToFit;
        _banner.style.height = 70;
        Add(_banner);

        style.paddingLeft = 1;
        style.borderRightWidth = 1;
        style.borderRightColor = Color.white;
        style.borderLeftWidth = 1;
        style.borderLeftColor = Color.white;
    }

    private void CardNameChanged(ChangeEvent<string> evt = null)
    {
        var split = _cardReference.spritePath.Split("/");
        split[split.Length - 1] = _titleField.value;
        
        CardType cardType = Enum.Parse<CardType>(_cardTypeField.value.ToString());
        if (cardType == CardType.Attack)
        {
            split[split.Length - 2] = "AttackCards";
        }
        if (cardType == CardType.Defense)
        {
            split[split.Length - 2] = "DefenceCards";
        }
        if (cardType == CardType.Movement)
        {
            split[split.Length - 2] = "MovementCards";
        }
        if (cardType == CardType.Curse)
        {
            split[split.Length - 2] = "CurseCards";
        }
        
        var path = String.Join("/",split);
        _banner.sprite = Resources.Load<Sprite>(path);
        _currentBannerPath = path;
    }

    private void CardQualityChanged(ChangeEvent<int> evt)
    {
        CardTypeChanged();
    }


    private void PickCardBanner()
    {
        var file = EditorUtility.OpenFilePanel("graphic selection", "Assets\\Resources\\Graphics\\CardSprites", "");

        if (file == null || file.Length == 0 || file == "")
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        string[] folders = file.Split("/");
        var index = 0;
        _titleField.value = Path.ChangeExtension(folders[folders.Length - 1],null);
        for (int i = 0; i < folders.Length; i++)
        {
            if (folders[i] == "Graphics")
            {
                index = i;
                break;
            }
        }


        var path = String.Join("/", folders.Skip(index));
        path = Path.ChangeExtension(path, null);

        _banner.sprite = Resources.Load<Sprite>(path);
        _currentBannerPath = path;
    }

    private void CardTypeChanged(ChangeEvent<Enum> evt = null)
    {
        string[] dirs = _cardReference.backgroundPath.Split("/");


        var type = _cardTypeField.value;

        if (evt != null)
        {
            type = evt.newValue;
        }

        switch (type)
        {
            case CardType.Attack:
                dirs[dirs.Length - 2] = "AttackCards";
                dirs[dirs.Length - 1] = "Attack" + PickCardQuality();
                break;
            case CardType.Defense:
                dirs[dirs.Length - 2] = "DefenceCards";
                dirs[dirs.Length - 1] = "Defence" + PickCardQuality();
                break;
            case CardType.Curse:
                dirs[dirs.Length - 2] = "CurseCards";
                dirs[dirs.Length - 1] = "Curse";
                break;
            case CardType.Movement:
                dirs[dirs.Length - 2] = "MovementCards";
                dirs[dirs.Length - 1] = "Move" + PickCardQuality();
                break;
        }

        _currentBackgroundPath = String.Join("/", dirs);
        _background.sprite = Resources.Load<Sprite>(String.Join("/", dirs));

        CardNameChanged();
    }



    private string PickCardQuality()
    {
        switch (_cardQualityField.value)
        {
            case 0: return " I";
            case 1: return " I";
            case 2: return " II";
            case 3: return " III";
        }
        return "";
    }

    private void PopulateFields()
    {
        if (_itemReference != null)
        {
            var addressables = AddressablesUtilities.LoadItems(Addressables.MergeMode.Union, AddressablesTags.AttackCard, AddressablesTags.DefenceCard, AddressablesTags.MovementCard);
            List<CardsSO> convCards = new();
            foreach (var card in addressables)
            {
                convCards.Add(card as CardsSO);
            }
            _cards.choices = convCards;
            _cards.value = _cardReference;
        }


        _idField.value = _cardReference.id;
        _cardTypeField.value = _cardReference.type;
        _rangeField.value = _cardReference.range;
        _isActiveField.value = _cardReference.isActive;
        _cardQualityField.value = _cardReference.cardQuality;
        _titleField.value = _cardReference.title;
        _descriptionField.value = _cardReference.description;
        _costField.value = _cardReference.cost;
        _damageField.value = _cardReference.damage;
        _moveField.value = _cardReference.move;
        _background.sprite = Resources.Load<Sprite>(_cardReference.backgroundPath);
        _banner.sprite = Resources.Load<Sprite>(_cardReference.spritePath);
    }
    private void AssignAsAddressable(UnityEngine.Object asset, string targetGroup, string targetLabel, bool isDefault)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        string assetPath = AssetDatabase.GetAssetPath(asset);
        string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
        var group = settings.FindGroup(targetGroup);
        var entry = settings.CreateOrMoveEntry(assetGUID, group);

        entry.SetLabel(targetLabel, true, true, true);
        if (isDefault)
        {
            entry.SetLabel("DefaultCard", true, true, true);
        }
    }
}
