using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Publiczna klasa sluzaca do zapisywania postepow gracza do pliku json. klasa zawiera w sobie liste 
 * wszystkich posiadanych itemow oraz miejsca w ekwipunku do ktorych moga zostac dodane nazwy przedmiotow sie tam znajdujacych
 */

public class SaveTemplate
{
    public List<string> inventory = new List<string>();

    public string head;
    public string chest;
    public string legs;
    public string boots;
    public string rightHand;
    public string leftHand;

    public string item1;
    public string item2;
    public string item3;
    public string item4;
    public string item5;
    public string item6;
}
