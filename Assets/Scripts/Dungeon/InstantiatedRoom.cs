using CardActions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
    [HideInInspector] public Room room;
    [HideInInspector] public UnityEngine.Grid grid;

    [HideInInspector] public Tilemap ground;

    //[HideInInspector] public Tilemap wall;//TODO: GRID??
    [HideInInspector] public Tilemap decorative;
    [HideInInspector] public Tilemap collision;
    [HideInInspector] public Tilemap miniMap;
    [HideInInspector] public Bounds roomColliderBounds;

    public GameObject enemyPrefab;
    public GameObject timer;
    public List<GameObject> enemyInRoomList = new();
    public List<GameObject> doorsList = new();

    private static bool _baseEnemyMoved = false;
    private bool _cleared = false;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        //Save room collider bounds
        roomColliderBounds = boxCollider2D.bounds;
    }

    public void Initialise(GameObject roomGameObject)
    {
        PopulateTilemapMemeberVariables(roomGameObject);

        BlockOffUnusedDoorWays();
        
        DisableCollisionTilemapRenderer();
        
        AddDoorsToRooms(roomGameObject.transform);
    }

    private void Update()
    {
        if(enemyInRoomList.Count == 0)
        {
            OpenAllDoors();
        }
    }

    private void AddDoorsToRooms(Transform roomTransform)
    {
        if (room.RoomNodeType.isCorridorEW || room.RoomNodeType.isCorridorNS) return;

        foreach (Doorway doorway in room.DoorWayList)
        {
            if (doorway.DoorPrefab != null && doorway.IsConnected)
            {
                float tileDistance = Settings.TILE_SIZE_PIXEL / Settings.PIXEL_PER_UNIT;

                GameObject door = null;

                if (doorway.Orientation == Orientation.north)
                {
                    door = Instantiate(doorway.DoorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.Position.x + tileDistance / 2f + 10.04f,
                        doorway.Position.y + tileDistance - 9.45f, 0f);
                }
                else if (doorway.Orientation == Orientation.south)
                {
                    door = Instantiate(doorway.DoorPrefab, gameObject.transform);
                    door.transform.rotation = new Quaternion(180f, 0f, 0f, 0f);
                    door.transform.localPosition =
                        new Vector3(doorway.Position.x + tileDistance / 2f + 10.04f, doorway.Position.y- 9.45f, 0f);
                }
                else if (doorway.Orientation == Orientation.east)
                {
                    door = Instantiate(doorway.DoorPrefab, gameObject.transform);
                    door.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                    door.transform.localPosition = new Vector3(doorway.Position.x + tileDistance / 2f + 10,
                        doorway.Position.y + tileDistance- 9.415f, 0f);
                }
                else if (doorway.Orientation == Orientation.west)
                {
                    door = Instantiate(doorway.DoorPrefab, gameObject.transform);
                    door.transform.localPosition =
                        new Vector3(doorway.Position.x / 2f + 10, doorway.Position.y + tileDistance- 9.415f, 0f);
                }
                doorsList.Add(door);
            }
        }
    }

    private void BlockOffUnusedDoorWays()
    {
        foreach (Doorway doorway in room.DoorWayList)
        {
            if (doorway.IsConnected)
                continue;

            if (collision != null)
            {
                BlockADoorwayOnTilemapLayer(collision, doorway);
            }

            if (ground != null)
            {
                BlockADoorwayOnTilemapLayer(ground, doorway);
            }

            if (decorative != null)
            {
                BlockADoorwayOnTilemapLayer(decorative, doorway);
            }

            if (miniMap != null)
            {
                BlockADoorwayOnTilemapLayer(miniMap, doorway);
            }
        }
    }

    private void BlockADoorwayOnTilemapLayer(Tilemap tilemap, Doorway doorway)
    {
        switch (doorway.Orientation)
        {
            case Orientation.north:
            case Orientation.south:
                BlockDoorwayHorizontally(tilemap, doorway);
                break;

            case Orientation.east:
            case Orientation.west:
                BlockDoorwayVertically(tilemap, doorway);
                break;

            case Orientation.none:
                break;
        }
    }

    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.DoorwayStartCopyPosition;

        for (int yPos = 0; yPos < doorway.DoorwayCopyTileHeight; yPos++)
        {
            for (int xPos = 0; xPos < doorway.DoorwayCopyTileWidth; xPos++)
            {
                //get rotation
                Matrix4x4 transformMatrix =
                    tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                //copy
                tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0),
                    tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));
                //set rotation
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0),
                    transformMatrix);
            }
        }
    }

    private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.DoorwayStartCopyPosition;

        for (int xPos = 0; xPos < doorway.DoorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.DoorwayCopyTileHeight; yPos++)
            {
                //get rotation
                Matrix4x4 transformMatrix =
                    tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                //copy
                tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0),
                    tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                //set rotation
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0),
                    transformMatrix);
            }
        }
    }

    private void DisableCollisionTilemapRenderer()
    {
        collision.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }

    private void PopulateTilemapMemeberVariables(GameObject roomGameobject)
    {
        grid = roomGameobject.GetComponentInChildren<UnityEngine.Grid>();

        Tilemap[] tilemaps = roomGameobject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.CompareTag("groundTilemap"))
            {
                ground = tilemap;
            }
            /*
            else if (tilemap.gameObject.CompareTag("frontTilemap"))
            {
                wall = tilemap;
            }*/
            else if (tilemap.gameObject.CompareTag("decoration1Tilemap"))
            {
                decorative = tilemap;
            }
            else if (tilemap.gameObject.CompareTag("collisionTilemap"))
            {
                collision = tilemap;
            }
            else if (tilemap.gameObject.CompareTag("minimapTilemap"))
            {
                miniMap = tilemap;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyInRoomList.Count == 0 && !collision.gameObject.CompareTag("Player")) return;
        CloseAllDoors();
    }

    /*
    private void PopulateRoomWithEnemies()
    {
        var positions = room.SpawnPositionArray;
        if (positions.Count() == 1) return;

        foreach (var position in positions)
        {
            var enemy = Instantiate(enemyPrefab, transform.Find("Grid"));
            enemy.gameObject.GetComponentInChildren<ApplyCardEffect>().gameObjectTimer = timer;
            enemy.transform.localPosition = new Vector3(position.x, position.y, -6f);
            enemyInRoomList.Add(enemy);
            enemy.GetComponent<HealthBar>().room = this;
        }
    }
    */
    
    public void OpenAllDoors()
    {
        //_cleared = true;
        foreach(var door in doorsList)
        {
            door.GetComponent<DoorLogic>().RoomCleared();
        }
        EventSystem.InstatiatedRoom.Invoke();
    }

    public void CloseAllDoors()
    {
        if (_cleared) return;
        _cleared = true;
        var positions = room.SpawnPositionArray;
        if (positions.Count() == 1) return;

        foreach (var position in positions)
        {
            var enemy = Instantiate(enemyPrefab, transform.Find("Grid"));
            enemy.gameObject.GetComponentInChildren<ApplyCardEffect>().gameObjectTimer = timer;
            enemy.transform.localPosition = new Vector3(position.x, position.y, -6f);
            enemyInRoomList.Add(enemy);
            enemy.GetComponent<HealthBar>().room = this;
        }
        
        foreach (var door in doorsList)
        {
            door.GetComponent<DoorLogic>().CloseDoors();
        }
        EventSystem.InstatiatedRoom.Invoke();
    }
}