using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    
    public List<GameObject> cards = new List<GameObject>();
    public GameObject cardPrefab;

    public Transform hand;
    // Start is called before the first frame update
    void Start()
    {
        AddCardToHand();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCardToHand()
    {
        var prefabCard = Instantiate(cardPrefab);
        var card = cards[0];
        cards.RemoveAt(0);
        // card.GetComponent<CardDisplay>().cardSO = card;
        card.transform.SetParent(hand);
        
        
        // foreach (var card in cards)
        // {
        //     
        // }
    }
}
