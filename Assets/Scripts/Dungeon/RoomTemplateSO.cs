using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * Public class RoomTemplateSO jest obiektem skryptowym (ScriptableObject) reprezentującym szablon pokoju w grze.
 *
 * Zawiera informacje o:
 * - prefabrykacie pokoju
 * - typie węzła pokoju
 * - granicach pokoju
 * - listach drzwi i pozycji spawnu
 *
 * Działa w trybie edytora:
 * - automatycznie aktualizuje identyfikator GUID po zmianie prefabrykatu
 * - sprawdza poprawność listy drzwi i pozycji spawnu
 *
 * Możliwe akcje:
 * - pobranie listy drzwi dla szablonu pokoju
 */

[CreateAssetMenu(fileName = "Room_ ", menuName ="Scriptable Objects/Dungeon/Room")]
public class RoomTemplateSO : ScriptableObject
{
   [HideInInspector] public string guid;

   #region Header ROOM PREFAB

   [Space(10)]
   [Header("ROOM PREFAB")]

   #endregion Header ROOM PREFAB

   #region Tooltip

   [Tooltip("The gameobject prefab for the room (this will contain all the tilemaps for the room and enviroment game objects)")]

   #endregion Tooltip
   public GameObject prefab;

   [HideInInspector] public GameObject previousPrefab; // this is used to regenarate guid if the so is copied and the prefab is changed

   #region Header ROOM CONFIGURATION

   [Space(10)]
   [Header("ROOM CONFIGURATION")]

   #endregion Header ROOM CONFIGURATION

   #region Tooltip

   [Tooltip("The rrom node type SO. The room node types correspond to the room nodes used in the room node graph. The exceptions being with corridors.\n" +
            "In the room node graph there is just one corridor type 'Corridor'.\n" +
            "For the room templates there are 2 corridor node types - CorridorsNS and CorridorEW")]

   #endregion Tooltip
   public RoomNodeTypeSO RoomNodeType;
   
   #region Tooltip

   [Tooltip("If you imagine a rectangle around the room tilemap that just completely encloses it, the room lower bounds represent the bottom left corner of that rectangle.\n" +
            "This should be setermined from the tilemap for the room (using the cordinate brush pointer to get the tilemap grid position for that bottom left corner)\n" +
            "Note: this is the local tilemap position and NOT World Position")]

   #endregion Tooltip
   public Vector2Int LowerBounds;
   
   #region Tooltip

   [Tooltip("If you imagine a rectangle around the room tilemap that just completely encloses it, the room upper bounds represent the top right corner of that rectangle.\n" +
            "This should be setermined from the tilemap for the room (using the cordinate brush pointer to get the tilemap grid position for that top right corner)\n" +
            "Note: this is the local tilemap position and NOT World Position")]

   #endregion Tooltip
   public Vector2Int UpperBounds;
   
   #region Tooltip

   [Tooltip("There should be a maximum of four doorways for a room - one for each compass direction.\n" +
            "These should have a consistene 2 tile opening size\n\n" +
            "POSITION:\n" +
            "NW: Left most upper corner of doorway\n" +
            "S: Left most downer corner of doorway\n" +
            "E: Right most upper corner of doorway\n\n" +
            "COPYING FROM: (block from where you are copying blocks for changing)\n" +
            "NSWE: The most Left grid From where you starting copy\nFloor should be changed as well!!!\n\n" +
            "COPY OVER:\n" +
            "From the point of COPYING draw rentagular of whole enterance to opposite COPYING point (like hiding enterance) and remember numbers near cordinates like size(width; height)")]

   #endregion Tooltip
   [SerializeField] public List<Doorway> DoorwayList;

   #region Tooltip

   [Tooltip("Each possible spawn position (used for enemies and chests) for the room in tilemap coordinates should be added to this array")]

   #endregion Tooltip
   public Vector2Int[] SpawnPositionArray;
   
   // Returns the list of Entrances for the room template
   public List<Doorway> GetDoorwayList()
   {
       return DoorwayList;
   }

   #region Validation

#if UNITY_EDITOR
    //Validate SO fields
    private void onValidate()
    {
        // Set unique GUID if empty or the prefab changes
        if (guid == "" || previousPrefab != prefab)
        {
            guid = GUID.Generate().ToString();
            previousPrefab = prefab;
            EditorUtility.SetDirty(this);
        }

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(DoorwayList), DoorwayList);
        
        // Check spawn position populated
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(SpawnPositionArray), SpawnPositionArray);
    }
#endif   

   #endregion Validation
   
}
