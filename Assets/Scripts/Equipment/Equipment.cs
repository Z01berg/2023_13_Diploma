using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/**
 * Class purpose is to store information about the equipment put in item slots, and about cards 
 * assigned to your deck.
 * There can be only one instance of this class in a scene.
 */

public class Equipment : MonoBehaviour
{

    #region Singleton

    public static Equipment Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of Equipment found!");
            //return;
        }
        Instance = this;
    }

    #endregion

    public Item head;
    public Item chest;
    public Item legs;
    public Item boots;
    public Item rightHand;
    public Item leftHand;

    public Item item1;
    public Item item2;
    public Item item3;
    public Item item4;
    public Item item5;
    public Item item6;

    public List<CardsSO> cards = new List<CardsSO>();

}
