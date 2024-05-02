using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string Id;
    public string TemplateID;
    public GameObject Prefab;
    public RoomNodeTypeSO RoomNodeType;
    public Vector2Int LowerBounds;
    public Vector2Int UpperBounds;
    public Vector2Int TemplateLowerBounds;
    public Vector2Int TemplateUpperBounds;
    public Vector2Int[] SpawnPositionArray;
    public List<string> ChildRoomIdList;
    public string ParentRoomId;
    public List<Doorway> DoorWayList;
    public bool IsPositioned = false;
    public InstantiatedRoom InstantiatedRoom;
    public bool IsList = false;
    public bool IsClearedOffEnemies = false;
    public bool IsPreviousVisited = false;

    public Room()
    {
        ChildRoomIdList = new List<string>();
        DoorWayList = new List<Doorway>();
    }
}
