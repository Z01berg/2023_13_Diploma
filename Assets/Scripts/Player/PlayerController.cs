using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultInputs;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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
    public class PlayerController : MonoBehaviour, IDefaultMausenKeysActions
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Transform _movePoint;
        [SerializeField] private LayerMask _whatStopsMovement;

        [SerializeField] private int _actionPoints;
        [SerializeField] private int _skillRange;
        [SerializeField] private int _skillDamage;

        private bool _turnOff = false;
        private static bool _isPlayerTurn;

        private DefaultInputs _controls;
        private Vector2 _moveVector = Vector2.zero;

        private void Start()
        {
            _movePoint.parent = null;
            GetComponent<PlayerController>().enabled = !GetComponent<PlayerController>().enabled;// TODO uncomment this after AI tests
            EventSystem.PlayerMove.AddListener(ToogleScrypt);
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _movePoint.position,
                _moveSpeed * Time.deltaTime
            );
            CalculatePlayerMove();
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
                    }
                    _moveVector = Vector3.zero;
                }
            }
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

        private void OnEnable()
        {
            PrepareInputs();
            _controls.DefaultMausenKeys.Move.performed += OnMove;
            _controls.DefaultMausenKeys.Equipment.performed += OnEquipment;
            _controls.DefaultMausenKeys.Menu.performed += OnMenu;
        }

        private void OnDisable()
        {
            _controls.Disable();
            _controls.DefaultMausenKeys.Move.performed -= OnMove;
            _controls.DefaultMausenKeys.Equipment.performed -= OnEquipment;
            _controls.DefaultMausenKeys.Menu.performed -= OnMenu;
        }

        private void PrepareInputs()
        {
            _controls = new DefaultInputs();
            _controls.DefaultMausenKeys.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveVector = context.ReadValue<Vector2>();
        }

        public void OnMenu(InputAction.CallbackContext context)
        {
            EventSystem.OpenClosePauseMenu?.Invoke();
        }

        public void OnEquipment(InputAction.CallbackContext context)
        {
            EventSystem.OpenCloseInventory?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            Debug.Log("interact");
#endif
        }

        public int ActionPoints => _actionPoints;
        public int SkillRange => _skillRange;
        public int SkillDamage => _skillDamage;
    }
}