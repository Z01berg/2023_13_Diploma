using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    [SerializeField] private Collider2D collision;

    void Update()
    {
        OnCollision();
    }

    private void OnCollision()
    {
        if (collision.GameObject().tag == "Player")
        {
            EventSystem.NewLevel.Invoke();
        }
    }
}
