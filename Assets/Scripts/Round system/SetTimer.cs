using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _Text;
    
    //boleans
    [SerializeField] private Boolean bohater = false;
    [SerializeField] private Boolean enemy = false;
    [SerializeField] private Boolean item = false;
    [SerializeField] private Boolean boss = false;
    
    private void Start()
    {
        Timer timer = FindObjectOfType<Timer>();
        
        String Person = Define(bohater, enemy, item, boss);;

        if (timer != null)
        {
            timer.AddTextFromSetTimer(_Text, Person);
        }
        else
        {
            Debug.LogError("WSTAW MANAGERS HALO <( ‵□′)───C＜─___-)||");
        }
    }

    private String Define(Boolean bohater, Boolean enemy, Boolean item, Boolean boss)
    {
        string code = ""; 
        
        if (bohater)
        {
            code = "Player";
        }
        else if (enemy)
        {
            code = "Enemy";
        }
        else if (item)
        {
            code = "Item";
        }
        else if (boss)
        {
            code = "Boss";
        }
        else
        {
            code = "N/A";
        }

        return code;
    }
}
