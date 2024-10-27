using System.Collections.Generic;
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
        
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<UIItemDragNDrop>().item.itemType == allowedItemType && transform.childCount == 0)
        {
            eventData.pointerDrag.GetComponent<UIItemDragNDrop>().BackToTempParent();
            eventData.pointerDrag.GetComponent<UIItemDragNDrop>()._tempOldParent = gameObject;
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
                        //slot.MakeTransparent();
                    }
                    else
                    {
                        slot.LightDown();
                    }
                }
            }
            return;
        }

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
                    //slot.MakeTransparent();
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
            foreach (var card in GetComponent<DefaultCards>()._cardsList)
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
            if (child.cardsList == null)
            {
                return;
            }
            foreach (var card in child.cardsList)
            {
                if (card == null) break;
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

    protected void LightUp()
    {
        if(_image == null) return;
        _image.color = _mouseOverColor;
    }

    protected void LightDown()
    {
        if (_image == null) return;
        _image.color = _originalColor;
        _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b,1);
    }

    protected void MakeTransparent()
    {
        if (_image == null) return;
        _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.4f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LookForSlotsOfType("lightup");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LookForSlotsOfType("lightdown");
    }

    public void DoubleClicked(bool force = false)
    {
        foreach (ItemSlot itemSlot in allItemSlots)
        {
            itemSlot.LightDown();
        }
        if (allowedItemType != ItemType.any)
        {
            if (transform.childCount == 1)
            {
                transform.GetChild(0).GetComponent<UIItemDragNDrop>().OnItemChangePlace(true);
            }
        }
        if (transform.childCount == 0) return;

        foreach (ItemSlot itemSlot in allItemSlots)
        {
            if (transform.GetChild(0).GetComponent<UIItemDragNDrop>().item.itemType == itemSlot.allowedItemType && (itemSlot.transform.childCount == 0 || force && itemSlot.transform.childCount <= 1))
            {
                var item = transform.GetChild(0);
                item.transform.SetParent(itemSlot.transform, false);
                item.GetComponent<RectTransform>().position = itemSlot.transform.GetComponent<RectTransform>().position;
                item.GetComponent<UIItemDragNDrop>().OnItemChangePlace(true);
                break;
            }
        }
    }
}
