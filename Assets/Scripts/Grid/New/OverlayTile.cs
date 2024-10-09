using System;
using UnityEngine;

namespace Grid.New
{
    public class OverlayTile : MonoBehaviour
    {
        public OverlayTile peviousTile;
        public Vector3 gridLocation;
        public Vector2 grid2DLocation => new Vector2(gridLocation.x, gridLocation.y);
        
        
        void Update()
        {
            if (Input.GetMouseButtonDown(1)){}
            
                // HideTile();
            
        }

        public void HideTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            //gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0.5f);

        }
        
        public void ShowTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 100, 100, 1);
        }
        public void ShowRangeTile()
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = new Color(241, 224, 92, 0.7f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(236, 91, 86, 80);
            Debug.Log("should work");
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                UnityEngine.Debug.Log(transform.position);
            }
        }
    }
}