using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    private GameObject Canvas;
    private GameObject zoomedCard;
    // public GridLayoutGroup gridLayoutGroup;
    // public GridLayout grid;
    // public HorizontalLayoutGroup HorizontalLayoutGroup;
    private void Awake()
    {
        Canvas = GameObject.Find("Game Canvas");
        resizeUnlock();
    }

    public void resizeUnlock()
    {
       
    }
    
    
    
    public void OnMouseEnter()
    {
        // zoomedCard = Instantiate(gameObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250),
            // Quaternion.identity);
        // zoomedCard.transform.SetParent(Canvas.transform, false);
        // zoomedCard.layer = LayerMask.NameToLayer("Zoom");
        transform.localScale= new Vector2(1.5f, 1.5f);
        transform.position += new Vector3(-65, 130, 0);
        
        // Debug.Log("entered");
        

    }

    public void OnMouseExit()
    {
        transform.localScale= new Vector2(1f, 1f);
        transform.position -= new Vector3(-65, 130, 0);
        // Debug.Log("exit");
        
    }
}

