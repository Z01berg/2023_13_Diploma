using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{

    public List<GameObject> cardsInHand;
    public GameObject cardPrefab;
    private static GameObject hand;
    private bool cardsSpawned;

    private int cardsAmount;
    // private int cardsAmount;
    
    void Start()
    {
        cardsInHand = GameObject.FindGameObjectsWithTag("Card").ToList();
        foreach (var c in cardsInHand)
        {
            c.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 240);
        }

        hand = gameObject;
        cardsAmount = gameObject.transform.childCount;

        // cardsAmount = cardsInHand.Count;
        // InstantiateCardsInHand();
    }

    private void Update()
    {
        if (cardsAmount != gameObject.transform.childCount)
        {
            UpdateHandLayout();
            cardsAmount = gameObject.transform.childCount;
        }
    }

    public void InstantiateCardsInHand()
    {
        // var cards = Equipment.Instance.cards;
        if (!cardsSpawned) return;

        
        foreach(var card in cardsInHand)
        {
            var prefabCard = Instantiate(cardPrefab);
            // prefabCard.GetComponent<CardDisplay>().cardSO = GetComponent<>().cardSO;
            prefabCard.transform.SetParent(hand.transform);

            cardsSpawned = true;
        }
    }

    public void UpdateHandLayout()
    {
        // LayoutRebuilder.ForceRebuildLayoutImmediate(hand.GetComponent<RectTransform>());
        // // Canvas.ForceUpdateCanvases();

        Debug.Log("LayoutUpdated");
        StartCoroutine(DelayedMethod());
                

    }
    
    IEnumerator DelayedMethod()
    {
        yield return new WaitForSeconds(0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(hand.GetComponent<RectTransform>());
        
    }

    public void ChangeTurn()
    {
        CardUse.isPlayersTurn = !CardUse.isPlayersTurn;
    }
}
