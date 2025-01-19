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
 * 
 * Klasa posiada metody:
 *  - OnDrop przypisuje przedmiot do slotu na ktury zostal upuszczony jesli jest to slot w ekwipunku
 *  - LookForSlotsOfType szuka slotow i kart do podswietlenia lub przywrucenia koloru
 *  - LightUp podswietla sloty poprzez zmiane koloru na _mouseOverColor
 *  - LightDown przywraca slotom podstawowy kolor
 *  - OnPointerEnter wywoluje LookForSlotsOfType kiedy kursor znajdzie sie wewnatrz slotu
 *  - OnPointerExit wywoluje LookForSlotsOfType kiedy kursor wyjdzie poza slot
 *  - DoubleClicked przenosi item wewnatrz slotu do pierwszego znalezionego slotu jaki uda sie znalezc
 */

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static List<ItemSlot> allItemSlots = new List<ItemSlot>();

    // typ ItemType.any jesli slot jest w panelu itemow
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
        // czy typ itemu zgadza sie z typem slotu i czy slot jest juz zajety
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
        // czy slot jest w panelu itemow
        if (allowedItemType == ItemType.any)
        {
            // czy slot jest pusty
            if (transform.childCount == 0) return;
            ItemType childType = GetComponentInChildren<UIItemDragNDrop>().item.itemType;
            foreach (ItemSlot slot in allItemSlots)
            {
                if (slot == this) continue;
                // czy znaleziony slot jest tego samego typu co item
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
            return;
        }

        foreach (ItemSlot slot in allItemSlots)
        {
            if (slot == this) continue;
            if (slot.gameObject.transform.childCount > 0)
            {
                // czy typ itemu zgadza sie z typem slotu
                if (slot.GetComponentInChildren<UIItemDragNDrop>().item.itemType != allowedItemType) continue;
                // czy slot jest w panelu itemow
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
        // czy slot ma item
        if (transform.childCount == 0)
        {
            // przechodzi przez karty w itemie
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
        // ustaw bazowy kolor dla wszystkich slotow
        foreach (ItemSlot itemSlot in allItemSlots)
        {
            itemSlot.LightDown();
        }
        // czy slot jest w panelu ekwipunku
        if (allowedItemType != ItemType.any)
        {
            // czy ma przypisany item
            if (transform.childCount == 1)
            {
                // item przenosi sie z powrotem do panelu itemow
                transform.GetChild(0).GetComponent<UIItemDragNDrop>().OnItemChangePlace(true);
            }
        }
        if (transform.childCount == 0) return;

        foreach (ItemSlot itemSlot in allItemSlots)
        {
            // czy zgadza sie typ itemu i slotu, czy znaleziony slot ma dzieci, czy przezucic item nawet jak slot jest pelen
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
