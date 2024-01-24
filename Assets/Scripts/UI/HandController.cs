using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public List<GameObject> cardsInHand;
    public GameObject cardPrefab;
    public Transform hand;
    private bool cardsSpawned;
    
    void Start()
    {
        cardsInHand = GameObject.FindGameObjectsWithTag("Card").ToList();
        foreach (var c in cardsInHand)
        {
            c.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 240);
        }
        // InstantiateCardsInHand();
    }

    public void InstantiateCardsInHand()
    {
        // var cards = Equipment.Instance.cards;
        if (!cardsSpawned) return;

        
        foreach(var card in cardsInHand)
        {
            var prefabCard = Instantiate(cardPrefab);
            // prefabCard.GetComponent<CardDisplay>().cardSO = GetComponent<>().cardSO;
            prefabCard.transform.SetParent(hand);

            cardsSpawned = true;
        }
    }
  
}
