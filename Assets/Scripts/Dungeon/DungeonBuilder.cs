using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Publiczna klasa DungeonBuilder dziedzicząca po SingletonMonobehaviour
 *
 * Ma za zadanie {get; set} 3 rzeczy:
 * - Tworzenie losowego labiryntu
 * - Generowanie losowego labiryntu
 * - Sprawdzanie czy dwa przedziały się nakładają na siebie
 * 
 */

[DisallowMultipleComponent]
public class DungeonBuilder : SingletonMonobehaviour<DungeonBuilder>
{
    public Dictionary<string, Room> DungeonBuilderRoomDictionary = new Dictionary<string, Room>();
    
    private Dictionary<string, RoomTemplateSO> _roomTemplateDictionary = new Dictionary<string, RoomTemplateSO>();
    private List<RoomTemplateSO> _roomTemplateList = null;
    private RoomNodeTypeListSO _roomNodeTypeList;
    private bool _dungeonBuildSuccessful;
    private OverlayManager _overlayManager;
    
    
    public GameObject portal;
    public GameObject enemyPrefab;
    public GameObject eE_Miner;
    public GameObject eE_Wizard;
    public GameObject eE_Warrior;
    public GameObject eE_Skeleton;
    public GameObject eE_Clerick;
    public GameObject eE_Explorer;
    public GameObject timer;

    protected override void Awake()
    {
        base.Awake();

        LoadRoomNodeTypeList();

        // Set dimed material to fully visible
        //  GameResources.Instance.dimedMaterial.SetFloat("Alpha_Slider", 1f);//TODO check if use material
    }

    private void Start()
    {
        _overlayManager = GetComponent<OverlayManager>();
    }

    private void LoadRoomNodeTypeList()
    {
        _roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    // Generate random dungeon, returns true if dungeon built, false if failed
    public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
    {
        _roomTemplateList = currentDungeonLevel.roomTemplateList;

        LoadRoomTemplatesIntoDictionary();

        _dungeonBuildSuccessful = false;
        int dungeonBuildAttempts = 0;

        while (!_dungeonBuildSuccessful && dungeonBuildAttempts < Settings.MAX_DUNGEON_BUILD_ATTEMPTS)
        {
            dungeonBuildAttempts++;

            RoomNodeGraphSO roomNodeGraph = SelectRandomRoomNodeGraph(currentDungeonLevel.roomNodeGraphList);

            int dungeonRebuildAttempsForNodeGraph = 0;
            _dungeonBuildSuccessful = false;

            // Loop until dungeon successfully built or more than max attempts for node graph
            while (!_dungeonBuildSuccessful &&
                   dungeonRebuildAttempsForNodeGraph <= Settings.MAX_DUNGEON_REBUILD_ATTEMPTS_FOR_ROOM_GRAPH)
            {
                // Clear dungeon room gameobjects and dungeon room dictionary
                ClearDungeon();

                dungeonRebuildAttempsForNodeGraph++;

                // Attempt to build a random dungeon for selected room node graph
                _dungeonBuildSuccessful = AttemptToBuiltRandomDungeon(roomNodeGraph);
            }

            if (_dungeonBuildSuccessful)
            {
                // Instantiate Room GameObjects
                InstantiatedRoomGameobjects();
            }
        }

        return _dungeonBuildSuccessful;
    }

    // init prefabs
    private void InstantiatedRoomGameobjects()
    {
        List<Tilemap> groundTileMap = new List<Tilemap>();
        List<Tilemap> wallTileMap = new List<Tilemap>();
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            Vector3 roomPosition = new Vector3(room.LowerBounds.x - room.TemplateLowerBounds.x,
               room.LowerBounds.y - room.TemplateLowerBounds.y, 0f);

            GameObject roomGameObject = Instantiate(room.Prefab, roomPosition, Quaternion.identity, transform);
            // Debug.Log($"room position {roomPosition.x}, {roomPosition.y}");

            InstantiatedRoom instantiatedRoom = roomGameObject.GetComponentInChildren<InstantiatedRoom>();
            instantiatedRoom.portal = portal;
            instantiatedRoom.enemyPrefab = enemyPrefab;
            instantiatedRoom.eE_Miner = eE_Miner;
            instantiatedRoom.eE_Wizard = eE_Wizard;
            instantiatedRoom.eE_Warrior = eE_Warrior;
            instantiatedRoom.eE_Skeleton = eE_Skeleton;
            instantiatedRoom.eE_Clerick = eE_Clerick;
            instantiatedRoom.eE_Explorer = eE_Explorer;
            instantiatedRoom.timer = timer;

            instantiatedRoom.room = room;
            instantiatedRoom.Initialise(roomGameObject);

            room.InstantiatedRoom = instantiatedRoom;

            groundTileMap.Add(roomGameObject.transform.GetComponentsInChildren<Tilemap>().First(x => x.GetComponent<TilemapRenderer>().sortingLayerName.Equals("Ground")));
            wallTileMap.Add(roomGameObject.transform.GetComponentsInChildren<Tilemap>().First(x => x.GetComponent<TilemapRenderer>().sortingLayerName.Equals("Wall")));
        }
        _overlayManager.CreateOverlaysForRoom(groundTileMap, wallTileMap);

    }

    private bool AttemptToBuiltRandomDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        Queue<RoomNodeSO> openRoomNodeQueue = new Queue<RoomNodeSO>();

        // Add enteranace Node to Room Node Queue from Room Node Graph
        RoomNodeSO enteranceNode = roomNodeGraph.GetRoomNode(_roomNodeTypeList.list.Find(x => x.isEntrance));

        if (enteranceNode != null)
        {
            openRoomNodeQueue.Enqueue(enteranceNode);
        }
        else
        {
            Debug.Log("No Enterance Node");
            return false; // Dungeon n ot build
        }

        // Start with no room overlaps
        bool noRoomOverlaps = true;

        // Process open room nodes queue
        noRoomOverlaps = ProcessRoomsInOpenRoomNodeQueue(roomNodeGraph, openRoomNodeQueue, noRoomOverlaps);

        // if all the room nodes have been processed and there hasn't been a room overlap then return true
        if (openRoomNodeQueue.Count == 0 && noRoomOverlaps)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Process rooms in the  open room node queue, returning true if there are no room overlaps
    private bool ProcessRoomsInOpenRoomNodeQueue(RoomNodeGraphSO roomNodeGraph, Queue<RoomNodeSO> openRoomNodeQueue, bool noRoomOverlaps)
    {
        // While room nodes is open room node queue & no room overlaps detected
        while (openRoomNodeQueue.Count > 0 && noRoomOverlaps == true)
        {
            // Get next room node from open room node queue
            RoomNodeSO roomNode = openRoomNodeQueue.Dequeue();

            // Add child Nodes to queue from room node graph (with links to this parent Room)
            foreach (RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNodes(roomNode))
            {
                openRoomNodeQueue.Enqueue(childRoomNode);
            }

            // if the room is the enterance mark as positioned and add to room dictionary
            if (roomNode.roomNodeType.isEntrance)
            {
                RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);

                Room room = CreateRoomFromTemplate(roomTemplate, roomNode);
                room.IsPositioned = true;

                DungeonBuilderRoomDictionary.Add(room.Id, room);
            }

            else // If not enterance
            {
                Room parentRoom = DungeonBuilderRoomDictionary[roomNode.parentRoomNodeIDList[0]];

                noRoomOverlaps = CanPlaceRoomWithNoOverlaps(roomNode, parentRoom);
            }
        }

        return noRoomOverlaps;
    }

    // Attempt ot place the room node in the dungeon - if room can be placed return the room, else return null
    private bool CanPlaceRoomWithNoOverlaps(RoomNodeSO roomNode, Room parentRoom)
    {
        bool roomOverlaps = true;

        while (roomOverlaps)
        {
            List<Doorway> unconnectedAvailableParentDoorways = GetUnconnectedAvailableDoorways(parentRoom.DoorWayList).ToList();

            if (unconnectedAvailableParentDoorways.Count == 0)
            {
                return false;
            }

            Doorway doorwayParent = unconnectedAvailableParentDoorways[UnityEngine.Random.Range(0, unconnectedAvailableParentDoorways.Count)];

            RoomTemplateSO roomTemplate = GetRandomRoomTemplateForRoomConsistentWithParent(roomNode, doorwayParent);

            Room room = CreateRoomFromTemplate(roomTemplate, roomNode);

            if (PlaceTheRoom(parentRoom, doorwayParent, room))
            {
                roomOverlaps = false;

                room.IsPositioned = true;

                DungeonBuilderRoomDictionary.Add(room.Id, room);
            }
            else
            {
                roomOverlaps = true;
            }
        }

        return true;
    }

    // Place the room  - returns true if the room doesn't overlap, false otherwise
    private bool PlaceTheRoom(Room parentRoom, Doorway doorwayParent, Room room)
    {
        Doorway doorway = GetOppositeDoorway(doorwayParent, room.DoorWayList);

        if (doorway == null)
        {
            doorwayParent.IsUnavailable = true;

            return false;
        }

        Vector2Int parentDoorwayPosition = parentRoom.LowerBounds + doorwayParent.Position - parentRoom.TemplateLowerBounds;

        Vector2Int adjustment = Vector2Int.zero;

        // Calculate adjustment position offset based on room doorway position that we are trying to connect (if this doorway is west then we need to add (1,0) to the east parent doorway)
        switch (doorway.Orientation)
        {
            case Orientation.north:
                adjustment = new Vector2Int(0, -1);
                break;
            case Orientation.east:
                adjustment = new Vector2Int(-1, 0);
                break;
            case Orientation.south:
                adjustment = new Vector2Int(0, 1);
                break;
            case Orientation.west:
                adjustment = new Vector2Int(1, 0);
                break;
            case Orientation.none:
                break;

            default:
                break;
        }

        room.LowerBounds = parentDoorwayPosition + adjustment + room.TemplateLowerBounds - doorway.Position;
        room.UpperBounds = room.LowerBounds + room.TemplateUpperBounds - room.TemplateLowerBounds;

        Room overlappingRoom = CheckForRoomOverlap(room);

        if (overlappingRoom == null)
        {
            doorwayParent.IsConnected = true;
            doorwayParent.IsUnavailable = true;

            doorway.IsConnected = true;
            doorway.IsUnavailable = true;

            return true;
        }
        else
        {
            doorwayParent.IsUnavailable = true;

            return false;
        }

    }

    // Check for rooms that overlap the upper and lower bounds parameters, and if there are overlapping rooms then return room else return null
    private Room CheckForRoomOverlap(Room roomToTest)
    {
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            if (room.Id == roomToTest.Id || !room.IsPositioned)
                continue;

            if (IsOverLappingRoom(roomToTest, room))
            {
                return room;
            }
        }

        return null;
    }

    // Get a room template by room template ID returns null if ID doesn't exist
    public RoomTemplateSO GetRoomTemplate(string roomTemplateID)
    {
        if (_roomTemplateDictionary.TryGetValue(roomTemplateID, out RoomTemplateSO roomTemplate))
        {
            return roomTemplate;
        }
        else
        {
            return null;
        }
    }

    // Get room by roomID, if no room exists with that ID return null
    public Room GetRoomByRoomID(string roomID)
    {
        if (DungeonBuilderRoomDictionary.TryGetValue(roomID, out Room room))
        {
            return room;
        }
        else
        {
            return null;
        }
    }

    // Check if 2 rooms overlap each other - return true if they overlap or false if they don't overlap
    private bool IsOverLappingRoom(Room room1, Room room2)
    {
        bool isOverlappingX = IsOverlappingInterval(room1.LowerBounds.x, room1.UpperBounds.x, room2.LowerBounds.x, room2.UpperBounds.x);
        bool isOverlappingY = IsOverlappingInterval(room1.LowerBounds.y, room1.UpperBounds.y, room2.LowerBounds.y, room2.UpperBounds.y);

        if (isOverlappingX && isOverlappingY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Check if interval 1 overlaps interval 2
    private bool IsOverlappingInterval(int imin1, int imax1, int imin2, int imax2)
    {
        if (Mathf.Max(imin1, imin2) <= Mathf.Min(imax1, imax2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Get the doorway from the doorway list that has the opposite orientation to doorway
    private Doorway GetOppositeDoorway(Doorway parentDoorway, List<Doorway> doorWayList)
    {
        foreach (Doorway doorwayToCheck in doorWayList)
        {
            if (parentDoorway.Orientation == Orientation.east && doorwayToCheck.Orientation == Orientation.west)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.Orientation == Orientation.west && doorwayToCheck.Orientation == Orientation.east)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.Orientation == Orientation.north && doorwayToCheck.Orientation == Orientation.south)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.Orientation == Orientation.south && doorwayToCheck.Orientation == Orientation.north)
            {
                return doorwayToCheck;
            }
        }

        return null;
    }

    // Get random roomtemplates for room node taking into account the parent doorway orientation
    private RoomTemplateSO GetRandomRoomTemplateForRoomConsistentWithParent(RoomNodeSO roomNode, Doorway doorwayParent)
    {
        RoomTemplateSO roomTemplate = null;

        if (roomNode.roomNodeType.isCorridor)
        {
            switch (doorwayParent.Orientation)
            {
                case Orientation.north:
                case Orientation.south:
                    roomTemplate = GetRandomRoomTemplate(_roomNodeTypeList.list.Find(x => x.isCorridorNS));
                    break;

                case Orientation.east:
                case Orientation.west:
                    roomTemplate = GetRandomRoomTemplate(_roomNodeTypeList.list.Find(x => x.isCorridorEW));
                    break;

                case Orientation.none:
                    break;

                default:
                    break;
            }
        }
        else
        {
            roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
        }

        return roomTemplate;
    }

    // Get unconnected Doorway
    private IEnumerable<Doorway> GetUnconnectedAvailableDoorways(List<Doorway> roomDoorwayList)
    {
        foreach (Doorway doorway in roomDoorwayList)
        {
            if (!doorway.IsConnected && !doorway.IsUnavailable)
            {
                yield return doorway;
            }
        }
    }

    // Create room based on romTemplate and layoutNode, and return the created room
    private Room CreateRoomFromTemplate(RoomTemplateSO roomTemplate, RoomNodeSO roomNode)
    {
        Room room = new Room();

        room.TemplateID = roomTemplate.guid;
        room.Id = roomNode.ID;
        room.Prefab = roomTemplate.prefab;
        room.RoomNodeType = roomTemplate.RoomNodeType;
        room.LowerBounds = roomTemplate.LowerBounds;
        room.UpperBounds = roomTemplate.UpperBounds;
        room.SpawnPositionArray = roomTemplate.SpawnPositionArray;
        room.TemplateLowerBounds = roomTemplate.LowerBounds;
        room.TemplateUpperBounds = roomTemplate.UpperBounds;

        room.ChildRoomIdList = CopyStringList(roomNode.childRoomNodeIDList);
        room.DoorWayList = CopyDoorwayList(roomTemplate.DoorwayList);

        if (roomNode.parentRoomNodeIDList.Count == 0) // Enterance
        {
            room.ParentRoomId = "";
            room.IsPreviousVisited = true;

            GameManager.Instance.SetCurrentRoom(room);
        }
        else
        {
            room.ParentRoomId = roomNode.parentRoomNodeIDList[0];
        }

        return room;
    }

    private List<Doorway> CopyDoorwayList(List<Doorway> oldDoorwaysList)
    {
        List<Doorway> newDoorwaysList = new List<Doorway>();

        foreach (Doorway doorway in oldDoorwaysList)
        {
            Doorway newDoorway = new Doorway();

            newDoorway.Position = doorway.Position;
            newDoorway.Orientation = doorway.Orientation;
            newDoorway.DoorPrefab = doorway.DoorPrefab;
            newDoorway.IsConnected = doorway.IsConnected;
            newDoorway.IsUnavailable = doorway.IsUnavailable;
            newDoorway.DoorwayStartCopyPosition = doorway.DoorwayStartCopyPosition;
            newDoorway.DoorwayCopyTileWidth = doorway.DoorwayCopyTileWidth;
            newDoorway.DoorwayCopyTileHeight = doorway.DoorwayCopyTileHeight;

            newDoorwaysList.Add(newDoorway);
        }

        return newDoorwaysList;
    }

    // Create deep copy of string list
    private List<string> CopyStringList(List<string> oldStringList)
    {
        List<string> newStringList = new List<string>();

        foreach (string stringValue in oldStringList)
        {
            newStringList.Add(stringValue);
        }

        return newStringList;
    }

    // Select random room template from list and return
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        List<RoomTemplateSO> matchingRoomTemplateList = new List<RoomTemplateSO>();

        foreach (RoomTemplateSO roomTemplate in _roomTemplateList)
        {
            if (roomTemplate.RoomNodeType == roomNodeType)
            {
                matchingRoomTemplateList.Add(roomTemplate);
            }
        }

        if (matchingRoomTemplateList.Count == 0)
            return null;

        return matchingRoomTemplateList[UnityEngine.Random.Range(0, matchingRoomTemplateList.Count)];

    }

    // Clear dungeon room gameobjects and dungeon room dictionary
    private void ClearDungeon()
    {
        if (DungeonBuilderRoomDictionary.Count > 0)
        {
            foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilderRoomDictionary)
            {
                Room room = keyValuePair.Value;

                if (room.InstantiatedRoom != null)
                {
                    Destroy(room.InstantiatedRoom.gameObject);
                }
            }

            DungeonBuilderRoomDictionary.Clear();
        }
    }

    private RoomNodeGraphSO SelectRandomRoomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if (roomNodeGraphList.Count > 0)
        {
            return roomNodeGraphList[UnityEngine.Random.Range(0, roomNodeGraphList.Count)];
        }
        else
        {
            Debug.Log("No room node graphs in list");
            return null;
        }
    }

    // Select a random room node graph from the list of room node graph
    private void LoadRoomTemplatesIntoDictionary()
    {
        _roomTemplateDictionary.Clear();

        foreach (RoomTemplateSO roomTemplate in _roomTemplateList)
        {
            if (!_roomTemplateDictionary.ContainsKey(roomTemplate.guid))
            {
                _roomTemplateDictionary.Add(roomTemplate.guid, roomTemplate);
            }
        }
    }
}
