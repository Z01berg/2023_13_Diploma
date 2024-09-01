using CardActions;
using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;
using Toggle = UnityEngine.UIElements.Toggle;

public class ItemCreatorWindow : VisualElement
{
    private Item _itemReference;

    private Image _spriteImage;
    private TextField _nameField;
    private TextField _descriptionField;
    private EnumField _typeField;

    public ItemCreatorWindow()
    {
        CreateFields();
    }

    public ItemCreatorWindow(Item itemReference, string name, string desc, ItemType itemType, Sprite sprite)
    {
        CreateFields();

        _itemReference = itemReference;

        _spriteImage.sprite = sprite;
        _nameField.value = name;
        _descriptionField.value = desc;
        _typeField.value = itemType;

    }

    private void CreateFields()
    {
        _spriteImage = new Image();
        _spriteImage.scaleMode = ScaleMode.ScaleToFit;
        Add(_spriteImage);

        _nameField = new TextField("name");
        Add(_nameField);

        _descriptionField = new TextField("description");
        _descriptionField.multiline = true;
        Add(_descriptionField);

        _typeField = new EnumField("item type", ItemType.any);
        Add(_typeField);
    }
}
