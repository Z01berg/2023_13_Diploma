using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/**
 * Publiczna klasa pozwalajaca na asynchroniczne pobieranie assetow w trakcie dzialania aplikacji. 
 * Do klasy do³¹czona jest klasa enum sk³adaj¹ca siê z tagów jakie mo¿na nadawaæ assetom lub po których je pobieraæ.
 * 
 * Klasa sk³ada siê z 3 metod:
 *  - LoadItems zwraca liste objektow otagowanych podanymi, jako jeden z argumentow, tagami
 *  - GetRandomItem zwraca pojedynczy losowy item nie posiadany jeszcze przez gracza
 *  - ItemsWithNames dodaje do inventory i ekwipunku przedmioty podane wewn¹trz SaveTemplate save
 */

public enum AddressablesTags
{
    DefenceCard,DefaultCard,MovementCard,AttackCard,Item,AttackCardsBanner,CurseCard,unlocked,locked
}

public class AddressablesUtilities : MonoBehaviour
{
    public static List<object> LoadItems(Addressables.MergeMode mergeMode = Addressables.MergeMode.Union, params AddressablesTags[] addressablesTags)
    {
        if (addressablesTags.Length == 0) return null;

        List<object> result = new List<object>();

        List<string> _keys = new List<string>();
        
        // konwertowanie z enum na string
        foreach (AddressablesTags tag in addressablesTags)
        {
            _keys.Add(tag.ToString());
        }

        AsyncOperationHandle<IList<object>> _loadHandle;

        // uruchamianie asynchronicznego pobierania assetow otagowanych podanymi tagami
        _loadHandle = Addressables.LoadAssetsAsync<object>(
            _keys,
            addressable =>
            {
                result.Add(addressable);
            }, mergeMode,
            false);

        // oczekiwanie na wynik
        _loadHandle.WaitForCompletion();

        return result;
    }

    public static void GetRandomItem()
    {
        // pobieranie wszystkich itemow
        var items = LoadItems(Addressables.MergeMode.Union,AddressablesTags.Item );
        var inventory = Inventory.Instance;

        // usowanie przedmiotow posiadanych przez gracza
        for (int i = 0; i < items.Count; i++)
        {
            var convItem = items[i] as Item;
            if (inventory.items.Contains(convItem))
            {
                items.Remove(items[i]);
                i--;
            }
        }
        if (items.Count <= 0) return;
        var index = UnityEngine.Random.Range(0,items.Count);
        if (index < 0)
        {
            return;
        }

        // convertowanie z object na Item
        var result = items[index] as Item;

        // dodawanie itemu do inventory
        Inventory.Instance.items.Add(result);
        if (!Inventory.Instance._itemsPanel.activeInHierarchy)
        {
            Inventory.Instance.notDisplayedYet.Add(result);
            return;
        }
        Inventory.Instance._itemsPanel.GetComponent<ListAllAvailable>().AddItemToList(result);
    }

    public static void ItemsWithNames(SaveTemplate save)
    {
        Inventory.Instance.items.Clear();

        // pobieranie wszystkich itemow
        var items = LoadItems(Addressables.MergeMode.Union, AddressablesTags.Item);
        
        // dla kazdego itemu dodajemy go do inventory i jesli trzeba, odpowiedniego miejsca w ekwipunku
        foreach (var item in items) 
        {
            if(save.inventory.Contains((item as Item).itemName))
            {
                Inventory.Instance.items.Add(item as Item);
            }
            if (save.head != null && save.head == (item as Item).itemName) 
            {
                Equipment.Instance.head = item as Item;
            }
            else if (save.chest != null && save.chest == (item as Item).itemName)
            {
                Equipment.Instance.chest = item as Item;
            }
            else if (save.legs != null && save.legs == (item as Item).itemName)
            {
                Equipment.Instance.legs = item as Item;
            }
            else if (save.boots != null && save.boots == (item as Item).itemName)
            {
                Equipment.Instance.boots = item as Item;
            }
            else if (save.rightHand != null && save.rightHand == (item as Item).itemName)
            {
                Equipment.Instance.rightHand = item as Item;
            }
            else if (save.leftHand != null && save.leftHand == (item as Item).itemName)
            {
                Equipment.Instance.leftHand = item as Item;
            }
            else if (save.item1 != null && save.item1 == (item as Item).itemName)
            {
                Equipment.Instance.item1 = item as Item;
            }
            else if (save.item2 != null && save.item2 == (item as Item).itemName)
            {
                Equipment.Instance.item2 = item as Item;
            }
            else if (save.item3 != null && save.item3 == (item as Item).itemName)
            {
                Equipment.Instance.item3 = item as Item;
            }
            else if (save.item4 != null && save.item4 == (item as Item).itemName)
            {
                Equipment.Instance.item4 = item as Item;
            }
            else if (save.item5 != null && save.item5 == (item as Item).itemName)
            {
                Equipment.Instance.item5 = item as Item;
            }
            else if (save.item6 != null && save.item6 == (item as Item).itemName)
            {
                Equipment.Instance.item6 = item as Item;
            }
        }
        // jesli okno inventory jest aktywne to dodaj item do okna
        if (!Inventory.Instance._itemsPanel.activeInHierarchy) return;
        Inventory.Instance._itemsPanel.GetComponent<ListAllAvailable>().ListAllItemsInInv();
    }
}
