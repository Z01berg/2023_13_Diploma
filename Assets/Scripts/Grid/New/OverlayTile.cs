using System;
using UnityEngine;

namespace Grid.New
{
    public class OverlayTile : MonoBehaviour
    {
        public float G; // Distance from starting node
        public float H; // Distance from end node
        public float F => G + H;
        
        [NonSerialized]public OverlayTile Previous;
        public Vector3 gridLocation;
        [NonSerialized] private bool _isRangeTile;
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
            if (!_isRangeTile)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(236, 91, 86, 0);
            }
        }
        
        public void ShowTile()
        {
            if (!_isRangeTile)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(230, 233, 234, 190);
            }
        }
        public void ShowRangeTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(140, 35, 115, 110);
            _isRangeTile = true;
        }
        public void HideRangeTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(1, 1, 1, 0);
            _isRangeTile = false;
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(transform.position);
            }
        }
    }
}