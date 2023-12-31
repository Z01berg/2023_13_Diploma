using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Equipment : MonoBehaviour
{

    #region Singleton

    public static Equipment Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of Equipment found!");
            return;
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

    public List<GameObject> cards = new List<GameObject>();

}
