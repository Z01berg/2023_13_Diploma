using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuAnimatorActions : MonoBehaviour
{
    [SerializeField] private GameObject _IngameMenu;

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void SetInventoryVisible()
    {
        _IngameMenu.GetComponent<IngameUIManager>().ChangeInventoryVisible();
    }

    public void SetMenuInvisible()
    {
        _IngameMenu.GetComponent<IngameUIManager>().SetMenuInvisible();
    }

    public void SetPauseVisible()
    {
        _IngameMenu.GetComponent<IngameUIManager>().ChangePauseVisible();
    }

    public void SetMapVisible()
    {
        _IngameMenu.GetComponent<IngameUIManager>().ChangeMapVisible();
    }
}
