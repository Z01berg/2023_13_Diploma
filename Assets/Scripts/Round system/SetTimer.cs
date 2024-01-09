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
    
    private void Awake()
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

    private string Define(bool bohater, bool enemy, bool item, bool boss)
    {
        return bohater ? "Player" :
            enemy ? "Enemy" :
            item ? "Item" :
            boss ? "Boss" :
            "N/A";
    }
}
