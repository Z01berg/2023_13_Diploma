using System.Linq;
using Grid.New;
using UnityEngine;

/**
 * Public class PlayerController jest skryptem kontrolera gracza, który zarządza ruchem gracza w grze.

    Zawiera informacje o:
    - prędkości poruszania się gracza
    -  punkcie ruchu gracza
    - warstwie zatrzymującej ruch gracza
    - punktach akcji gracza
    - zasięgu umiejętności gracza
    - obrażeniach umiejętności gracza
    - włączaniu/wyłączaniu skryptu

    Działa w trybie gry:
    - przemieszcza gracza w kierunku punktu ruchu
    - wykrywa przeszkody, które zatrzymują ruch gracza
    - oblicza ruch gracza na podstawie wejścia gracza
    - wyłącza skrypt gracza po zakończeniu ruchu

    Możliwe akcje:
    - pobranie punktów akcji gracza
    - pobranie zasięgu umiejętności gracza
    - pobranie obrażeń umiejętności gracza
 */

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Transform _movePoint;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _whatStopsMovement;

        [SerializeField] private int _actionPoints;
        [SerializeField] private int _skillRange;
        [SerializeField] private int _skillDamage;

        private bool _turnOff = false;
        private static bool _isPlayerTurn;

        private Vector2 _moveVector = Vector2.zero;
        
        public OverlayTile standingOnTile;


        private void Start()
        {
            _movePoint.parent = null;
            GetComponent<PlayerController>().enabled = !GetComponent<PlayerController>().enabled;// TODO uncomment this after AI tests
            EventSystem.ChangeHealthPlayer.AddListener(UpdateHealth);
            EventSystem.PlayerMove.AddListener(ToogleScrypt);
            EventSystem.MovePlayer.AddListener(SetMoveVector);
            standingOnTile = GetCurrentTile();
        }

        private void UpdateHealth(int arg0)
        {
            transform.GetComponent<HealthBar>().ChangeHealth(arg0);
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _movePoint.position,
                _moveSpeed * Time.deltaTime
            );
            CalculatePlayerMove();
            CalculatePlayerGrid();
            standingOnTile = GetCurrentTile();
        }

        public OverlayTile GetCurrentTile()
        {
            RaycastHit2D? hit = GetFocusedOnTile(transform.position);

            if (hit.HasValue)
            {
                OverlayTile overlayTile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

                return overlayTile;
            }
            return null;
        }

        public RaycastHit2D? GetFocusedOnTile(Vector3 spawnPosition)
        {
            Vector2 spawnPosition2d = new Vector2(spawnPosition.x, spawnPosition.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(spawnPosition2d, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        private void LateUpdate()
        {
            if (_turnOff)
            {
                _turnOff = false;
                this.enabled = false;
            }
        }

        private void CalculatePlayerMove()
        {
            if (Vector3.Distance(transform.position, _movePoint.position) <= .05f)
            {
                if (Mathf.Abs(_moveVector.x) == 1f || Mathf.Abs(_moveVector.y) == 1f)
                {
                    if (!Physics2D.OverlapCircle(_movePoint.position + new Vector3(_moveVector.x, _moveVector.y, 0), .2f,
                            _whatStopsMovement))
                    {
                        _movePoint.position += new Vector3(_moveVector.x, _moveVector.y, 0);
                        RotateSprite(new Vector3(_moveVector.x, _moveVector.y, 0));
                    }
                }
            }
        }

        private void CalculatePlayerGrid()
        {
                // RaycastHit2D
        }

        private void ToogleScrypt(bool isThis)
        {
            _isPlayerTurn = isThis;
            if (this.enabled && !isThis)
            {
                _turnOff = true;
            }
            else
            {
                
                this.enabled = isThis;
            }
        }
        public static bool getPlayerTurn()
        {
            return _isPlayerTurn;
        }

        private void SetMoveVector(Vector2 vector)
        {
            _moveVector = vector;
        }

        public int ActionPoints => _actionPoints;
        public int SkillRange => _skillRange;
        public int SkillDamage => _skillDamage;
        
        private void RotateSprite(Vector3 direction)
        {
            if (direction == new Vector3(-1, 0, 0))
            {
                _spriteRenderer.flipX = true;
            }
            else if (direction == new Vector3(1, 0, 0))
            {
                _spriteRenderer.flipX = false;
            }
        }
        
    }
}