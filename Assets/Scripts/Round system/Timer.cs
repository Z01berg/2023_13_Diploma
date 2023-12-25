using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text turn;
    [SerializeField] private List<TMP_Text> texts = new List<TMP_Text>();
    [SerializeField] private List<String> id = new List<String>();
    private List<TimerData> timers = new List<TimerData>();
    private int activeTimerIndex = 0;
    private bool counting = false;
    
    private float timeToPause = 1f;
    
    public void AddTextFromSetTimer(TMP_Text newText, String text)
    {
        texts.Add(newText);
        id.Add(text);
    }

    void Start()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            int value = int.Parse(texts[i].text);
            timers.Add(new TimerData { Value = value, Tag = id[i]});
        }
    }
    
    void Update()
    {
        HandleTimerInput();
        
        UpdateTexts();
        
        if (counting)
        {
            StartCountdown();
        }
    }
    
    void HandleTimerInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeActiveTimer(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeActiveTimer(-1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeActiveTimerValue(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
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
            if (timers[i].Value > 0)
            {
                timers[i].Value--;
            }

            if (timers[i].Value == 0)
            {
                activeTimerIndex = i;
                turn.text = "Turn: " + timers[i].Tag;
                anyTimerReachedZero = true;
                timeToPause = 1f;
            }
        }

        UpdateTexts();
        Thread.Sleep((int)(timeToPause * 100f));

        if (timeToPause > 0.1)
        {
            timeToPause -= 0.05f;
        }
        else
        {
            timeToPause = 0.1f;
        }


        if (anyTimerReachedZero)
        {
            counting = false;
        }
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
            if (i == activeTimerIndex)
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
}

public class TimerData
{
    public int Value { get; set; }
    public string Tag { get; set; }
}
