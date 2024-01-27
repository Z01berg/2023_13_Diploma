using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private int _visionRangeInTiles = 5;
        [SerializeField] private Tilemap _gridOverlayTilemap;
        [SerializeField] private float _tileSize = 1f;

        private Transform _player;
        private bool _isCombatStarted;

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _gridOverlayTilemap = transform.Find("GridOverlay").GetComponent<Tilemap>();
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
                Debug.Log("Player detected - initiating combat");
                return _isCombatStarted = true;
        }

        bool ExitCombat()
        {
                _gridOverlayTilemap.GetComponent<Renderer>().enabled = false;
                Debug.Log("Player outside combat zone");
                return _isCombatStarted = false;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(_visionRangeInTiles * _tileSize * 2, _visionRangeInTiles * _tileSize * 2, 0));
        }
        
    }
}
