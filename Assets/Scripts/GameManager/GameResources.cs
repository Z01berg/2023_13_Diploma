using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Klasa GameResources jest odpowiedzialna za zarządzanie zasobami gry.
 *
 * Ma możliwość:
 * - Udostępniania instancji GameResources.
 * - Przechowywania referencji do listy typów pokojów w danżonie.
 */

public class GameResources : MonoBehaviour
{
   private static GameResources _instance;

   public static GameResources Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = Resources.Load<GameResources>("GameResources");
         }

         return _instance;
      }
   }

   #region Header DUNGEON

   [Space(10)]
   [Header("DUNGEON")]

   #endregion
   #region Tooltip

   [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]

   #endregion
   public RoomNodeTypeListSO roomNodeTypeList;
}
