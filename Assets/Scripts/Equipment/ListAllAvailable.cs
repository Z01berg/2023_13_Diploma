using System.Collections.Generic;
using UnityEngine;

/**
 * Publiczna klasa listujaca wszystkie itemy z listy inventory i dodajaca je do okna inventory w UI
 *  parent - okno inventory w UI
 *  slotPrefab - prefab okna na item
 *  itemUIPrefab - prefab itemu
 *  
 *  Funkcja ListAllItemsInInv() odpowiada za stworzenie slotu oraz przedmiotu, usuniecie i dodanie potrzebnych komponentow
 *  i dodanie ich do okna inventory w UI poprzez dodanie parenta.
 */

public class ListAllAvailable : MonoBehaviour
{

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject itemUIPrefab;
    [SerializeField] private List<GameObject> _eqSlots; 

    private void Start()
    {
        EventSystem.DisplayAllItems.AddListener(ListAllItemsInInv);
    }

    public void ListAllItemsInInv()
    {
        foreach (Transform child in parent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (var x in Inventory.Instance.items)
        {
            AddItemToList(x);
        }
    }

    public void AddItemToList(Item x)
    {
        var eq = Equipment.Instance;

        var slot = Instantiate(slotPrefab);
        slot.transform.SetParent(parent);
        slot.transform.localScale = Vector3.one;

        slot.GetComponent<ItemSlot>().allowedItemType = ItemType.any;

        var item = Instantiate(itemUIPrefab);
        item.transform.SetParent(slot.transform);
        item.transform.localScale = Vector3.one;
        item.GetComponent<UnityEngine.UI.Image>().sprite = x.icon;
        item.GetComponent<UnityEngine.UI.Image>().preserveAspect = true;
        var d = item.GetComponent<UIItemDragNDrop>();
        d.item = x;
        d.parentTransform = slot.transform;
        d.originParentTransform = slot.transform;

        if (
            (eq.leftHand != null && eq.leftHand.itemName == x.itemName) || 
            (eq.rightHand != null && eq.rightHand.itemName == x.itemName) || 
            (eq.boots != null && eq.boots.itemName == x.itemName) || 
            (eq.legs != null && eq.legs.itemName == x.itemName) || 
            (eq.chest != null && eq.chest.itemName == x.itemName) || 
            (eq.head != null && eq.head.itemName == x.itemName) || 
            (eq.item1 != null && eq.item1.itemName == x.itemName) || 
            (eq.item2 != null && eq.item2.itemName == x.itemName) || 
            (eq.item3 != null && eq.item3.itemName == x.itemName) || 
            (eq.item4 != null && eq.item4.itemName == x.itemName) || 
            (eq.item5 != null && eq.item5.itemName == x.itemName) || 
            (eq.item6 != null && eq.item6.itemName == x.itemName))
        {
            slot.GetComponent<ItemSlot>().DoubleClicked();
        }
    }
}
