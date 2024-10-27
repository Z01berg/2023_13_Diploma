using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    [SerializeField] private Collider2D collision;
    [SerializeField] private Animator _animator;

    void Update()
    {
        OnCollision();
    }

    private void OnCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(this.transform.position, collision.bounds.size, 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _animator.SetTrigger("Exit");
                EventSystem.NewLevel.Invoke();
            }
        }
        
    }

    private void OnSpawn(Vector3 ordinats)//TODO: Spawn Animation
    {
        this.transform.position = ordinats;
        _animator.SetTrigger("New_Level");
    }
}
