using System.Collections.Generic;
using UnityEngine;

/**
 * Schemat na podstawie ktorego tworzone sa objekty typu Item.
 * Przechowuje informacje na ich temat:
 *  itemType -  typ itemu specyfikowany przez klase enum ItemType
 *  itemName - nazwe itemu
 *  description - opis itemu
 *  cards - liste kart do niego przypisanych
 *  icon - grafczna reprezentacje itemu
 */

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
    public List<CardsSO> cards = new List<CardsSO>();
    public Sprite icon;
}
