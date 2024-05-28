using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    [SerializeField] private Collider2D _trigger;
    [SerializeField] private Collider2D _collision;
    [SerializeField] private Animator _animator;

    private bool _isOpen = false;

    void Start()
    {
        _animator.SetBool("open", true);
        _collision.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == _trigger)
        {
            _collision.enabled = true;

            _animator.SetBool("open", false);
            _isOpen = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == _trigger)
        {
            _collision.enabled = false;

            if (_isOpen)
            {
                _animator.SetBool("open", true);
                _isOpen = false;
            }
        }
    }
}