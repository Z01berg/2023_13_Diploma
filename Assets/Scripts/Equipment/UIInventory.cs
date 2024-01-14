using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private GameObject cardsPanel;
    [SerializeField] private GameObject itemsPanel;
    [SerializeField] private GameObject EquipmentPanel;

    public void SaveState()
    {
        Equipment.Instance.cards.Clear();

        foreach (Transform card in cardsPanel.transform)
        {
            Equipment.Instance.cards.Add(card.gameObject);
        }

        GameObject slot = EquipmentPanel.gameObject.transform.Find("HeadSlot").gameObject;
        if(slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.head = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.head = null;

        slot = EquipmentPanel.gameObject.transform.Find("ChestSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.chest = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.chest = null;

        slot = EquipmentPanel.gameObject.transform.Find("LegsSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.legs = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.legs = null;

        slot = EquipmentPanel.gameObject.transform.Find("BootsSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.boots = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.boots = null;

        slot = EquipmentPanel.gameObject.transform.Find("RightHandSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.rightHand = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.rightHand = null;

        slot = EquipmentPanel.gameObject.transform.Find("LeftHandSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.leftHand = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.leftHand = null;

        slot = EquipmentPanel.gameObject.transform.Find("ItemSlot1").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item1 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item1 = null;

        slot = EquipmentPanel.gameObject.transform.Find("ItemSlot2").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item2 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item2 = null;

        slot = EquipmentPanel.gameObject.transform.Find("ItemSlot3").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item3 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item3 = null;

        slot = EquipmentPanel.gameObject.transform.Find("ItemSlot4").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item4 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item4 = null;

        slot = EquipmentPanel.gameObject.transform.Find("ItemSlot5").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item5 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item5 = null;

        slot = EquipmentPanel.gameObject.transform.Find("ItemSlot6").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item6 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item6 = null;

        SceneManager.LoadScene("Z01berg");
    }

    public void LoadState()
    {
        Debug.LogWarning("NotImplemented");
    }
    
}
