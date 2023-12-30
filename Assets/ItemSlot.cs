using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public ItemType allowedItemType;
    public GameObject itemUIPrefab;
    private GameObject weaponPanel;
    public GameObject itemSlot;

    private void Start()
    {
        weaponPanel = GameObject.Find("ItemsPanel").gameObject;

        if (allowedItemType != ItemType.any)
        {
            foreach(var item in Equipment.Instance.equipment)
            {
                if(item.itemType == allowedItemType)
                {
                    var i = Instantiate(itemUIPrefab);
                    i.transform.SetParent(transform);

                    i.GetComponent<UnityEngine.UI.Image>().sprite = item.icon;
                    var d = i.GetComponent<UIItemDragNDrop>();
                    d.item = item;
                    d.parentTransform = transform;
                    i.GetComponent<RectTransform>().position = transform.GetComponent<RectTransform>().position;
                    
                    d.AddCardsToDeck();
                    
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<UIItemDragNDrop>().item.itemType == allowedItemType) 
        {
            //eventData.pointerDrag.GetComponent<UIItemDragNDrop>().AddCardsToDeck();
            eventData.pointerDrag.transform.SetParent(transform, false);
            eventData.pointerDrag.GetComponent<RectTransform>().position = transform.GetComponent<RectTransform>().position;
        }
    }
}
