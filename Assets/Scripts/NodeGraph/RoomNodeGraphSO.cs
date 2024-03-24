using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * Public class RoomNodeGraphSO jest obiektem skryptowym (ScriptableObject) reprezentującym graf węzłów pokoi w grze.
 *
 * Zawiera informacje o:
 * - liście węzłów pokoi
 * - słowniku węzłów pokoi
 * - typach węzłów pokoi
 * - węźle, z którego prowadzony jest aktualnie rysowany połączenie
 * - pozycji aktualnie rysowanego połączenia
 *
 * Działa w trybie edytora:
 * - automatycznie aktualizuje słownik węzłów po każdej zmianie
 * - umożliwia rysowanie połączeń między węzłami w trybie edytora
 *
 * Możliwe akcje:
 * - pobranie węzła pokoju na podstawie jego identyfikatora
 * - ustawienie węzła, z którego będzie prowadzone połączenie, w trybie edytora
 */

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();

    private void Awake()
    {
        LoadRoomNodeDictionary();
    }
    
    //load the room node dictionary from the room node list
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();
        
        //Populate dictionary
        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.ID] = node;
        }
    }

    //Get room node by room nodeID
    public RoomNodeSO GetRoomNode(string roomNodeID)
    {
        if (roomNodeDictionary.TryGetValue(roomNodeID, out RoomNodeSO roomNode))
        {
            return roomNode;
        }
        return null;
    }
    
    #region Editor Code

#if UNITY_EDITOR
    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null;
    [HideInInspector] public Vector2 linePosition;

    //Repopulate node dictionary every time a change is made in the editor
    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }

    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
    {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
    }
#endif

    #endregion Editor Code
}
