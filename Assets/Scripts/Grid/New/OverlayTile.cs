using System;
using UnityEngine;

namespace Grid.New
{
    public class OverlayTile : MonoBehaviour
    {
        public OverlayTile peviousTile;
        public Vector3 gridLocation;
        [NonSerialized] public bool isRangeTile;
        public Vector2 grid2DLocation => new Vector2(gridLocation.x, gridLocation.y);
        
        
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                HideTile();
            }
        }

        public void HideTile()
        {
            if (!isRangeTile)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(236, 91, 86, 0);
            }
        }
        
        public void ShowTile()
        {
            if (!isRangeTile)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(230, 233, 234, 190);
            }
        }
        public void ShowRangeTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(140, 35, 115, 110);
            isRangeTile = true;
        }
        public void HideRangeTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(1, 1, 1, 0);
            isRangeTile = false;
            
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