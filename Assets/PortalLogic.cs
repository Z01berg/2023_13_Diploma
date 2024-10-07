using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    [SerializeField] private Collider2D collision;

    void Update()
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventSystem.NewLevel.Invoke();
        }
    }
}
