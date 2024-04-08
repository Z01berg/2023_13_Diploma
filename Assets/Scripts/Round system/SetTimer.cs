using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Publiczna klasa SetTimer jest odpowiedzialna za ustawienie czasomierza oraz wyświetlenie tekstu na podstawie typu postaci.
 *
 * Ma w sobie informacje o:
 * - tekście do wyświetlenia
 * - typie postaci (Player, Enemy, Item, Boss, None)
 * - obiekcie HealthBar, z którym jest powiązany
 *
 * Na podstawie typu postaci, SetTimer określa tekst do wyświetlenia oraz przekazuje go do czasomierza.
 */


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

