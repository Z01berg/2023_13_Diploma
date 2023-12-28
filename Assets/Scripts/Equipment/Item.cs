using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName = "PLC";
    [TextArea]
    public string description;
    public List<Object> cards = new List<Object>();
    public Sprite icon;
}
