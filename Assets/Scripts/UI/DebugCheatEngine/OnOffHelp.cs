using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffHelp : MonoBehaviour
{
    [SerializeField] private GameObject Text;
    void Start()
    {
        EventSystem.ShowHelpSheet.AddListener(SwitchObject);
    }

    private void SwitchObject()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
