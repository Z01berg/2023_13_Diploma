using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUse : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject spot;
    void Start()
    {
        spot = GameObject.Find("Spot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button pressed on " + gameObject.name);
            transform.SetParent(spot.transform);
            transform.position = spot.transform.position;
        }
        
    }
}
