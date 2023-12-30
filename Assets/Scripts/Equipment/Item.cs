using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public enum ItemType
{
    cheast,hand,head,boots,legs,additional,any
}

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
   
    public ItemType itemType;
    public string itemName = "PLC";
    [TextArea]
    public string description;
    public List<GameObject> cards = new List<GameObject>();
    public Sprite icon;
}
