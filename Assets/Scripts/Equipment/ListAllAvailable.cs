using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ListAllAvailable : MonoBehaviour
{

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject itemUIPrefab;

    void Start()
    {
        foreach(var x in Inventory.Instance.items)
        {
            var slot = Instantiate(slotPrefab);
            slot.transform.SetParent(parent);

            var item = Instantiate(itemUIPrefab);
            item.transform.SetParent(slot.transform);
            item.GetComponent<UnityEngine.UI.Image>().sprite = x.icon;
            //var img = item.GetComponentsInChildren<UnityEngine.UI.Image>();
            //foreach(var y in img)
            //{
            //    if(y.gameObject.transform.parent != null)
            //        y.sprite = x.icon;
            //}
            
        }
    }

    
}
