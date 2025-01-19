using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

/**
 * Publiczna klasa pozwalajaca na deserializacje plikow zawierajaca zapisane karty w formacie Json.
 * Klasa zawiera metody:
 *  - Import przerabia objekty pobrane z plikow json na itemy i karty
 *  - PickCardQuality przerabia numeryczna wartosc karty na liczbe rzymska
 *  - AssignAsAddressable dodaje assety do rejestru addressables
 */
public struct JsonItems
{
    public List<JItem> itemsList;
}

[System.Serializable]
public struct JItem
{
    public string itemType;
    public string itemName;
    public string description;
    public List<string> cards;
    public string icon;
}

public class Deserialization : Editor
{
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _chest;
    [SerializeField] private GameObject _legs;
    [SerializeField] private GameObject _boots;
    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _it1;
    [SerializeField] private GameObject _it2;
    [SerializeField] private GameObject _it3;
    [SerializeField] private GameObject _it4;
    [SerializeField] private GameObject _it5;
    [SerializeField] private GameObject _it6;

    [SerializeField] private TextAsset AttackFile;
    [SerializeField] private JsonAttackCards objects;
    string cPath = "Assets/ScriptableObjectAssets/Card/Attack/";

    [SerializeField] private TextAsset CurseFile;
    [SerializeField] private JsonCurseCards curseObjects;
    string cursePath = "Assets/ScriptableObjectAssets/Card/Curse/";

    [SerializeField] private TextAsset DefenceFile;
    [SerializeField] private JsonDefenceCards dObjects;
    string dPath = "Assets/ScriptableObjectAssets/Card/Defence/";

    [SerializeField] private TextAsset MovementFile;
    [SerializeField] private JsonMovementCards mObjects;
    string mPath = "Assets/ScriptableObjectAssets/Card/Move/";

    [SerializeField] private TextAsset jsonItemsFile;
    [SerializeField] private JsonItems itemsObjects;
    string itemsPath = "Assets/ScriptableObjectAssets/Items/";

    public void Import()
    {
        objects = JsonUtility.FromJson<JsonAttackCards>(AttackFile.text);

        // tworzenie karty ataku
        foreach (var obj in objects.attackCardsList)
        {
            var s = ScriptableObject.CreateInstance<CardsSO>();

            s.id = obj.id;
            s.type = CardType.Attack;
            s.isActive = obj.isActive;
            s.cardQuality = obj.cardQuality;
            s.title = obj.title;
            s.description = obj.description;
            s.cost = obj.cost;
            s.damage = obj.damage;
            s.move = obj.move;
            s.backgroundPath = "Graphics/CardBackgrounds/AttackCards/Attack" + PickCardQuality(s.cardQuality);
            s.spritePath = $"Graphics/CardSprites/AttackCards/{s.title}";
            s.range = obj.range;

            AssetDatabase.CreateAsset(s, cPath + s.title + ".asset");
            AssetDatabase.SaveAssets();

            AssignAsAddressable(s, "AttackCardsGroup", "AttackCard", s.cardQuality == 0);
        }

        curseObjects = JsonUtility.FromJson<JsonCurseCards>(CurseFile.text);

        foreach (var obj in curseObjects.curseCardsList)
        {
            var s = ScriptableObject.CreateInstance<CardsSO>();

            s.id = obj.id;
            s.type = CardType.Curse;
            s.isActive = obj.isActive;
            s.cardQuality = obj.cardQuality;
            s.title = obj.title;
            s.description = obj.description;
            s.cost = obj.cost;
            s.damage = obj.damage;
            s.move = obj.move;
            s.backgroundPath = "Graphics/CardBackgrounds/CurseCards/Curse";
            s.spritePath = $"Graphics/CardSprites/CurseCards/{s.title}";
            s.range = obj.range;

            AssetDatabase.CreateAsset(s, cursePath + s.title + ".asset");
            AssetDatabase.SaveAssets();

            AssignAsAddressable(s, "CurseCardsGroup", "CurseCard", s.cardQuality == 0);
        }
        
        dObjects = JsonUtility.FromJson<JsonDefenceCards>(DefenceFile.text);

        foreach (var obj in dObjects.defenceCardsList)
        {
            var s = ScriptableObject.CreateInstance<CardsSO>();

            s.id = obj.id;
            s.isActive = obj.isActive;
            s.type = CardType.Defense;
            s.cardQuality = obj.cardQuality;
            s.title = obj.title;
            s.description = obj.description;
            s.cost = obj.cost;
            s.damage = obj.damage;
            s.move = obj.move;
            s.backgroundPath = "Graphics/CardBackgrounds/DefenceCards/Defence" + PickCardQuality(s.cardQuality);
            s.spritePath = $"Graphics/CardSprites/DefenceCards/{s.title}";
            s.range = obj.range;

            AssetDatabase.CreateAsset(s, dPath + s.title + ".asset");
            AssetDatabase.SaveAssets();

            AssignAsAddressable(s, "DefenceCardsGroup", "DefenceCard", s.cardQuality == 0);
        }
        
        mObjects = JsonUtility.FromJson<JsonMovementCards>(MovementFile.text);

        foreach (var obj in mObjects.movementCardsList)
        {
            var s = ScriptableObject.CreateInstance<CardsSO>();

            s.id = obj.id;
            s.isActive = obj.isActive;
            s.type = CardType.Movement;
            s.cardQuality = obj.cardQuality;
            s.title = obj.title;
            s.description = obj.description;
            s.cost = obj.cost;
            s.damage = obj.damage;
            s.move = obj.move;
            s.backgroundPath = "Graphics/CardBackgrounds/MovementCards/Move" + PickCardQuality(s.cardQuality);
            s.spritePath = $"Graphics/CardSprites/MovementCards/{s.title}";
            s.range = obj.range;

            AssetDatabase.CreateAsset(s, mPath + s.title + ".asset");
            AssetDatabase.SaveAssets();

            AssignAsAddressable(s, "MovementCardsGroup", "MovementCard", s.cardQuality == 0);
        }

        itemsObjects = JsonUtility.FromJson<JsonItems>(jsonItemsFile.text);

        // tworzenie itemow
        foreach (var obj in itemsObjects.itemsList)
        {
            var s = ScriptableObject.CreateInstance<Item>();

            switch (obj.itemType)
            {
                case "hand":
                    s.itemType = ItemType.hand;
                    break;
                case "cheast":

                    s.itemType = ItemType.cheast;
                    break;
                case "head":

                    s.itemType = ItemType.head;
                    break;
                case "boots":

                    s.itemType = ItemType.boots;
                    break;
                case "legs":

                    s.itemType = ItemType.legs;
                    break;
                case "additional":

                    s.itemType = ItemType.additional;
                    break;
                case "any":
                    s.itemType = ItemType.any;
                    break;
            }

            s.itemName = obj.itemName;
            s.description = obj.description;

            // dodawanie kart do itemow
            foreach(var c in obj.cards)
            {
                var guids = AssetDatabase.FindAssets("t:" + typeof(CardsSO),null);

                foreach(var guid in guids)
                {
                    string p = AssetDatabase.GUIDToAssetPath(guid);
                    
                    if (Path.GetFileNameWithoutExtension(p) == c && p != null)
                    {
                        s.cards.Add(AssetDatabase.LoadAssetAtPath<CardsSO>(p));
                    }
                    
                }
            }

            s.icon = Resources.Load<Sprite>(obj.icon);

            AssetDatabase.CreateAsset(s, itemsPath + s.itemName + ".asset");
            AssignAsAddressable(s, "Items", "Item", false);
            AssetDatabase.SaveAssets();

            
        }
        
    }
    private string PickCardQuality(int value)
    {
        switch (value)
        {
            case 0: return " I";
            case 1: return " I";
            case 2: return " II";
            case 3: return " III";
        }
        return "";
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
