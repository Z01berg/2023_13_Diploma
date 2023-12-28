using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName = "PLC";
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private List<Object> cards = new List<Object>();
    [SerializeField] private Sprite icon;
}
