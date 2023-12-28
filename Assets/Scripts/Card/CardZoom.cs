using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    private GameObject Canvas;
    private GameObject zoomedCard;
    private void Awake()
    {
        Canvas = GameObject.Find("Game Canvas");
    }

    public void OnMouseEnter()
    {
        zoomedCard = Instantiate(gameObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250),
            Quaternion.identity);
        zoomedCard.transform.SetParent(Canvas.transform, false);
        zoomedCard.layer = LayerMask.NameToLayer("Zoom");

        RectTransform rectTransform = zoomedCard.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(250, 375);
    }

    public void OnMouseExit()
    {
        Destroy(zoomedCard);
    }
}
