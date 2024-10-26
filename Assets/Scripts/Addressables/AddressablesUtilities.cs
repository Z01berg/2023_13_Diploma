using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
       
        foreach (AddressablesTags tag in addressablesTags)
        {
            _keys.Add(tag.ToString());
        }

        AsyncOperationHandle<IList<object>> _loadHandle;

        _loadHandle = Addressables.LoadAssetsAsync<object>(
            _keys,
            addressable =>
            {
                result.Add(addressable);
            }, mergeMode,
            false);

        _loadHandle.WaitForCompletion();

        return result;
    }

    public static void GetRandomItem()
    {
        var items = LoadItems(Addressables.MergeMode.Union,AddressablesTags.Item );
        var inventory = Inventory.Instance;
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
        var result = items[index] as Item;

        Inventory.Instance.items.Add(result);
        if(!Inventory.Instance._itemsPanel.activeInHierarchy) return;
        Inventory.Instance._itemsPanel.GetComponent<ListAllAvailable>().AddItemToList(result);
    }

    public static void ItemsWithNames(SaveTemplate save)
    {
        Inventory.Instance.items.Clear();

        var items = LoadItems(Addressables.MergeMode.Union, AddressablesTags.Item);
        
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
        if (!Inventory.Instance._itemsPanel.activeInHierarchy) return;
        Inventory.Instance._itemsPanel.GetComponent<ListAllAvailable>().ListAllItemsInInv();
    }
}
