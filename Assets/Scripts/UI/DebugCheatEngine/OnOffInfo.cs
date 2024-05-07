using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffInfo : MonoBehaviour
{
    [SerializeField] private GameObject Text;
    void Start()
    {
        EventSystem.ShowCheatEngine.AddListener(SwitchObject);
    }

    private void SwitchObject()
    {
        Debug.Log("Fuck you");
        Text.SetActive(!Text.activeSelf);
    }
}
