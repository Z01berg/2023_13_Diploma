using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Klasa pozwala na upuszczanie przedmiotow na odpowiednie sloty ekwipunku.
 * 
 * allowedItemType - przechowuje informacje na temat typu itemu przyjmowanego przez dany slot
 * itemUIPrefab - prefab ojektu Item UI
 * 
 * w momencie aktywacji publicznej funkcji OnDrop() sprawdzany jest dozwolony typ itemu, jesli 
 * sie zgadza to item przypisywany jest do danego slotu w ekwipunku.
 */

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public ItemType allowedItemType;
    public GameObject itemUIPrefab;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<UIItemDragNDrop>().item.itemType == allowedItemType && transform.childCount ==0) 
        {
            eventData.pointerDrag.transform.SetParent(transform, false);
            eventData.pointerDrag.GetComponent<RectTransform>().position = transform.GetComponent<RectTransform>().position;
        }
    }
}
