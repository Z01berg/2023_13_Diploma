using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Klasa HelperUtilities to statyczna klasa narzędziowa zawierająca metody pomocnicze do walidacji danych.
 *
 * * Metoda ValidateCheckEmptyString służy do sprawdzania czy dany ciąg znaków jest pusty.
     * Zwraca true, jeśli ciąg jest pusty, w przeciwnym razie zwraca false.
 *
 * * Metoda ValidateCheckEnumerableValues służy do sprawdzania czy kolekcja zawiera elementy oraz czy nie zawiera wartości null.
     * Zwraca true, jeśli kolekcja jest pusta lub zawiera wartości null.
 */

public static class HelperUtilities 
{
    //TODO Empty String check debug
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "" || stringToCheck == " ")
        {
            Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }

        return false;
    }
    
    //TODO List empty or contains null value check - returns true if there is an error
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName,
        IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        foreach (var item in enumerableObjectToCheck)
        {
            if (item == null)
            {
                Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }

        return error;
    }
}
