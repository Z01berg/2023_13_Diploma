using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridOverlay : MonoBehaviour
    {
        private Tilemap gridOverlayTilemap;

        void Start()
        {
            gridOverlayTilemap = transform.Find("GridOverlay").GetComponent<Tilemap>();
            gridOverlayTilemap.GetComponent<Renderer>().enabled = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                ToggleGridOverlay();
            }
        }

        void ToggleGridOverlay()
        {
            gridOverlayTilemap.GetComponent<Renderer>().enabled = !gridOverlayTilemap.GetComponent<Renderer>().enabled;
        }
        
    }
}
