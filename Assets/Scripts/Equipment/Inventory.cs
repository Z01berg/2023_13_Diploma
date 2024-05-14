using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/**
 * Publiczna klasa przechowuja ca i dajaca latwy dostep do wszystkich itemow
 * istniejacych w grze. Przy rozpoczeciu gry klasa znajduje i pobiera wszystkie ScriptableObjects typu Item 
 * ktore istnieja a nastepnie dodaje je do listy items do ktorej dostep mozna otrzymac z poza tej klasy
 * poprzez uzycie instrukcji "Inventory.Instance.items"
 */

public class Inventory : MonoBehaviour
{

    AsyncOperationHandle<IList<Item>> loadHandle;

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
        
        LoadItems();
        loadHandle.WaitForCompletion();
        
        GameObject.Find("ItemsPanel").GetComponent<ListAllAvailable>().ListAllItemsInInv();
        
        /*
        string[] fileEntries = System.IO.Directory.GetFiles("Assets/ScriptableObjectAssets/Items/");
        foreach (string fileEntry in fileEntries)
        {
            if(fileEntry.EndsWith(".asset"))
                items.Add(AssetDatabase.LoadAssetAtPath<Item>(fileEntry));
        }
*/


    }

    #endregion


    public List<Item> items = new List<Item>();

    private void LoadItems()
    {
        List<string> _keys = new List<string>() { "items" };
        

        loadHandle = Addressables.LoadAssetsAsync<Item>(
            _keys,
            addressable =>
            {
               items.Add(addressable);
            }, Addressables.MergeMode.Union,
            false);
    }
}
