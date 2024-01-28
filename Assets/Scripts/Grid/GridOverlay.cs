using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridOverlay : MonoBehaviour
    {
        private Tilemap _gridOverlayTilemap;

        void Start()
        {
            _gridOverlayTilemap = transform.Find("GridOverlay").GetComponent<Tilemap>();
            _gridOverlayTilemap.GetComponent<Renderer>().enabled = false;
        }
    }
}
