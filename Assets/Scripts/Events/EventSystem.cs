using UnityEngine;
using UnityEngine.Events;

public class EventSystem: MonoBehaviour
{
    public static UnityEvent PlayerMove = new UnityEvent();
    public static UnityEvent<GameObject> WhatHP = new UnityEvent<GameObject>();
}
