using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.VersionControl;
using UnityEditor;
using Unity.Mathematics;

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
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        
        var inventory = Inventory.Instance ?? throw new Exception("No inventory exists");

        foreach (var item in inventory.items) 
        {
            string assetPath = AssetDatabase.GetAssetPath(item);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);

            var group = settings.FindGroup("items");
            var entry = settings.FindAssetEntry(assetGUID);

            if (entry != null)
            {
                entry.labels.Add("unlocked");
                entry.labels.Remove("locked");
            }
        }
    }

    public static void LockAllItems()
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        var inventory = Inventory.Instance ?? throw new Exception("No inventory exists");

        foreach (var item in inventory.items)
        {
            string assetPath = AssetDatabase.GetAssetPath(item);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);

            var group = settings.FindGroup("items");
            var entry = settings.FindAssetEntry(assetGUID);

            if (entry != null)
            {
                entry.labels.Remove("unlocked");
                entry.labels.Add("locked");
            }
        }
    }

    public static Item GetRandomItem()
    {
        var items = LoadItems(Addressables.MergeMode.Union,AddressablesTags.Item, AddressablesTags.locked);
        
        for (int i = 0; i < items.Count; i++)
        {
            if (Inventory.Instance.items.Contains(items[i] as Item))
            {
                items.Remove(items[i]);
                i--;
            }
        }

        var index = UnityEngine.Random.Range(0,items.Count);
        var result = items[index] as Item;

        return result;
    }
}
