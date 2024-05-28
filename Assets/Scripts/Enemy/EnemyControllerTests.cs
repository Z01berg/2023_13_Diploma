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
    public class EnemyControllerTests : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Transform _movePoint;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _whatStopsMovement;

        private int _movement = 4;
        private int _attack = 1;

        private bool _turnOff = false;
        private static bool _isEnemyTurn;

        private Vector2 _moveVector = Vector2.zero;

        private void Start()
        {
            _movePoint.parent = null;
            GetComponent<PlayerController>().enabled = !GetComponent<PlayerController>().enabled;// TODO uncomment this after AI tests
            EventSystem.EnemyMove.AddListener(ToogleScrypt);
            EventSystem.MoveEnemy.AddListener(SetMoveVector);
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _movePoint.position,
                _moveSpeed * Time.deltaTime
            );
            CalculateEnemyMove();
        }

        private void LateUpdate()
        {
            if (_turnOff)
            {
                _turnOff = false;
                this.enabled = false;
            }
        }

        private void CalculateEnemyMove()
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

        private void ToogleScrypt(bool isThis)
        {
            _isEnemyTurn = isThis;
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
            return _isEnemyTurn;
        }

        private void SetMoveVector(Vector2 vector)
        {
            _moveVector = vector;
        }
        
        
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