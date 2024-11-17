using System;
using System.Collections;
using System.Collections.Generic;
using Dungeon;
using TMPro;
using UI;
using UnityEngine;

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
    [SerializeField] private GameObject _deck;
    [SerializeField] private GameObject _player;

    private List<TimerData> _timers = new List<TimerData>();

    private int _activeTimerIndex = 0; //czyja runda

    private bool _counting = false; // po wciśnięciu enter

    private float _timeToPause = 0.4f; //do animacji timerów

    private bool _cheat = false; // włączenie na "R CTRL" zmieniania znaczenia timerów
    
    private bool _createdDeck = false;

    private Animator _animator;
    private DeckController _deckController;
    private EnemyController _enemyController;
    private bool _canDraw = false; // TODO: Zmienić to dobieranie

    public void AddTextFromSetTimer(TMP_Text newText, String text, GameObject HP)
    {
        _timers.Add(new TimerData(0, text, HP, -1, newText, text));
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyController = FindObjectOfType<EnemyController>();
        EventSystem.InstatiatedRoom.AddListener(AddToTimer);
        EventSystem.DeleteReference.AddListener(DeleteTimer);
        EventSystem.FinishEnemyTurn.AddListener(FinishTurn);
        EventSystem.InitInv.AddListener(ChangeBool);
        EventSystem.ZeroTimer.AddListener(ResetCurrentTimer);
        EventSystem.StartCountdown.AddListener(EnteredRoom);
        _deckController = _deck.GetComponent<DeckController>();
    }

    void EnteredRoom()
    {
        _counting = true;
    }

    private void ResetCurrentTimer()
    {
        ChangeActiveTimerValue(0);
    }

    private void ChangeBool()
    {
        _createdDeck = true;
    }
    
    private void FinishTurn(int arg0)
    {
        SetTimerValue(_activeTimerIndex, arg0);
        EnteredRoom();
    }

    void AddToTimer()
    {
        _timers.Clear();

        for (int i = 0; i < _timers.Count; i++)
        {
            int value = int.Parse(_timers[i].Text.text);
            string tag = _timers[i].Id;
            GameObject hp = _timers[i].HP;
            int enemyId = -1;

            if (tag == "Enemy" && hp != null)
            {
                Transform enemyTransform = hp.transform.parent?.parent;
                if (enemyTransform != null)
                {
                    _enemyController = enemyTransform.GetComponent<EnemyController>();
                    enemyId = _enemyController?.GetEnemyId() ?? -1;
                }
            }

            var timerData = new TimerData(value, tag, hp, enemyId, _timers[i].Text, _timers[i].Id);
            _timers.Add(timerData);
        }

        EventSystem.AssignTimerIndex?.Invoke(0); 
    }

    void Update()
    {
        HandleTimerInput();

        UpdateTexts();

        if (_counting)
        {
            StartCountdown();
        }
    }

    void HandleTimerInput()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            _cheat = !_cheat;
            EventSystem.ShowCheatEngine.Invoke();
        }

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

        if (Input.GetKeyDown(KeyCode.Return) || _createdDeck)
        {
            if (_deckController.IsDeckCreated())
            {
                EnteredRoom();
                _createdDeck = !_createdDeck;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayedAttackCard();
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

                if (_timers[_activeTimerIndex].Tag == "Player")
                {
                    EventSystem.PlayerMove.Invoke(true);
                    if (_canDraw && CombatMode.isPlayerInCombat)
                    {
                        EventSystem.DrawACard.Invoke();
                    }

                    _canDraw = true;
                }
                else
                {
                    EventSystem.PlayerMove.Invoke(false);
                }

                if (_timers[_activeTimerIndex].Tag == "Enemy" && _timers[_activeTimerIndex] != null)
                {
                    EventSystem.EnemyMove.Invoke(_timers[_activeTimerIndex].EnemyId, new Vector3(_player.transform.position.x, _player.transform.position.y, -6) );
                }
                else
                {
                    EventSystem.EnemyMove.Invoke(-1, new Vector3(_player.transform.position.x, _player.transform.position.y, -6) );
                }

                _turn.text = "Turn: " + _timers[_activeTimerIndex].Tag;
                _timeToPause = 0.4f;
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

    public void SetTimerValue(int index, int newValue)
    {
        if (index >= 0 && index < _timers.Count)
        {
            _timers[index].UpdateValue(newValue);
        }
        UpdateTexts();
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

        UpdateTexts();
    }

    void UpdateTexts()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            _timers[i].Text.text = _timers[i].Value.ToString();
        }
    }

    void CalculatePriority()
    {
        if (_timers.Count > 0)
        {
            _activeTimerIndex = (_activeTimerIndex + 1) % _timers.Count;
        }
    }
    
    void DeleteTimer(int timerToDelete)
    {
        if (timerToDelete >= 0 && timerToDelete < _timers.Count)
        {
            _timers.RemoveAt(timerToDelete);

            if (timerToDelete == _activeTimerIndex)
            {
                if (_timers.Count > 0)
                {
                    _activeTimerIndex = Mathf.Clamp(_activeTimerIndex, 0, _timers.Count - 1);
                }
            }

            UpdateTexts();
            AddToTimer();
        }
        else
        {
            Debug.LogError("Timer index out of range: " + timerToDelete);
        }
    }
    
    void PlayedAttackCard()
    {
        EventSystem.WhatHP.Invoke(_timers[_activeTimerIndex].HP, _activeTimerIndex);
    }

    public void ChangeActiveTimerValue(int change)
    {
        if (_timers.Count > _activeTimerIndex)
        {
            _timers[_activeTimerIndex].UpdateValue(change);
        }
        UpdateTexts();
    }

    public Dictionary<string, int> tagPriority = new Dictionary<string, int>
    {
        { "Item", 3 },
        { "Player", 2 },
        { "Boss", 1 },
        { "Enemy", 0 },
        { "N/A", -1 }
    };
}
