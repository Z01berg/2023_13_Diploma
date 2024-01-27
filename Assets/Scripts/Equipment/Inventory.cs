using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class Inventory : MonoBehaviour
{

    #region Singleton

    public static Inventory Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        Instance = this;

        string[] fileEntries = System.IO.Directory.GetFiles("Assets/ScriptableObjectAssets/Items/");
        foreach (string fileEntry in fileEntries)
        {
            if(fileEntry.EndsWith(".asset"))
                items.Add(AssetDatabase.LoadAssetAtPath<Item>(fileEntry));
        }
        GameObject.Find("ItemsPanel").GetComponent<ListAllAvailable>().ListAllItemsInInv();
    }

    #endregion

    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        //items.Add(item);
    }

    public void Remove(Item item)
    {
        //items.Remove(item);
    }
}
