using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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

        if (timer != null)
        {
            string tag = Define(_characterType);
            int value = (_characterType == CharacterType.Enemy) ? Random.Range(5, 10) : 0;
            _text.text = value.ToString();

            timer.AddTextFromSetTimer(_text, tag, _hp);
        }
        else
        {
            Debug.LogError("Timer manager is missing!");
        }
    }


    private string Define(CharacterType type)
    {
        return type.ToString();
    }
}

