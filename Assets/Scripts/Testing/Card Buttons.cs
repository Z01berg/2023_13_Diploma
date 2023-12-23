using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class CardButtons : MonoBehaviour
{
    public GameObject card;
    public Transform hand;
    private int _cardsInHand;
    

    public void AddCard()
    {
        _cardsInHand = GameObject.FindGameObjectsWithTag("Card").Length;
        if (_cardsInHand < 10)
        {
            Instantiate(card, hand, true).transform.SetParent(hand);
        }
    }

    public void DeleteCard()
    {
        
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        if (cards.Length > 0)
        {
           Destroy(cards[cards.Length-1]); 
        }
        
    }
}
