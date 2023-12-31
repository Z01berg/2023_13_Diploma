using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{

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
    }

    #endregion

    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        //items.Add(item);
    }

    public void Remove(Item item)
    {
        //items.Remove(item);
    }
}
