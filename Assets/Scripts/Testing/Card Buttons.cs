using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class CardButtons : MonoBehaviour
{
    public GameObject card;
    public Transform hand;

    public void AddCard()
    { 
        Instantiate(card, hand, true).transform.SetParent(hand);
        
    }

    public void DeleteCard()
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        Destroy(cards[cards.Length-1]);
    }
}
