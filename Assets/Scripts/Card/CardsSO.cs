using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card",menuName ="Scriptable Objects/Card")]
public class CardsSO : ScriptableObject
{
    public int id;
    public int range;
    public bool isActive;
    public int cardQuality;
    public string title;
    public string description;
    public int cost;
    public int damage;
    public int move;
    public string backgroundPath;
    public string spritePath;
    public object transform;
}
