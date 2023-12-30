using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Equipment : MonoBehaviour
{

    #region Singleton

    public static Equipment Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of Equipment found!");
            return;
        }
        Instance = this;
    }

    #endregion

    public List<Item> equipment = new List<Item>();

    public void Add(Item item)
    {
        equipment.Add(item);
    }

    public void Remove(Item item)
    {
        equipment.Remove(item);
    }
}
