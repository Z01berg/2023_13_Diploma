using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CreditsAutoScroll : MonoBehaviour
{
    private Scrollbar _scrollbar;
    private Coroutine _coroutine;

    private void Start()
    {
        _scrollbar = GetComponent<Scrollbar>();
        _scrollbar.value = 1;
    }

    public void ScrollCredits()
    {
        _scrollbar.value = 1;
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        while (true)
        {
            _scrollbar.value = _scrollbar.value - 0.00001f;
            yield return new WaitForSeconds(.01f);
            
        }
        
    }

    private void OnMouseDown()
    {
        Time.timeScale = 0;
    }

    private void OnMouseUp()
    {
        Time.timeScale = 1;
    }
}
