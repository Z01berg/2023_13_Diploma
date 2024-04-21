using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Publiczna clasa TimerData ma za zadanie {get; set} 3 rzeczy:
 * - Value (znaczenie zegarka)
 * - Tag (Tag z enuma "boss"; "player")
 * - HP (znaczenie HP)
 */

/**
 * Publiczna klasa Timer ma za zadanie zarządzać funkcjonalnością zegarów oraz nimi samymi.
 *
 * Ma możliwość:
 * - Dodawania tekstu, tagu i odnośnika do obiektu HP dla nowego zegara.
 * - Obsługiwanie wejścia z klawiatury w celu manipulacji zegarami.
 * - Aktualizacji tekstów zegarów.
 * - Uruchamiania odliczania zegarów.
 * - Obliczania priorytetu dla aktywnego zegara.
 * - Ma ograniczenia dla Timer Value (0 - 99)
 */

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _turn;
    [SerializeField] private List<TMP_Text> _texts = new List<TMP_Text>();
    [SerializeField] private List<String> _id = new List<String>();
    [SerializeField] private List<GameObject> _hpAdres = new List<GameObject>();
    private List<TimerData> _timers = new List<TimerData>();
    
    private int _activeTimerIndex = 0; //czyja runda
    private bool _counting = false; // po wciśnięciu enter
    
    private float _timeToPause = 0.5f; //do animacji timerów
    
    private bool _cheat = false; // włączenie na "R CTRL" zmieniania znaczenia timerów

    private Animator _animator;
    
    public void AddTextFromSetTimer(TMP_Text newText, String text, GameObject HP)
    {
        _texts.Add(newText);
        _id.Add(text);
        _hpAdres.Add(HP);
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        
        for (int i = 0; i < _texts.Count; i++)
        {
            int value = int.Parse(_texts[i].text);
            _timers.Add(new TimerData { Value = value, Tag = _id[i], HP = _hpAdres[i]});
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            _cheat = !_cheat;
        }
        
        HandleTimerInput();
        
        UpdateTexts();
        
        if (_counting)
        {
            StartCountdown();
        }

        KeepTimerInsideBreakets();
    }
    
    void HandleTimerInput()
    {
        if (Input.GetKeyDown(KeyCode.Comma) && _cheat)
        {
            ChangeActiveTimer(1);
        }
        else if (Input.GetKeyDown(KeyCode.Period) && _cheat)
        {
            ChangeActiveTimer(-1);
        }

        if (Input.GetKeyDown(KeyCode.Quote) && _cheat)
        {
            ChangeActiveTimerValue(1);
        }
        else if (Input.GetKeyDown(KeyCode.Slash) && _cheat)
        {
            ChangeActiveTimerValue(-1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _counting = true;
        }
    }

    void StartCountdown()
    {
        bool anyTimerReachedZero = false;

        for (int i = 0; i < _timers.Count; i++)
        {
            if (_timers[i].Value == 0)
            {
                anyTimerReachedZero = true;
                _counting = false;
                CalculatePriority();
                
                EventSystem.WhatHP.Invoke(_timers[_activeTimerIndex].HP);
                
                if (_timers[_activeTimerIndex].Tag == "Player")
                {
                    _animator.SetBool("K_turn", true);
                    EventSystem.PlayerMove.Invoke(true);
                }
                else
                {
                    EventSystem.PlayerMove.Invoke(false);
                }

                if (_timers[_activeTimerIndex].Tag == "Enemy" )
                {
                    _animator.SetBool("K_turn", true);
                    EventSystem.EnemyMove.Invoke(true);
                }
                else
                {
                    EventSystem.EnemyMove.Invoke(false);
                }
                
                _turn.text = "Turn: " + _timers[_activeTimerIndex].Tag;
                _timeToPause = 0.5f;
            }
        }

        if (!anyTimerReachedZero)
        {
            _counting = false;
            for (int i = 0; i < _timers.Count; i++)
            {
                if (_timers[i].Value > 0)
                {
                    _timers[i].Value--;
                }
            }
        
            UpdateTexts();
            StartCoroutine(AnimationPauseCoroutine());
        }
    }

    IEnumerator AnimationPauseCoroutine()
    {
        if (_timeToPause > 0.01)
        {
            _timeToPause -= 0.05f;
        }
        else
        {
            _timeToPause = 0.01f;
        }

        yield return new WaitForSeconds(_timeToPause);

        StartCountdown();
    }

    void ChangeActiveTimerValue(int change)
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            if (i == _activeTimerIndex)
            {
                _timers[i].Value += change;
            }
        }
        UpdateTexts();
    }

    void KeepTimerInsideBreakets()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            if (i == _activeTimerIndex && !_counting)
            {
                if (_timers[i].Value > 99)
                {
                    _timers[i].Value = 99;
                }
                else if (_timers[i].Value < 0)
                {
                    _timers[i].Value = 0;
                }
            }
        }
    }

    void ChangeActiveTimer(int change)
    {
        _activeTimerIndex += change;
        if (_activeTimerIndex < 0)
        {
            _activeTimerIndex = _timers.Count - 1;
        }
        else if (_activeTimerIndex >= _timers.Count)
        {
            _activeTimerIndex = 0;
        }
    }

    void UpdateTexts()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            if (i == _activeTimerIndex && !_counting)
            {
                _texts[i].color = Color.red;
            }
            else
            {
                _texts[i].color = Color.white;
            }

            _texts[i].text = _timers[i].Value.ToString();
        }
    }

    void CalculatePriority()
    {
        Dictionary<string, int> tagPriority = new Dictionary<string, int>
        {
            { "Item", 3 },    
            { "Player", 2 },
            { "Boss", 1 },
            { "Enemy", 0 },
            { "N/A", -1 }
        };

        int highestPriority = -1;
        int highestPriorityIndex = -1;

        for (int i = 0; i < _timers.Count; i++)
        {
            string currentTag = _timers[i].Tag;
            int currentValue = _timers[i].Value;

            if (tagPriority.ContainsKey(currentTag))
            {
                int currentPriority = tagPriority[currentTag];
        
                if (currentPriority > highestPriority && currentValue == 0)
                {
                    highestPriority = currentPriority;
                    highestPriorityIndex = i;
                }
            }
        }

        if (highestPriorityIndex != -1)
        {
            _activeTimerIndex = highestPriorityIndex;
            UpdateTexts();
        }
        else
        { 
            _activeTimerIndex = _timers.FindIndex(timer => timer.Tag == "N/A");
            UpdateTexts();
        }
    }
}

public class TimerData
{
    public int Value { get; set; }
    public string Tag { get; set; }
    public GameObject HP { get; set; }
}
