using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
            displayCards();
        }
    }

    private void Update()
    {
        if (transform.childCount > 0 && _defaultAdded)
        {
            hideCards();
            _defaultAdded = false;
        }
        else if(transform.childCount < 1 && !_defaultAdded)
        {
            displayCards();
            _defaultAdded = true;
        }
    }

    private void displayCards()
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

    private void hideCards()
    {
        foreach(var c in _cardsList)
        {
            Destroy(c);
        }
    }

    public void assignCard(CardsSO card)
    {
        _cards.Add(card);
    }

    public void clearList()
    {
        _cards = new();
    }
}
