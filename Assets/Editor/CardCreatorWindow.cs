using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

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

    private string _currentBackgroundPath;
    private string _currentBannerPath;

    public CardCreatorWindow()
    {
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


        _savedCardReference = _cardReference;

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

        EditorUtility.SetDirty(_cardReference);
        //EditorUtility.SetDirty(_itemReference);

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

        _cardQualityField = new SliderInt("card quality", 0, 2);
        _cardQualityField.RegisterValueChangedCallback(CardQualityChanged);
        Add(_cardQualityField);

        _titleField = new TextField("title");
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
            var addressables = AddressablesUtilities.LoadItems(AddressablesTags.AttackCard, AddressablesTags.DefenceCard, AddressablesTags.MovementCard);
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
}
