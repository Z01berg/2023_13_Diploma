using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using CardActions;
using Dungeon;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemyController : MonoBehaviour
{
    
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    private Transform _player;
    private Tilemap _gridOverlayTilemap;
    private Pathfinding _pathfinding;
    private List<Vector3> _currentPath;
    private int _currentWaypointIndex;
    private bool _isChasing;
    private InstantiatedRoom room;
    
    private bool _turnOff = false; 
    private bool _isEnemyTurn = false;
    private bool _endedMove = true;
    private Vector3 _playerPosition;
    private int _enemyId;

    void Start()
    {
        _enemyId = GetInstanceID();
        
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        
        _pathfinding = GetComponent<Pathfinding>();
        if (_pathfinding == null)
        {
            Debug.LogError("Pathfinding script not found on the Enemy object");
        }
        
        GameObject gridOverlayObject = GameObject.FindWithTag(/*"GridOverlayTag"*/ "groundTilemap");
        if (gridOverlayObject != null)
        {
            _gridOverlayTilemap = gridOverlayObject.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("GridOverlay object not found with the specified tag!");
        }
        
        Transform gridTransform = transform.parent;
        Transform walkableTilemapTransform = gridTransform.Find("Ground");
        Transform collisionTilemapTransform = gridTransform.Find("CollisionTOP");

        _pathfinding.groundTilemap = walkableTilemapTransform.GetComponent<Tilemap>();
        _pathfinding.topTilemap = collisionTilemapTransform.GetComponent<Tilemap>();

        if (walkableTilemapTransform != null)
        {
            Debug.Log("Walkable tilemap accessed successfully.");
        }
        else
        {
            Debug.LogError("Failed to access walkable tilemap.");

        }
        
        if (collisionTilemapTransform != null)
        {
            Debug.Log("Collision tilemap accessed successfully.");
        }
        else
        {
            Debug.LogError("Failed to access collision tilemap.");

        }

        room = GetComponentInParent<InstantiatedRoom>();
        
        EventSystem.EnemyMove.AddListener(ToggleScript);
    }

    void Update()
    {
        if (_isEnemyTurn && _endedMove)
        {
            MoveTowardsThePlayer();
        }
        else
        {
            _turnOff = false;
            _isEnemyTurn = false;
        }
        
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int clickedTilePosition = GetClickedTilePosition();

            if (_gridOverlayTilemap != null && _gridOverlayTilemap.HasTile(clickedTilePosition))
            {
                Vector3 targetPosition = _gridOverlayTilemap.CellToWorld(clickedTilePosition);
                ChaseTarget(targetPosition);
            }
        }
        */
        
        /*
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChaseTarget(_player.position);
        }
        */
        
        /*
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
        */
        
        /*
        if (Input.GetKeyDown(KeyCode.I))
        {
            _moveSpeed += 1f;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            _moveSpeed -= 1f;
        }
        */
        
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
        EventSystem.ChangeHealthPlayer.Invoke(-1);
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            ApplyCardEffect playerCardEffect = player.GetComponent<ApplyCardEffect>();
            if (playerCardEffect != null)
            {
                playerCardEffect.ShowHitAnimation();
            }
        }
        
        EndEnemyTurn();
    }

    private void MoveTowardsThePlayer() 
    {
        _endedMove = false;
        
        _playerPosition = _player.position;

        _currentPath = _pathfinding.FindPath(_playerPosition);
        if (_currentPath != null)
        {
            StartCoroutine(MoveAlongPath());
        }
        else
        {
            EndEnemyTurn();
        }
    }

    private IEnumerator MoveAlongPath() 
    {
        for(int i = 0; i <= 3; i++)
        {
            if (i >= _currentPath.Count - 1)
            {
                Attack();
                break;
            }
            
            Vector3 nextWaypoint = _currentPath[i];
            nextWaypoint.z = transform.position.z;
            
            _animator.SetTrigger("Move");
            
            while (Vector3.Distance(transform.position, nextWaypoint) >= 0.001f)
            {
                float step = _moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, step);
                yield return null;
            }
        }
        EndEnemyTurn();
    }
    
    private void EndEnemyTurn()
    { 
        _endedMove = true;
        _isEnemyTurn = false;
        EventSystem.FinishEnemyTurn.Invoke(Random.Range(5, 8));
    }

     /*
     Vector3Int GetClickedTilePosition()
     {
         Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         return _gridOverlayTilemap.WorldToCell(clickPosition);
     }
     */
     
     // void ChaseTarget(Vector3 targetPosition)
     // {
     //     Vector3 startPosition = transform.position;
     //
     //     _currentPath = _pathfinding.FindPath(targetPosition);
     //     _currentWaypointIndex = 0;
     // }
    
    
    private void ToggleScript(int enemyId, Vector3 playerPosition)
    {
        
        if (enemyId == _enemyId)
        {
            _isEnemyTurn = true;
            this.enabled = true;
        }
        else
        {
            _isEnemyTurn = false;
            this.enabled = false;
        }
    }

    
    void LateUpdate()
    {
        /*
        if (_isChasing)
        {
            ChaseTarget(_player.position);
        }

        if (_currentPath != null && _currentPath.Count > 0)
        {
            Vector3 nextWaypoint = _currentPath[_currentWaypointIndex];
            Vector3 currentPosition = transform.position;

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
        */
        
        /*
        if (_turnOff)
        {
            _turnOff = false;
            this.enabled = false;
        }
        */
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0));
    }

    private void OnDestroy()
    {
        if (room.enemyInRoomList.Count == 0)
        {
            CombatMode.SetFalse();
        }
    }

    public int GetEnemyId()
    {
        return _enemyId;
    }

    private bool CheckIfEnemyExists()
    {
        return gameObject != null;
    }
    
}