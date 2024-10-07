using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum AddressablesTags
{
    DefenceCard,DefaultCard,MovementCard,AttackCard,Item,AttackCardsBanner,CurseCard
}

public class AddressablesUtilities : MonoBehaviour
{
    public static List<object> LoadItems(params AddressablesTags[] addressablesTags)
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
            }, Addressables.MergeMode.Union,
            false);

        _loadHandle.WaitForCompletion();

        return result;
    }
}
