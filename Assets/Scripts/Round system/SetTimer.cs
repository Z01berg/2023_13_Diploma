using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * 
 */

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
    [SerializeField] private TMP_Text _text;

    [SerializeField] private CharacterType _characterType = CharacterType.None;

    [SerializeField] private GameObject _hp;

    private void Awake()
    {
        Timer timer = FindObjectOfType<Timer>();

        string person = Define(_characterType);

        if (timer != null)
        {
            timer.AddTextFromSetTimer(_text, person, _hp);
        }
        else
        {
            Debug.LogError("WSTAW \"MANAGERS\" HALO <( ‵□′)───C＜─___-)||");
        }
    }

    private string Define(CharacterType type)
    {
        return type.ToString();
    }
}

