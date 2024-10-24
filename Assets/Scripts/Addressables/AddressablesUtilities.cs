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
        
        for (int i = 0; i < items.Count; i++)
        {
            if (Inventory.Instance.items.Contains(items[i] as Item))
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

    public static void ItemsWithNames(List<string> names)
    {
        Inventory.Instance.items.Clear();

        var items = LoadItems(Addressables.MergeMode.Union, AddressablesTags.Item);

        foreach (var item in items) 
        {
            if(names.Contains((item as Item).itemName))
            {
                Inventory.Instance.items.Add(item as Item);
            }
        }
        if (!Inventory.Instance._itemsPanel.activeInHierarchy) return;
        Inventory.Instance._itemsPanel.GetComponent<ListAllAvailable>().ListAllItemsInInv();
    }
}
