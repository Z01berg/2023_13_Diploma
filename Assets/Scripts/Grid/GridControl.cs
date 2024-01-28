using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridControl : MonoBehaviour
    {

        [SerializeField] private Tilemap _targetTilemap;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPosition = _targetTilemap.WorldToCell(worldPoint);
                Debug.Log(clickPosition);
            }
        }
    }
}
