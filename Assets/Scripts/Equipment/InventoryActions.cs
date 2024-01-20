using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActions : MonoBehaviour
{
    private bool _active;
    [SerializeField] private GameObject _inv;

    private void Start()
    {
        _active = _inv.activeSelf;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(_active)
            {
                _inv.gameObject.SetActive(false);
                _active = false;
            }
            else
            {
                _inv.gameObject.SetActive(true);
                _active=true;
            }
        }
    }
}
