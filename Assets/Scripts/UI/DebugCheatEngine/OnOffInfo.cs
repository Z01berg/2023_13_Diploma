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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchObject()
    {
        Debug.Log("Fuck you");
        //this.Text. = !this.Text.active;
    }
}
