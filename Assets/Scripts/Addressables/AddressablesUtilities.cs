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

    public static void SaveAsUnlockedItems()
    {
        
        var inventory = Inventory.Instance ?? throw new Exception("No inventory exists");

        foreach (var item in inventory.items) 
        {
            item.accessibility = Accessibility.unlocked;
            PlayerPrefs.SetString(item.itemName, item.accessibility.ToString());
        }
        PlayerPrefs.Save();
    }

    public static void LockAllItems()
    {
        var inventory = Inventory.Instance ?? throw new Exception("No inventory exists");

        foreach (var item in inventory.items)
        {
            item.accessibility = Accessibility.locked;
            PlayerPrefs.SetString(item.itemName,item.accessibility.ToString());
        }
        Inventory.Instance = new();
        PlayerPrefs.Save();
    }

    public static Item GetRandomItem()
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

        var index = UnityEngine.Random.Range(0,items.Count);
        if (index < 0)
        {
            return null;
        }
        var result = items[index] as Item;

        return result;
    }
}
