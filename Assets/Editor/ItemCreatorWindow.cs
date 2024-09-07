using CardActions;
using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;
using Toggle = UnityEngine.UIElements.Toggle;
using Codice.Utils;

public class ItemCreatorWindow : VisualElement, IModifiable
{
    string itemsPath = "Assets/ScriptableObjectAssets/Items/";
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

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    private void CreateFields()
    {
        _spriteImage = new Image();
        
        _spriteImage.style.backgroundColor = Color.white;
        _spriteImage.style.width = 100;
        _spriteImage.style.height = 100;
        _spriteImage.style.alignSelf = Align.Center;
        _spriteImage.scaleMode = ScaleMode.ScaleToFit;
        Add(_spriteImage);

        Button importGraphic = new Button(ImportGraphic);
        importGraphic.text = "import graphic";
        importGraphic.style.paddingBottom = 10;
        importGraphic.style.height = 20;
        importGraphic.style.width= 100;
        importGraphic.style.alignSelf = Align.Center;
        importGraphic.style.unityTextAlign = TextAnchor.MiddleCenter;
        Add(importGraphic);

        _nameField = new TextField("name");
        Add(_nameField);

        _descriptionField = new TextField("description");
        _descriptionField.multiline = true;
        Add(_descriptionField);

        _typeField = new EnumField("item type", ItemType.any);
        Add(_typeField);

    }

    void IModifiable.Save()
    {
        if(_nameField.value == "" || _nameField.value == null)
        {
            throw new System.Exception("Item name has not been given.");
        }

        if (_itemReference == null) 
        {
            _itemReference = ScriptableObject.CreateInstance<Item>();
            AssetDatabase.CreateAsset(_itemReference, itemsPath + _nameField.value + ".asset");
            AssignAsAddressable(_itemReference, "Items", "Item", false);
            AssetDatabase.SaveAssets();
        }
        
        _itemReference.itemName = _nameField.value;
        _itemReference.description = _descriptionField.value;
        _itemReference.icon = _spriteImage.sprite;
        _itemReference.itemType = (ItemType)_typeField.value;
    }

    void ImportGraphic()
    {
        var file = EditorUtility.OpenFilePanel("graphic selection","..","");
        Debug.Log(file);

        if (file == null || file == "") 
        {
            return;
        }

        // TODO: Copy file to item graphic directory
    }

    private void AssignAsAddressable(Object asset, string targetGroup, string targetLabel, bool isDefault)
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
