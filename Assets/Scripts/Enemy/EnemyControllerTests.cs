using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

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
        private static bool _isEnemyTurn = false;

        private Vector2 _moveVector = Vector2.zero;

        private void Start()
        {
            var srPos = _spriteRenderer.transform.position;
            _movePoint.position = srPos;
            GetComponent<EnemyControllerTests>().enabled = !GetComponent<EnemyControllerTests>().enabled;// TODO uncomment this after AI tests
            EventSystem.EnemyMove.AddListener(ToogleScrypt);
            // EventSystem.MoveEnemy.AddListener(CalculateEnemyMove);
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _movePoint.position,
                _moveSpeed * Time.deltaTime
            );
            


        }

        public void actions()
        {
            if (_movement > 0)
            {
                // SetMoveVector();   
                // CalculateEnemyMove();
                _movement--;
            }
            else
            {
                // _movement = 4;
                EventSystem.FinishEnemyTurn.Invoke();
            }
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
            if (Vector3.Distance(transform.position, _movePoint.position) <= .05f && _isEnemyTurn)
            {
                if (Mathf.Abs(_moveVector.x) == 1f || Mathf.Abs(_moveVector.y) == 1f)
                {
                    if (!Physics2D.OverlapCircle(_movePoint.position + new Vector3(_moveVector.x, _moveVector.y, 0), .2f,
                            _whatStopsMovement))
                    {
                        Random random = new Random();
                        var val = random.NextInt(1, 5);
                        switch (val)
                        {
                            case 1: _moveVector = new Vector3(transform.position.x + 1, transform.position.y, -6);
                                break;
                            case 2: _moveVector = new Vector3(transform.position.x - 1, transform.position.y, -6);
                                break;
                            case 3: _moveVector = new Vector3(transform.position.x , transform.position.y + 1, -6);
                                break;
                            case 4: _moveVector = new Vector3(transform.position.x, transform.position.y - 1, -6);
                                break;
                        }
                        _movement--;
                        _movePoint.position += new Vector3(_moveVector.x, _moveVector.y, 0);
                        RotateSprite(new Vector3(_moveVector.x, _moveVector.y, 0));
                    }
                }
            }
        }

        private void SetMoveVector()
        {
            Random random = new Random();
            Vector2 moveVector = new Vector2();
            var val = 1;
            switch (val)
            {
                case 1: moveVector = new Vector3(transform.position.x + 1, transform.position.y, -6);
                    break;
                case 2: moveVector = new Vector3(transform.position.x - 1, transform.position.y, -6);
                    break;
                case 3: moveVector = new Vector3(transform.position.x , transform.position.y + 1, -6);
                    break;
                case 4: moveVector = new Vector3(transform.position.x, transform.position.y - 1, -6);
                    break;
            }
            // _moveVector = moveVector;
            // _movePoint.position = moveVector;
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