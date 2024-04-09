using System.Collections.Generic;
using UnityEngine;

/**
 * Publiczna klasa przechowujaca informacje na temat przypisanego do postaci
 * ekwipunku oraz pozyskanych z niego kart.
 * Kazdy element ekwipunku przypisywany jest do zmiennej odpowiadajacej jego polozeniu.
 * Karty przypisane sa odpowiednio do listy publicznej listy cards, do kturej dostep
 * mozna uzyskac poprzez uzycie polecenia "Equipment.Instance.cards"
 * W scenie powinna znajdowac sie tylko jedna instancja tej klasy.
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

    public List<CardsSO> cards = new List<CardsSO>();

}
