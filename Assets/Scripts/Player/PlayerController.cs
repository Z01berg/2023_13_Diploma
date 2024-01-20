using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Transform _movePoint;
        [SerializeField] private LayerMask _whatStopsMovement;

        public bool DidSmth = false; //dla Timer kiedy jakaś karta rozegrana true inaczej treba 2 razy kliknąć enter
        
        private void Start()
        {
            _movePoint.parent = null;
            GetComponent<PlayerController>().enabled = !GetComponent<PlayerController>().enabled;
            EventSystem.PlayerMove.AddListener(ToogleScrypt);
        }
    
        private void Update()
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                _movePoint.position, 
                _moveSpeed * Time.deltaTime
                );

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
        }

        private void ToogleScrypt()
        {
            GetComponent<PlayerController>().enabled = !GetComponent<PlayerController>().enabled;
        }
    }
}
