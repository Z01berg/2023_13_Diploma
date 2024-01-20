using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public GameObject[] cards;
    public Transform container;
    public Transform  minPos, maxPos;
    public static int maxAmountOfCards = 10;
    
    private List<int> order = new List<int>();
    private Vector3[] cardPosisionsArray = new Vector3[maxAmountOfCards];
    private int[] cardPositionsOrder = {5, 4, 6, 3, 7, 2, 8, 1, 9};
    void Start()
    {
        CountPositions();
        
        cards = GameObject.FindGameObjectsWithTag("Card");
        SetCardsInPosition();
    }

    public void CountPositions()
    {
        Vector2 distanceBetweenCards = (maxPos.position - minPos.position)/maxAmountOfCards;
        Debug.Log(distanceBetweenCards);
        for (int i = 1; i < cardPosisionsArray.Length; i++)
        {
            cardPosisionsArray[i] = (Vector2)minPos.position + (distanceBetweenCards * i);
        }
    }

    public void SetCardsInPosition()
    {
        
        for (int i = 0; i < cards.Length; i++)
        {
            Debug.Log("Pozycja " + cardPositionsOrder[i]);
            cards[i].transform.position = new Vector3(cardPosisionsArray[cardPositionsOrder[i]].x,
                cardPosisionsArray[cardPositionsOrder[i]].y, cardPositionsOrder[i]);
            cards[i].transform.SetSiblingIndex(i);
            order.Add(cardPositionsOrder[i]);
            // Debug.Log("Index " + cards[i].transform.GetSiblingIndex());
        }
        
        order.Sort();
        
        for (int i = 0; i < order.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(order.IndexOf(cardPositionsOrder[i]));
            Debug.Log(cards[i].transform.GetSiblingIndex());
        }
        
        
    }
}
