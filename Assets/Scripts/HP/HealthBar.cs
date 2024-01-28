using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject game_object;
    [SerializeField] private GameObject body;
    
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text data;
    private string[] parts;
    private int maxValue = 0;
    private int value = 0;
    
    private bool This = false;
    
    private void Start()
    {
        Split();
        SetData();
        UpdateHealthText();
        
        EventSystem.WhatHP.AddListener(HandleWhatHP);
    }

    private void Update()
    {
        Kill();

        if (Input.GetKeyDown(KeyCode.P) && This)
        {
            ChangeHealth(-1);
            UpdateHealthText();
        }
    }

    private void Split()
    {
        parts = (data.text).Split('/');
    }

    private void SetData()
    {
        maxValue = int.Parse(parts[1]);
        value = int.Parse(parts[0]);
    }

    private void UpdateHealth()
    {
        value = int.Parse(parts[0]);
    }

    public void ChangeHealth(int health)
    {
        value += health;
    }

    private void UpdateHealthText()
    {
        data.text = value.ToString() + " / " + maxValue.ToString();
    }

    private void Kill()
    {
        if (value <= 0 && This)
        {
            Destroy(game_object);
            Destroy(body);
        }
    }

    private void HandleWhatHP(GameObject recieved)
    {
        if (recieved != game_object)
        {
            This = !This;
        }
    }
}