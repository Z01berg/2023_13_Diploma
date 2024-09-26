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
            if (Input.GetMouseButtonDown(0))
            {
                HideTile();
            }
        }

        public void HideTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        
        public void ShowTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
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