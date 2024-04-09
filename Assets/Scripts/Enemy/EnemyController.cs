using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Transform _enemyCenterPoint;

    private Transform _player;
    private Tilemap _gridOverlayTilemap;
    private Pathfinding _pathfinding;
    private List<Vector3> _currentPath;
    private int _currentWaypointIndex;
    private bool _isChasing = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        
        _pathfinding = GetComponent<Pathfinding>();
        if (_pathfinding == null)
        {
            Debug.LogError("Pathfinding script not found on the Enemy object");
        }

        GameObject gridOverlayObject = GameObject.FindWithTag("GridOverlayTag");
        if (gridOverlayObject != null)
        {
            _gridOverlayTilemap = gridOverlayObject.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("GridOverlay object not found with the specified tag!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int clickedTilePosition = GetClickedTilePosition();

            if (_gridOverlayTilemap != null && _gridOverlayTilemap.HasTile(clickedTilePosition))
            {
                Vector3 targetPosition = _gridOverlayTilemap.CellToWorld(clickedTilePosition);
                ChaseTarget(targetPosition);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChaseTarget(_player.position);
        }
        
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!_isChasing)
            {
                ChaseTarget(_player.position);
                _isChasing = true;
            }
            else
            {
                _isChasing = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            _moveSpeed += 1f;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            _moveSpeed -= 1f;
        }
    }

    Vector3Int GetClickedTilePosition()
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _gridOverlayTilemap.WorldToCell(clickPosition);
    }

    void ChaseTarget(Vector3 targetPosition)
    {
        Vector3 startPosition = _enemyCenterPoint.position;

        _currentPath = _pathfinding.FindPath(startPosition, targetPosition);
        _currentWaypointIndex = 0;
    }

    void LateUpdate()
    {
        if (_isChasing)
        {
            ChaseTarget(_player.position);
        }

        if (_currentPath != null && _currentPath.Count > 0)
        {
            Vector3 nextWaypoint = _currentPath[_currentWaypointIndex];
            Vector3 currentPosition = _enemyCenterPoint.position;

            if (Vector3.Distance(currentPosition, nextWaypoint) < 0.001f)
            {
                if (_currentWaypointIndex < _currentPath.Count - 1)
                {
                    _currentWaypointIndex++;
                    nextWaypoint = _currentPath[_currentWaypointIndex];
                }
            }

            float step = _moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(currentPosition, nextWaypoint, step);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0));
    }
}

