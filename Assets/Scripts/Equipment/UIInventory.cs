using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Publiczna klasa sluzaca do zapisywania oraz wczytywania ekwipunku bedacego w uzytku przez gracza.
 * 
 * Posiada zmienne prywatne:
 * - _cardsPanel: powiazany z oknem kart w UI
 * - _itemsPanel: powiazane z oknem inventory w UI
 * - _equipmentPanel: powiazane z oknem equipment w UI
 * 
 * Funkcja SaveState() zapisuje aktualny stan ekwipunku oraz dodaje karty do decku, w tym celu przeszukiwane s¹
 * wszystkie sloty ekwipunku w oknie UI a nastepnie znalezione w nich itemy przypisywane sa do objektu Equipment
 * w odpowiednie zmienne. W razie braku przedmiotu w slocie przepisywana jest wartosc null.
 */

public class UIInventory : MonoBehaviour
{
    [SerializeField] private GameObject _cardsPanel;
    [SerializeField] private GameObject _itemsPanel;
    [SerializeField] private GameObject _equipmentPanel;
    [SerializeField] private GameObject _deck;

    private DeckController _deckController;

    private void Start()
    {
        _deckController = _deck.GetComponent<DeckController>();
    }

    public void SaveState()
    {
        Equipment.Instance.cards.Clear();

        foreach (Transform card in _cardsPanel.transform)
        {
            Equipment.Instance.cards.Add(card.GetComponent<CardDisplay>().cardSO);
        }

        _deckController.ManageDeck();
        
        GameObject slot = _equipmentPanel.gameObject.transform.Find("HeadSlot").gameObject;
        if(slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.head = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.head = null;

        slot = _equipmentPanel.gameObject.transform.Find("ChestSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.chest = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.chest = null;

        slot = _equipmentPanel.gameObject.transform.Find("LegsSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.legs = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.legs = null;

        slot = _equipmentPanel.gameObject.transform.Find("BootsSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.boots = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.boots = null;

        slot = _equipmentPanel.gameObject.transform.Find("RightHandSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.rightHand = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.rightHand = null;

        slot = _equipmentPanel.gameObject.transform.Find("LeftHandSlot").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.leftHand = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.leftHand = null;

        slot = _equipmentPanel.gameObject.transform.Find("ItemSlot1").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item1 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item1 = null;

        slot = _equipmentPanel.gameObject.transform.Find("ItemSlot2").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item2 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item2 = null;

        slot = _equipmentPanel.gameObject.transform.Find("ItemSlot3").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item3 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item3 = null;

        slot = _equipmentPanel.gameObject.transform.Find("ItemSlot4").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item4 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item4 = null;

        slot = _equipmentPanel.gameObject.transform.Find("ItemSlot5").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item5 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item5 = null;

        slot = _equipmentPanel.gameObject.transform.Find("ItemSlot6").gameObject;
        if (slot.GetComponentInChildren<UIItemDragNDrop>() != null)
            Equipment.Instance.item6 = slot.GetComponentInChildren<UIItemDragNDrop>().item;
        else
            Equipment.Instance.item6 = null;
    }
    
}
