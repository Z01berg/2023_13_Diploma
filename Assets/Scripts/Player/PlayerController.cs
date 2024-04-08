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
            
            if (_turnOff)
            {
                CalculatePlayerMove();
                _turnOff = false;
                this.enabled = false;
            }
            else
            {
                CalculatePlayerMove();  
            }
        }

        private void CalculatePlayerMove()
        {
            if (Vector3.Distance(transform.position, _movePoint.position) <= .05f)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                if (Mathf.Abs(horizontal) == 1f)
                {
                    if (!Physics2D.OverlapCircle(_movePoint.position + new Vector3(horizontal, 0f, 0f), .2f, _whatStopsMovement))
                    {
                        _movePoint.position += new Vector3(horizontal, 0f, 0f);
                    }
                }
                else if (Mathf.Abs(vertical) == 1f)
                {
                    if (!Physics2D.OverlapCircle(_movePoint.position + new Vector3(0f, vertical, 0f), .2f, _whatStopsMovement))
                    {
                        _movePoint.position += new Vector3(0f, vertical, 0f);
                    }
                }
            }
        }

        private void ToogleScrypt(bool isThis)
        {
            if (this.enabled && !isThis)
            {
                _turnOff = true;
            }
            else
            {
                this.enabled = !this.enabled;
            }

            
        }
        
        public int ActionPoints => _actionPoints;
        public int SkillRange => _skillRange;
        public int SkillDamage => _skillDamage; 
        
    }
}
