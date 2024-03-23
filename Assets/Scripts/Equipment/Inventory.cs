using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * A class containing a list of all items currently existing in the game.
 * There should only be one instance of this class in a scene.
 * 
 * The items are stored in the public items list variable.
 * 
 * The Awake() function check if there is only one class of this type in a scene and assignes
 * all the items to the items list.
 */

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
}
