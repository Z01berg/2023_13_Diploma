using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ListAllAvailable : MonoBehaviour
{

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;

    void Start()
    {
        foreach(var x in Inventory.Instance.items)
        {
            var item = Instantiate(prefab);
            item.transform.SetParent(parent);
            var img = item.gameObject.GetComponentInChildren<UnityEngine.UI.Image>();
            img.sprite = x.icon;
        }
    }

    
}
