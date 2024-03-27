using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Klasa EnemyController odpowiedzialna jest za opisywanie zachowania
 * testowego modelu przeciwnika. Znajduje się w nim logika
 * odpowiedzialna za wykrywanie postaci gracza i inicjowanie
 * walki poprzez generowanie widoku siatki. Jest to wstępny
 * prototyp na bazie którego zostaną wykreowane lepsze
 * rozwiązania.
 */

    public class EnemyController : MonoBehaviour
    {
        
        /*
         * Zmienne _visionRangeInTiles i _tileSize
         * związane są z kalkulacją systemu wykrycia
         * gracza przez przeciwnika. Jeśli potrzebne będą
         * zmiany w układzie używanych tilemap,
         * można będzie dostosować poniższe wartości
         * z poziomu edytora Unity. Pomocna w tym będzie metoda
         * OnDrawGizmos().
         */
        
        //określa zasięg wykrycia gracza przez przeciwnika co X tileów
        [SerializeField] private int _visionRangeInTiles = 5;
        //defaultowo 1f, ale można zmienić w zależności od potrzeb
        [SerializeField] private float _tileSize = 1f;
        [SerializeField] private int _healthPoints;
        [SerializeField] private float _moveSpeed = 5f;

        private Transform _player;
        private Tilemap _gridOverlayTilemap;
        private bool _isCombatStarted;
        private Pathfinding _pathfinding;

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;

            _pathfinding = GetComponent<Pathfinding>();
            if (_pathfinding == null)
            {
                Debug.LogError("Pathfinding cript not found on the Enemy object");
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
            int tileDistanceToPlayer = CalculateTileDistanceToPlayer();

            if (tileDistanceToPlayer <= _visionRangeInTiles)
            {
                EnterCombat();
            }
            else
            {
                ExitCombat();
            }
        }
        
        int CalculateTileDistanceToPlayer()
        {
            Vector3 playerPosition = _player.position;
            Vector3 enemyPosition = transform.position;

            int tileDistanceX = Mathf.RoundToInt(Mathf.Abs(playerPosition.x - enemyPosition.x) / _tileSize);
            int tileDistanceY = Mathf.RoundToInt(Mathf.Abs(playerPosition.y - enemyPosition.y) / _tileSize);

            return Mathf.Max(tileDistanceX, tileDistanceY);
        }

        bool EnterCombat()
        {
                _gridOverlayTilemap.GetComponent<Renderer>().enabled = true;
                _isCombatStarted = true;
                ChasePlayer();
                return _isCombatStarted;
        }

        bool ExitCombat()
        {
                _gridOverlayTilemap.GetComponent<Renderer>().enabled = false;
                return _isCombatStarted = false;
        }
        
        void ChasePlayer()
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = _player.position;

            List<Vector3> path = _pathfinding.FindPath(startPosition, targetPosition);

            if (path != null && path.Count > 0)
            {
                Vector3 nextWaypoint = path[0];
                Vector3 currentPosition = new Vector3(
                    Mathf.Round(transform.position.x),
                    Mathf.Round(transform.position.y),
                    0f);
                Vector3 targetTileCenter = new Vector3(
                    Mathf.Round(nextWaypoint.x) + 0.5f,
                    Mathf.Round(nextWaypoint.y) + 0.5f,
                    0f);

                if (currentPosition == targetTileCenter)
                {
                    path.RemoveAt(0);
                    if (path.Count > 0)
                    {
                        nextWaypoint = path[0];
                        targetTileCenter = new Vector3(
                            Mathf.Round(nextWaypoint.x) + 0.5f,
                            Mathf.Round(nextWaypoint.y) + 0.5f,
                            0f);
                    }
                }
                
                float step = _moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetTileCenter, step);
            }
        }

        public bool IsCombatStarted => _isCombatStarted;

        public int HealthPoints
        {
            get => _healthPoints;
            set => _healthPoints = value;
        }
        
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(_visionRangeInTiles * _tileSize * 2, _visionRangeInTiles * _tileSize * 2, 0));
        }
        
        
        
    }

