using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

public class Deserialization : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private JsonCards objects;
    string cPath = "Assets/ScriptableObjectAssets/Card/";

    [SerializeField] private TextAsset jsonItemsFile;
    [SerializeField] private JsonItems itemsObjects;
    string itemsPath = "Assets/ScriptableObjectAssets/Items/";

    void Start()
    {
        //Inventory.Instance.items = new List<Item>();
        
        objects = JsonUtility.FromJson<JsonCards>(jsonFile.text);

        foreach (var obj in objects.attackCardsList)
        {
            var s = ScriptableObject.CreateInstance<CardsSO>();

            s.id = obj.id;
            s.isActive = obj.isActive;
            s.cardQuality = obj.cardQuality;
            s.title = obj.title;
            s.description = obj.description;
            s.cost = obj.cost;
            s.damage = obj.damage;
            s.move = obj.move;
            s.backgroundPath = obj.backgroundPath;
            s.spritePath = obj.spritePath;

            AssetDatabase.CreateAsset(s, cPath + s.title + ".asset");
            AssetDatabase.SaveAssets();
        }
        
        itemsObjects = JsonUtility.FromJson<JsonItems>(jsonItemsFile.text);

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
            AssetDatabase.SaveAssets();
            
            Inventory.Instance.items.Add(AssetDatabase.LoadAssetAtPath<Item>(itemsPath + s.itemName + ".asset"));
        }
        GameObject.Find("ItemsPanel").GetComponent<ListAllAvailable>().ListAllItemsInInv();
    }

}
