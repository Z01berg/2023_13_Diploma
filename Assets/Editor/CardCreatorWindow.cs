using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class CardCreatorWindow : ScrollView
{
    private CardsSO _cardReference;

    private IntegerField _idField;
    private EnumField _cardTypeField;
    private IntegerField _rangeField;
    private Toggle _isActiveField;
    private IntegerField _cardQualityField;
    private TextField _titleField;
    private TextField _descriptionField;
    private IntegerField _costField;
    private IntegerField _damageField;
    private IntegerField _moveField;
    private Image _background;
    private Image _banner;

    public CardCreatorWindow(CardsSO cardReference)
    {
        CreateFields();

        _cardReference = cardReference;

        _idField.value = _cardReference.id;
        _cardTypeField.value = _cardReference.type;
        _rangeField.value = _cardReference.range;
        _isActiveField.value = _cardReference.isActive;
        _cardQualityField.value = _cardReference.cardQuality;
        _titleField.value = _cardReference.title;
        _descriptionField.value = cardReference.description;
        _costField.value = cardReference.cost;
        _damageField.value = cardReference.damage;
        _moveField.value = cardReference.move;
        _background.sprite = Resources.Load<Sprite>(cardReference.backgroundPath);
        _banner.sprite = Resources.Load<Sprite>(cardReference.spritePath);
    }

    private void CreateFields()
    {
        _idField = new IntegerField("id");
        _idField.isReadOnly = true;
        Add(_idField);

        _cardTypeField = new("card type", CardType.Movement);
        Add(_cardTypeField);

        _rangeField = new IntegerField("range");
        Add(_rangeField);

        _isActiveField = new("is active");
        Add(_isActiveField);

        _cardQualityField = new IntegerField("card quality");
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
        _banner.scaleMode = ScaleMode.ScaleToFit;
        _banner.style.height = 70;
        Add(_banner);

        style.paddingLeft = 1;
        style.borderRightWidth = 1;
        style.borderRightColor = Color.white;
        style.borderLeftWidth = 1;
        style.borderLeftColor = Color.white;
    }
}
