using UnityEngine;
using UnityEngine.Events;

public static class EventSystem
{
    public static UnityEvent<bool> PlayerMove = new UnityEvent<bool>();
    public static UnityEvent<GameObject> WhatHP = new UnityEvent<GameObject>();
    public static UnityEvent Death = new UnityEvent();
}
