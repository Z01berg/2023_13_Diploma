using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/**
 * Publiczna klasa pozwalajaca na deserializacje plikow zawierajaca zapisane karty w formacie Json
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

public class Deserialization : MonoBehaviour
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
    [SerializeField] private JsonCards objects;
    string cPath = "Assets/ScriptableObjectAssets/Card/Attack/";

    [SerializeField] private TextAsset DefenceFile;
    [SerializeField] private JsonCards dObjects;
    string dPath = "Assets/ScriptableObjectAssets/Card/Defence/";

    [SerializeField] private TextAsset MovementFile;
    [SerializeField] private JsonCards mObjects;
    string mPath = "Assets/ScriptableObjectAssets/Card/Move/";

    [SerializeField] private TextAsset jsonItemsFile;
    [SerializeField] private JsonItems itemsObjects;
    string itemsPath = "Assets/ScriptableObjectAssets/Items/";

    public void Import()
    {
        //Inventory.Instance.items = new List<Item>();
        
        objects = JsonUtility.FromJson<JsonCards>(AttackFile.text);

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
            s.range = obj.range;

            AssetDatabase.CreateAsset(s, cPath + s.title + ".asset");
            AssetDatabase.SaveAssets();

            if (s.name.Contains("Defoult"))
            {
                var lScript = _leftHand.GetComponent<DefaultCards>();
                var rScript = _rightHand.GetComponent<DefaultCards>();
                lScript.ClearList();
                rScript.ClearList();

                for (int i = 0; i < 3; i++)
                {
                    lScript.AssignCard(s);
                    rScript.AssignCard(s);
                }
            }
        }

        dObjects = JsonUtility.FromJson<JsonCards>(DefenceFile.text);

        foreach (var obj in dObjects.attackCardsList)
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
            s.range = obj.range;

            AssetDatabase.CreateAsset(s, dPath + s.title + ".asset");
            AssetDatabase.SaveAssets();

            if (s.name.Contains("Default"))
            {
                var headScript = _head.GetComponent<DefaultCards>();
                var chestScript = _chest.GetComponent<DefaultCards>();
                headScript.ClearList();
                chestScript.ClearList();

                for (int i = 0; i < 3; i++)
                {
                    headScript.AssignCard(s);
                    chestScript.AssignCard(s);
                }
            }

        }

        mObjects = JsonUtility.FromJson<JsonCards>(MovementFile.text);

        foreach (var obj in mObjects.attackCardsList)
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
            s.range = obj.range;

            AssetDatabase.CreateAsset(s, mPath + s.title + ".asset");
            AssetDatabase.SaveAssets();

            if (s.name.Contains("Default"))
            {
                var legsScript = _legs.GetComponent<DefaultCards>();
                legsScript.ClearList();
                
                for (int i = 0; i < 3; i++)
                {
                    legsScript.AssignCard(s);
                }
            }
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
            
            // not needed. Left just in case. I might use it later
            //Inventory.Instance.items.Add(AssetDatabase.LoadAssetAtPath<Item>(itemsPath + s.itemName + ".asset"));
        }
        //GameObject.Find("ItemsPanel").GetComponent<ListAllAvailable>().ListAllItemsInInv();
    }

}
