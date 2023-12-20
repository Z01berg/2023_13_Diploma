using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _Text;
    
    private void Start()
    {
        Timer timer = FindObjectOfType<Timer>();

        if (timer != null)
        {
            timer.AddTextFromSetTimer(_Text);
        }
        else
        {
            Debug.LogError("WSTAW MANAGERS HALO <( ‵□′)───C＜─___-)||");
        }
    }
}
