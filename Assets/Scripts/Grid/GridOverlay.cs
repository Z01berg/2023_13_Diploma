using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridOverlay : MonoBehaviour
    {
        private Renderer _gridOverlayTilemapRenderer;
        private bool _checkGridOverlay;
        
        void Start()
        {
            _gridOverlayTilemapRenderer = transform.GetComponent<Tilemap>().GetComponent<Renderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _gridOverlayTilemapRenderer.enabled = !_gridOverlayTilemapRenderer.enabled;
            }
        }
    }
}
