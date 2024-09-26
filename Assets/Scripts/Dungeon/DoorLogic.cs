using System;
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

    public void RoomCleared()
    {
        _collision.enabled = false;
        _animator.SetBool("open", true);
        _isOpen = false;
    }

    public void CloseDoors()
    {
        _animator.SetBool("open", false);
        _isOpen = true;
        _collision.enabled = true;
    }
}