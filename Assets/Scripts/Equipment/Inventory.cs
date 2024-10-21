using System;
using System.Collections.Generic;
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

    private AsyncOperationHandle<IList<Item>> _loadHandle;

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
        _loadHandle.WaitForCompletion();

        foreach (var item in items) 
        {
            item.accessibility = Enum.Parse<Accessibility>(PlayerPrefs.GetString(item.itemName));
        }

        items.RemoveAll(x  => x.accessibility == Accessibility.locked);

        GameObject.Find("ItemsPanel").GetComponent<ListAllAvailable>().ListAllItemsInInv();
        
    }

    #endregion


    public List<Item> items = new List<Item>();

    private void LoadItems()
    {
        List<string> _keys = new List<string>() { "Item" };
        items.Clear();

        _loadHandle = Addressables.LoadAssetsAsync<Item>(
            _keys,
            addressable =>
            {
               items.Add(addressable);
            }, Addressables.MergeMode.Intersection,
            false);
        
    }
}
