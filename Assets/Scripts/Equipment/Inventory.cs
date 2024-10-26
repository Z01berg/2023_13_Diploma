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
    public GameObject _itemsPanel;
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

        SaveSystem.LoadGame();
    }

    #endregion

    public List<Item> notDisplayedYet = new List<Item>();

    public List<Item> items = new List<Item>();
    /*
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

        _loadHandle.WaitForCompletion();

        _itemsPanel.GetComponent<ListAllAvailable>().ListAllItemsInInv();
    }
    */
}
