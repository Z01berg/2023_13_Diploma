using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using Object = System.Object;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text turn;
    [SerializeField] private List<TMP_Text> texts = new List<TMP_Text>();
    [SerializeField] private List<String> id = new List<String>();
    [SerializeField] private List<GameObject> HP_adres = new List<GameObject>();
    private List<TimerData> timers = new List<TimerData>();
    
    private int activeTimerIndex = 0; //czyja runda
    private bool counting = false; // po wciśnięciu enter
    
    private float timeToPause = 1f; //do animacji timerów
    

    private bool Cheat = false; // włączenie na "R CTRL" zmieniania znaczenia timerów
    
    public void AddTextFromSetTimer(TMP_Text newText, String text, GameObject HP)
    {
        texts.Add(newText);
        id.Add(text);
    }

    void Start()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            int value = int.Parse(texts[i].text);
            timers.Add(new TimerData { Value = value, Tag = id[i], HP = HP_adres[i]});
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            Cheat = !Cheat;
        }
        
        HandleTimerInput();
        
        UpdateTexts();
        
        if (counting)
        {
            StartCountdown();
        }
    }
    
    void HandleTimerInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && Cheat)
        {
            ChangeActiveTimer(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && Cheat)
        {
            ChangeActiveTimer(-1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && Cheat)
        {
            ChangeActiveTimerValue(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && Cheat)
        {
            ChangeActiveTimerValue(-1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            counting = true;
        }
    }

    void StartCountdown()
    {
        bool anyTimerReachedZero = false;

        for (int i = 0; i < timers.Count; i++)
        {
            if (timers[i].Value == 0)
            {
                anyTimerReachedZero = true;
                counting = false;
                CalculatePriority();
                
                EventSystem.WhatHP.Invoke(timers[activeTimerIndex].HP);
                
                if (timers[activeTimerIndex].Tag == "Player")
                {
                    EventSystem.PlayerMove.Invoke();
                }
              
                turn.text = "Turn: " + timers[activeTimerIndex].Tag;
                timeToPause = 1f;
            }
        }

        if (!anyTimerReachedZero)
        {
            counting = false;
            for (int i = 0; i < timers.Count; i++)
            {
                if (timers[i].Value > 0)
                {
                    timers[i].Value--;
                }
            }
        
            UpdateTexts();
            StartCoroutine(AnimationPauseCoroutine());
        }
    }

    IEnumerator AnimationPauseCoroutine()
    {
        if (timeToPause > 0.1)
        {
            timeToPause -= 0.5f;
        }
        else
        {
            timeToPause = 0.1f;
        }

        yield return new WaitForSeconds(timeToPause);

        StartCountdown();
    }

    void ChangeActiveTimerValue(int change)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            if (i == activeTimerIndex)
            {
                timers[i].Value += change;
            }
        }
        UpdateTexts();
    }
    
    void ChangeActiveTimer(int change)
    {
        activeTimerIndex += change;
        if (activeTimerIndex < 0)
        {
            activeTimerIndex = timers.Count - 1;
        }
        else if (activeTimerIndex >= timers.Count)
        {
            activeTimerIndex = 0;
        }
    }

    void UpdateTexts()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            if (i == activeTimerIndex && !counting)
            {
                texts[i].color = Color.red;
            }
            else
            {
                texts[i].color = Color.white;
            }

            texts[i].text = timers[i].Value.ToString();
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

        for (int i = 0; i < timers.Count; i++)
        {
            string currentTag = timers[i].Tag;
            int currentValue = timers[i].Value;

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
            activeTimerIndex = highestPriorityIndex;
            UpdateTexts();
        }
        else
        { 
            activeTimerIndex = timers.FindIndex(timer => timer.Tag == "N/A");
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
