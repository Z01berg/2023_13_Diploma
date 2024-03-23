using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/**
 * The class keeps track of item slots and gives a player default cards if a certain
 * part of equipment has not been assigned, or takes them away when if it has been assigned.
 * 
 * It contains public classes:
 *  DisplayCards() - displayes default cards assigned to a slot
 *  HideCards() - hides default cards assigned to a slot
 *  AssignCard() - adds card to a list of all cards contained by slot
 *  ClearList() - clears a list of all cards of a slot
 */

public class DefaultCards : MonoBehaviour
{
    public List<CardsSO> _cards = new();
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardsPanel;
    public List<GameObject> _cardsList = new();
    private bool _defaultAdded = true;

    private void Awake()
    {
        if(transform.childCount < 1)
        {
            DisplayCards();
        }
    }

    private void Update()
    {
        if (transform.childCount > 0 && _defaultAdded)
        {
            HideCards();
            _defaultAdded = false;
        }
        else if(transform.childCount < 1 && !_defaultAdded)
        {
            DisplayCards();
            _defaultAdded = true;
        }
    }

    private void DisplayCards()
    {
        _cardsList = new();
        foreach (var c in _cards)
        {
            var card = Instantiate(cardPrefab);
            card.GetComponent<CardDisplay>().cardSO = c;
            Destroy(card.GetComponent<CardZoom>());
            Destroy(card.GetComponent<CardUse>());
            card.transform.SetParent(cardsPanel.transform);
            _cardsList.Add(card);
        }
    }

    private void HideCards()
    {
        foreach(var c in _cardsList)
        {
            Destroy(c);
        }
    }

    public void AssignCard(CardsSO card)
    {
        _cards.Add(card);
    }

    public void ClearList()
    {
        _cards = new();
    }
}
