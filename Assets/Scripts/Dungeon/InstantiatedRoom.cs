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

        DisableCollisionTilemapRenderer();
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
