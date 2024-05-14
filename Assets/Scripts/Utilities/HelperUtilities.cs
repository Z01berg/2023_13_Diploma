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
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "" || stringToCheck == " ")
        {
            Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }

        return false;
    }
    
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName,
        IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fieldName + " is null in object " + thisObject.name.ToString());
            return true;
        }
        
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
    
    public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        UnityEngine.Grid grid = currentRoom.InstantiatedRoom.grid;

        Vector3 nearestSpawnPosition = Vector3.zero;
        float nearestDistance = Mathf.Infinity;

        foreach (Vector2Int spawnPositionGrid in currentRoom.SpawnPositionArray)
        {
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

            float distanceToPlayer = Vector3.Distance(spawnPositionWorld, playerPosition);

            if (distanceToPlayer < nearestDistance)
            {
                nearestSpawnPosition = spawnPositionWorld;
                nearestDistance = distanceToPlayer;
            }
        }

        return nearestSpawnPosition;
    }

}
