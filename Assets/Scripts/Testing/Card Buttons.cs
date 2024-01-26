using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class CardButtons : MonoBehaviour
{
    public GameObject card;
    public GameObject hand;
    private int _cardsInHand;
    

    public void AddCard()
    {
        // _cardsInHand = GameObject.FindGameObjectsWithTag("Card").Length;
        // if (_cardsInHand < 10)
        // {
        //     Instantiate(card, hand.transform, true).transform.SetParent(hand.transform);
        // }
        // HandController.UpdateHandLayout();

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
