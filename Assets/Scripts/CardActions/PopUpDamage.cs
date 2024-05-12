using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    [SerializeField]private float lifetime = 2f;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
        Debug.Log("POP up");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
