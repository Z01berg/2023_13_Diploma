// using UnityEngine;
//
// public class Test : MonoBehaviour
// {
//     [SerializeField] private float _gridSize = 1f;
//     [SerializeField] private float _gridOpacity = 0.5f;
//     [SerializeField] private Color _gridColor = Color.gray;
//     [SerializeField] private UnityEngine.Camera _cam;
//
//     private void OnDrawGizmos()
//     {
//         DrawBackgroundGrid();
//     }
//
//     private void DrawBackgroundGrid()
//     {
//         Camera cam = _cam;
//
//         float camHeight = cam.orthographicSize * 2;
//         float camWidth = camHeight * cam.aspect;
//
//         int verticalLineCount = Mathf.CeilToInt((camWidth + _gridSize) / _gridSize);
//         int horizontalLineCount = Mathf.CeilToInt((camHeight + _gridSize) / _gridSize);
//
//         Gizmos.color = new Color(_gridColor.r, _gridColor.g, _gridColor.b, _gridOpacity);
//
//         Vector3 camPosition = cam.transform.position;
//
//         Vector3 gridOffset = new Vector3(camPosition.x % _gridSize, camPosition.y % _gridSize, 0);
//
//         for (int i = 0; i < verticalLineCount; i++)
//         {
//             Gizmos.DrawLine(new Vector3(_gridSize * i, -camHeight / 2, 0) + gridOffset,
//                 new Vector3(_gridSize * i, camHeight / 2, 0f) + gridOffset);
//         }
//
//         for (int i = 0; i < horizontalLineCount; i++)
//         {
//             Gizmos.DrawLine(new Vector3(-camWidth / 2, _gridSize * i, 0) + gridOffset,
//                 new Vector3(camWidth / 2, _gridSize * i, 0f) + gridOffset);
//         }
//
//         Gizmos.color = Color.white;
//     }
// }
// TODO delete me or use as gizmos only, show grid