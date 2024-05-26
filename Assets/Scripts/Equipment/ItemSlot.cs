using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Klasa pozwala na upuszczanie przedmiotow na odpowiednie sloty ekwipunku.
 * 
 * allowedItemType - przechowuje informacje na temat typu itemu przyjmowanego przez dany slot
 * itemUIPrefab - prefab ojektu Item UI
 * 
 * w momencie aktywacji publicznej funkcji OnDrop() sprawdzany jest dozwolony typ itemu, jesli 
 * sie zgadza to item przypisywany jest do danego slotu w ekwipunku.
 */

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static List<ItemSlot> allItemSlots = new List<ItemSlot>();

    public ItemType allowedItemType;
    public GameObject itemUIPrefab;

    [SerializeField] private Color _mouseOverColor = Color.green;
    private Color _originalColor;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        _originalColor = _image.color;
    }

    private void OnEnable()
    {
        allItemSlots.Add(this);
    }

    private void OnDisable()
    {
        allItemSlots.Remove(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<UIItemDragNDrop>().item.itemType == allowedItemType && transform.childCount ==0) 
        {
            eventData.pointerDrag.transform.SetParent(transform, false);
            eventData.pointerDrag.GetComponent<RectTransform>().position = transform.GetComponent<RectTransform>().position;
        }
    }

    private void LookForSlotsOfType(string command)
    {
        if (allowedItemType == ItemType.any)
        {
            if (transform.childCount == 0) return;
            ItemType childType = GetComponentInChildren<UIItemDragNDrop>().item.itemType;
            foreach (ItemSlot slot in allItemSlots)
            {
                if (slot == this) continue;
                if (slot.allowedItemType == childType)
                {
                    if (command == "lightup")
                    {
                        slot.LightUp();
                    }
                    else
                    {
                        slot.LightDown();
                    }
                }
            }
        }
        else
        {
            foreach (ItemSlot slot in allItemSlots)
            {
                if (slot == this) continue;
                if (slot.gameObject.transform.childCount > 0)
                {
                    if (slot.GetComponentInChildren<UIItemDragNDrop>().item.itemType != allowedItemType) continue;
                    if (slot.allowedItemType != ItemType.any) continue;
                    if (command == "lightup")
                    {
                        slot.LightUp();
                    }
                    else
                    {
                        slot.LightDown();
                    }
                }
                else
                {
                    slot.LightDown();
                }
            }
            if (transform.childCount == 0)
            {
                foreach(var card in GetComponent<DefaultCards>()._cardsList)
                {
                    if (command == "lightup")
                    {
                        card.transform.Find("ArtworkMask").transform.Find("BG").GetComponent<Image>().color = _mouseOverColor;
                        card.transform.SetAsFirstSibling();
                    }
                    else
                    {
                        card.transform.Find("ArtworkMask").transform.Find("BG").GetComponent<Image>().color = Color.white;
                    }
                }
            }
            else
            {
                var child = GetComponentInChildren<UIItemDragNDrop>();
                foreach (var card in child.cardsList)
                {
                    if (command == "lightup")
                    {
                        card.transform.Find("ArtworkMask").transform.Find("BG").GetComponent<Image>().color = _mouseOverColor;
                        card.transform.SetAsFirstSibling();
                    }
                    else
                    {
                        card.transform.Find("ArtworkMask").transform.Find("BG").GetComponent<Image>().color = Color.white;
                    }
                }
            }
            
        }
    }

    protected void LightUp()
    {
        _image.color = _mouseOverColor;
    }

    protected void LightDown()
    {
        _image.color = _originalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LookForSlotsOfType("lightup");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LookForSlotsOfType("lightdown");
    }
}
