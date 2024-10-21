using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class EnemyControllerTests : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = .1f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _whatStopsMovement;
        private Vector3 _playerPosition;

        private int _movement = 4;
        private int _attack = 1;

        private bool _turnOff = false;
        private static bool _isEnemyTurn = false;

        private Vector2 _moveVector = Vector2.zero;
        //private Random _random;

        private bool _endedMove = true;

        private void Start()
        {
            var srPos = _spriteRenderer.transform.position;
            //_random = new Random(); // Inicjalizacja generatora liczb losowych
            //GetComponent<EnemyControllerTests>().enabled = !GetComponent<EnemyControllerTests>().enabled;
            //EventSystem.EnemyMove.AddListener(ToggleScript);
        }

        private void Update()
        {
            if (_isEnemyTurn)
            {
                if (_movement > 0 && _endedMove)
                {
                    Move();
                }
                else
                {
                    if (CheckDistance(this.transform,_playerPosition))
                    {
                        EventSystem.FinishEnemyTurn.Invoke(Random.Range(7, 15));
                        Attack();
                    }
                    else
                    {
                        EventSystem.FinishEnemyTurn.Invoke(Random.Range(5, 8));
                    }
                    _turnOff = false;
                    _isEnemyTurn = false;
                    _movement = 4;
                }
            }
        }
        public bool CheckDistance(Transform transform1, Vector3 transform2)
        {
            // Pobierz pozycje obiektów transformacji
            Vector2 position1 = transform1.position;
            Vector2 position2 = transform2;
            // Oblicz odległość między nimi
            float distance = Vector2.Distance(position1, position2);
            
            // Sprawdź czy odległość jest mniejsza lub równa 2f
            if (distance <= 2f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Attack()
        {
            EventSystem.ChangeHealthPlayer.Invoke(-2);
        }

        private void LateUpdate()
        {
            if (_turnOff)
            {
                _turnOff = false;
                this.enabled = false;
            }
        }

        private void Move()
        {
            _endedMove = false;
            var value = new Vector3(Random.Range(-2,2), Random.Range(-2, 2));
            if (Mathf.Abs(value.x) == 1f || Mathf.Abs(value.y) == 1f)//TODO: może Debug.Break
            {

                Vector3 targetPosition = transform.position + new Vector3(value.x * 0.5f, value.y * 0.5f, -6);

                Collider2D[] colliders = Physics2D.OverlapBoxAll(targetPosition, new Vector2(0.5f, 0.5f), -6);

                bool canMove = true;

                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("collisionTilemap"))
                    {
                        canMove = false;
                        break;
                    }

                }
                if (canMove)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        targetPosition,
                        _moveSpeed * Time.deltaTime/4
            );
                    transform.position += new Vector3(value.x * 1f, value.y * 1f, 0);
                    
                }
                _movement--;
            }
            _endedMove = true;
        }

        /*
        private void ToggleScript(bool isThis, Vector3 plpos)
        {
            _playerPosition = plpos;
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
        */

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
