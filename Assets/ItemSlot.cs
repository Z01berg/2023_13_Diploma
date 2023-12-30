using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public ItemType allowedItemType;
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
