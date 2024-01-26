using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CardUse : MonoBehaviour
{

    private GameObject spot;
    private GameObject hand;
    public bool isCardInPlay = true;
    public static bool isPlayersTurn = true;

    void Start()
    {
        spot = GameObject.Find("Spot");
        hand = GameObject.Find("Hand");
    }

   

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && isCardInPlay && isPlayersTurn)
        {
            Debug.Log("Left mouse button pressed on " + gameObject.name);
            transform.SetParent(spot.transform);
            transform.position = spot.transform.position;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(144, 204);
            hand.transform.position += new Vector3(0, -300, 0);

        }else if (Input.GetMouseButtonDown(1) && isCardInPlay && isPlayersTurn)
        {
            Debug.Log("Right mouse button pressed on " + gameObject.name);
            transform.SetParent(hand.transform);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 240);
            // transform.localScale= new Vector2(1f, 1f);
            // transform.position = hand.transform.position;
            hand.transform.position += new Vector3(0, 300, 0);

        }

        

    }
}
