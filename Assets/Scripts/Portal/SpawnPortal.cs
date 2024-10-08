using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool turnOn = false;
    private void Start()
    {
        EventSystem.NewLevel.AddListener(OnSpawn);
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
    }

    private void OnSpawn()
    {
        if (!turnOn)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            turnOn = true;
        }
        _animator.SetTrigger("New_Level");
    }
}
