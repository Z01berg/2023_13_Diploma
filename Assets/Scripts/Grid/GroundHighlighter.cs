using UnityEngine;
using UnityEngine.Tilemaps;


namespace Grid
{
    public class GroundHighlighter : MonoBehaviour
    {
        private UnityEngine.Grid _grid;
        [SerializeField] private Tilemap interactiveTilemap;
        [SerializeField] private Tile hoverTile;
        [SerializeField] private Tile moveRangeTile;
        [SerializeField] private Tile skillRangeTile;

        private Vector3Int _previousMousePosition;
        private HighlightMode _currentHighlightMode = HighlightMode.SingleTile;
        private Transform _playerTransform;
        private Vector3Int _previousPlayerPosition;
        private Vector3Int _currentPlayerPosition;

        private enum HighlightMode
        {
            MoveRange,
            SkillRange,
            SingleTile
        }

        private void Start()
        {
            _grid = gameObject.GetComponent<UnityEngine.Grid>();
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _currentPlayerPosition = GetPlayerPosition();
            _previousPlayerPosition = _currentPlayerPosition;
        }

        private void Update()
        {
            Vector3Int mousePosition = GetMousePosition();
            _currentPlayerPosition = GetPlayerPosition();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _currentHighlightMode = HighlightMode.MoveRange;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentHighlightMode = HighlightMode.SkillRange;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _currentHighlightMode = HighlightMode.SingleTile;
            }

            if (_currentHighlightMode == HighlightMode.SkillRange || _currentHighlightMode == HighlightMode.SingleTile)
            {
                if (!mousePosition.Equals(_previousMousePosition))
                {
                    ClearTiles();
                    HighlightTiles(mousePosition);
                    _previousMousePosition = mousePosition;
                }
            }
            else
            {
                ClearTiles();
                HighlightTiles(_currentPlayerPosition);
                _previousPlayerPosition = _currentPlayerPosition;
                if (!_currentPlayerPosition.Equals(_previousPlayerPosition))
                {
                    ClearTiles();
                    HighlightTiles(_currentPlayerPosition);
                    _previousPlayerPosition = _currentPlayerPosition;
                }
            }
        }

        void ClearTiles()
        {
            interactiveTilemap.ClearAllTiles();
        }

        void HighlightTiles(Vector3Int mousePosition)
        {
            switch (_currentHighlightMode)
            {
                case HighlightMode.MoveRange:
                    HighlightMoveRange(3);
                    break;

                case HighlightMode.SkillRange:
                    HighlightSkillRange(mousePosition, 2);
                    break;

                case HighlightMode.SingleTile:
                    interactiveTilemap.SetTile(mousePosition, hoverTile);
                    break;
            }
        }

        void HighlightMoveRange(int actionPoints)
        {
            Vector3Int playerPosition = GetPlayerPosition();

            for (int x = -actionPoints; x <= actionPoints; x++)
            {
                for (int y = -actionPoints; y <= actionPoints; y++)
                {
                    int distance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (distance <= actionPoints)
                    {
                        Vector3Int tilePosition = new Vector3Int(playerPosition.x + x, playerPosition.y + y, playerPosition.z);
                        interactiveTilemap.SetTile(tilePosition, moveRangeTile);
                    }
                }
            }
        }

        void HighlightSkillRange(Vector3Int mousePosition, int skillRange)
        {
            for (int x = -skillRange; x <= skillRange; x++)
            {
                for (int y = -skillRange; y <= skillRange; y++)
                {
                    int distance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (distance <= skillRange)
                    {
                        Vector3Int tilePosition = new Vector3Int(mousePosition.x + x, mousePosition.y + y, mousePosition.z);
                        interactiveTilemap.SetTile(tilePosition, skillRangeTile);
                    }
                }
            }
        }
        
        Vector3Int GetPlayerPosition()
        {
            return _grid.WorldToCell(_playerTransform.position);
        }

        Vector3Int GetMousePosition()
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return _grid.WorldToCell(mouseWorldPosition);
        }
        
    }
}