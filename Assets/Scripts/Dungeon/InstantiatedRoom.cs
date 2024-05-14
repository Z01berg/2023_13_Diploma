using System.Collections;
using System.Collections.Generic;
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
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), transformMatrix);
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
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), transformMatrix);
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
    
}
