using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Transform _movePoint;
        [SerializeField] private LayerMask _whatStopsMovement;
        
        [SerializeField] private int _actionPoints;
        [SerializeField] private int _skillRange;
        [SerializeField] private int _skillDamage;
        
        private bool _turnOff = false;
        
        private void Start()
        {
            _movePoint.parent = null;
            //GetComponent<PlayerController>().enabled = !GetComponent<PlayerController>().enabled;// TODO uncomment this after AI tests
            EventSystem.PlayerMove.AddListener(ToogleScrypt);
        }
    
        private void Update()
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                _movePoint.position, 
                _moveSpeed * Time.deltaTime
                );

            CalculatePlayerMove(_turnOff);
        }

        private void CalculatePlayerMove(bool lastMove) // TODO optimaze this
        {
            if (Vector3.Distance(transform.position, _movePoint.position) <= .05f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(
                            _movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f),
                            .2f,
                            _whatStopsMovement))
                    {
                        _movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    }
                } else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(
                            _movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f),
                            .2f,
                            _whatStopsMovement))
                    {
                        _movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    }
                }
            }
            else if (Vector3.Distance(transform.position, _movePoint.position) <= .05f && lastMove)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(
                            _movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f),
                            .2f,
                            _whatStopsMovement))
                    {
                        _movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    }
                } else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(
                            _movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f),
                            .2f,
                            _whatStopsMovement))
                    {
                        _movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    }
                }
                this.enabled = _turnOff;
            }
        }

        private void ToogleScrypt(bool isThis)
        {
            _turnOff = true;
        }
        
        public int ActionPoints => _actionPoints;
        public int SkillRange => _skillRange;
        public int SkillDamage => _skillDamage; 
        
    }
}
