using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Public class SetTimer is responsible for setting up timers and displaying text based on character type.
 *
 * It contains information about:
 * - The text to display
 * - The character type (Player, Enemy, Item, Boss, None)
 * - The HealthBar object it's linked to
 *
 * Based on the character type, SetTimer determines the text to display and passes it to the Timer.
 */
public class SetTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private CharacterType _characterType = CharacterType.None;
    [SerializeField] private GameObject _hp;

    private void Awake()
    {
        Timer timer = FindObjectOfType<Timer>();

        string tag = Define(_characterType);

        if (timer != null)
        {
            if (_characterType == CharacterType.Enemy)
            {
                int ran = Random.Range(5, 10);
                _text.text = ran.ToString();
                timer.AddTextFromSetTimer(_text, tag, _hp);
            }
            else
            {
                if (_characterType == CharacterType.Player)
                {
                    _text.text = "0";
                }
                else
                {
                    _text.text = "0";
                }

                timer.AddTextFromSetTimer(_text, tag, _hp);
            }
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