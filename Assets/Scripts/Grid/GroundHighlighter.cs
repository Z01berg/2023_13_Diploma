using Enemy;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Grid
{
    public class GroundHighlighter : MonoBehaviour
    {
        private UnityEngine.Grid _grid;
        private PlayerController _playerController;
        private EnemyController _enemyController;
        [SerializeField] private Tilemap interactiveTilemap;
        [SerializeField] private Tile hoverTile;
        [SerializeField] private Tile moveRangeTile;
        [SerializeField] private Tile skillRangeTile;
        
        private HighlightMode _currentHighlightMode = HighlightMode.SingleTile;
        private Transform _playerTransform;
        private Vector3Int _previousPlayerPosition;
        private Vector3Int _currentPlayerPosition;
        private Vector3Int _previousMousePosition;
        private Vector3Int _currentMousePosition;

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
            _playerController = FindObjectOfType<PlayerController>();
            _enemyController = FindObjectOfType<EnemyController>();
        }

        private void Update()
        {
            _currentMousePosition = GetMousePosition();
            _currentPlayerPosition = GetPlayerPosition();

            if (_enemyController.IsCombatStarted)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _currentHighlightMode = HighlightMode.MoveRange;
                } else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _currentHighlightMode = HighlightMode.SkillRange;
                }
            }
            else
            {
                _currentHighlightMode = HighlightMode.SingleTile;
            }
            
            if (_currentHighlightMode == HighlightMode.SkillRange || _currentHighlightMode == HighlightMode.SingleTile)
            {
                ClearTilesWMousePosition();
                if (!_currentMousePosition.Equals(_previousMousePosition))
                {
                    ClearTilesWMousePosition();
                }
            }
            else
            {
                ClearTilesWPlayerPosition();
                if (!_currentPlayerPosition.Equals(_previousPlayerPosition))
                {
                    ClearTilesWPlayerPosition();
                }
            }
        }

        private void ClearTiles()
        {
            interactiveTilemap.ClearAllTiles();
        }

        private void HighlightTiles(Vector3Int mousePosition)
        {
            switch (_currentHighlightMode)
            {
                case HighlightMode.MoveRange:
                    HighlightMoveRange(_playerController.ActionPoints);
                    break;

                case HighlightMode.SkillRange:
                    HighlightSkillRange(mousePosition, _playerController.SkillRange);
                    break;

                case HighlightMode.SingleTile:
                    interactiveTilemap.SetTile(mousePosition, hoverTile);
                    break;
            }
        }

        private void HighlightMoveRange(int actionPoints)
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

        private void ClearTilesWMousePosition()
        {
            ClearTiles();
            HighlightTiles(_currentMousePosition);
            _previousMousePosition = _currentMousePosition;
        }

        private void ClearTilesWPlayerPosition()
        {
            ClearTiles();
            HighlightTiles(_currentPlayerPosition);
            _previousPlayerPosition = _currentPlayerPosition;
        }
        
        private Vector3Int GetPlayerPosition()
        {
            return _grid.WorldToCell(_playerTransform.position);
        }

        private Vector3Int GetMousePosition()
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return _grid.WorldToCell(mouseWorldPosition);
        }
        
    }
}