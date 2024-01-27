using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class CardZoom : MonoBehaviour
{
    private GameObject zoomedCard;
    public bool isCardInHand = true;
    public bool IsCardInHand
    {
        set { isCardInHand = value; }
    }
    
    private void Update()
    {
        if (transform.parent.CompareTag("Hand"))
        {
            isCardInHand = true;
        }
        else
        {
            isCardInHand = false;
        }
    }

    public void OnMouseEnter()
    {

        if (isCardInHand)
        {
            transform.localScale= new Vector2(1.5f, 1.5f);
            transform.position += new Vector3(-65, 130, 0);
        }
    }

    public void OnMouseExit()
    {
        if (isCardInHand)
        {
            transform.localScale= new Vector2(1f, 1f);
            transform.position -= new Vector3(-65, 130, 0);
        }
        
        // Debug.Log("exit");
        
    }
}

