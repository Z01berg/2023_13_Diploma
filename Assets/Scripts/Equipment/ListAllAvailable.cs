using UnityEngine;

/**
 * Publiczna klasa listujaca wszystkie itemy z listy inventory i dodajaca je do okna inventory w UI
 *  parent - okno inventory w UI
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

            slot.GetComponent<ItemSlot>().allowedItemType = ItemType.any;

            var item = Instantiate(itemUIPrefab);
            item.transform.SetParent(slot.transform);
            item.GetComponent<UnityEngine.UI.Image>().sprite = x.icon;
            var d = item.GetComponent<UIItemDragNDrop>();
            d.item = x;
            d.parentTransform = slot.transform;
            d.originParentTransform = slot.transform;

        }
    }
}
