using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuAnimatorActions : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryActions;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetInventoryVisible()
    {
        _inventoryActions.GetComponent<InventoryActions>().SetInventoryVisible();
    }
}
