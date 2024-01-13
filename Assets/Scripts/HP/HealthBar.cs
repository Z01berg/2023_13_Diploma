using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private int health = 10;

    private void Start()
    {
        SetMaxHealth(health);
    }

    private void Update()
    {
        SetHealth(health);
    }


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void ChangeHealth(int health)
    {
        this.health += health;
    }
}
