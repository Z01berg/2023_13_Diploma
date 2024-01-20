using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CharacterType
{
    Player,
    Enemy,
    Item,
    Boss,
    None
}

public class SetTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _Text;

    [SerializeField] private CharacterType characterType = CharacterType.None;

    [SerializeField] private GameObject HP;

    private void Awake()
    {
        Timer timer = FindObjectOfType<Timer>();

        string person = Define(characterType);

        if (timer != null)
        {
            timer.AddTextFromSetTimer(_Text, person, HP);
        }
        else
        {
            Debug.LogError("WSTAW MANAGERS HALO <( ‵□′)───C＜─___-)||");
        }
    }

    private string Define(CharacterType type)
    {
        return type.ToString();
    }
}

