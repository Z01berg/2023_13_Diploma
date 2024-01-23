using UnityEngine;
using UnityEngine.Tilemaps;


namespace Grid
{
    public class GroundHighlighter : MonoBehaviour
    {

        private UnityEngine.Grid grid;
        [SerializeField] private Tilemap interacativeTilemap;
        [SerializeField] private Tile hoverTile;

        private Vector3Int previousMousePosition = new Vector3Int();

        private void Start()
        {
            grid = gameObject.GetComponent<UnityEngine.Grid>();
        }

        private void Update()
        {
            Vector3Int mousePosition = GetMousePosition();
            if (!mousePosition.Equals(previousMousePosition))
            {
                interacativeTilemap.SetTile(previousMousePosition, null);
                interacativeTilemap.SetTile(mousePosition, hoverTile);
                previousMousePosition = mousePosition;
            }
        }

        Vector3Int GetMousePosition()
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return grid.WorldToCell(mouseWorldPosition);
        }
        
    }
}