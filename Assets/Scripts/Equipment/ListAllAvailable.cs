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

    public void ListAllItemsInInv()
    {
        foreach (var x in Inventory.Instance.items)
        {
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

        }
    }
}
