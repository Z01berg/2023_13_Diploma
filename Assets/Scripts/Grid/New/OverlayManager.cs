using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grid.New;
using UnityEngine;
using UnityEngine.Tilemaps;


public class OverlayManager : MonoBehaviour
{
    private static OverlayManager _instance;
    
    public static OverlayManager Instance => _instance;

    public GameObject overlayPrefab;
    public GameObject overlayContainer;


    public Dictionary<Vector2, OverlayTile> map;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    public void CreateOverlaysForRoom(List<Tilemap> groundTilemaps, List<Tilemap> wallTilemaps)
    {

        if (overlayContainer.transform.childCount != 0)
        {
            ResetOverContainer();
        }
        
        map = new Dictionary<Vector2, OverlayTile>();
        
        foreach (var groundTm in groundTilemaps)
        {
            var wallTm = wallTilemaps[groundTilemaps.IndexOf(groundTm)];
            BoundsInt bounds = groundTm.cellBounds;

            for (int z = bounds.max.z; z > bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        var groundTileLocation = new Vector3Int(x, y, z);
                        var cellWorldPosition = groundTm.GetCellCenterWorld(groundTileLocation);
                        
                        if (wallTm.HasTile(wallTm.WorldToCell(cellWorldPosition))) //check if there is a wall tile on the same spot so the overlay tile won't generate in walls
                        {
                            continue;
                        }
                        
                        var tileKey = new Vector2((float)Math.Round(cellWorldPosition.x,3), (float)Math.Round(cellWorldPosition.y,3));

                        if (groundTm.HasTile(groundTileLocation - new Vector3Int(0, 0, 1)) && groundTm.gameObject
                                .GetComponent<TilemapRenderer>().sortingLayerName.Equals("Ground"))
                        {
                            if (!map.ContainsKey(tileKey))
                            {
                                var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y,
                                    cellWorldPosition.z + 1);
                                // overlayTile.GetComponent<SpriteRenderer>().sortingOrder =
                                //     tm.GetComponent<TilemapRenderer>().sortingOrder+1;
                                overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = tileKey;
                                map.Add(tileKey, overlayTile.gameObject.GetComponent<OverlayTile>());
                            }
                        }
                    }
                }
            }
        }
    }

    private void ResetOverContainer()
    {
        foreach (Transform child in overlayContainer.transform) {
            Destroy(child.gameObject);
        }
    }

    public List<OverlayTile> GetRangeTiles(Vector2 playerOccupiedTile)
    {
        var rangeTiles = new List<OverlayTile>();

        Vector2 playerPos = new Vector2((float)Math.Round(playerOccupiedTile.x, 3), (float)Math.Round(playerOccupiedTile.y, 3));

        rangeTiles.Add(map[playerPos]);
        
        Vector2 tileToCheck = new Vector2((float)Math.Round(playerOccupiedTile.x + 1,3), (float)Math.Round(playerOccupiedTile.y,3));
        if (map.ContainsKey(tileToCheck))
        {
            rangeTiles.Add(map[tileToCheck]);
        }  
        
        tileToCheck = new Vector2((float)Math.Round(playerOccupiedTile.x,3), (float)Math.Round(playerOccupiedTile.y + 1,3));

        if (map.ContainsKey(tileToCheck))
        {
            rangeTiles.Add(map[tileToCheck]);
        }  
        
        tileToCheck = new Vector2((float)Math.Round(playerOccupiedTile.x - 1,3), (float)Math.Round(playerOccupiedTile.y,3));

        if (map.ContainsKey(tileToCheck))
        {
            rangeTiles.Add(map[tileToCheck]);
        }  
        
        tileToCheck = new Vector2((float)Math.Round(playerOccupiedTile.x,3), (float)Math.Round(playerOccupiedTile.y - 1,3));

        if (map.ContainsKey(tileToCheck))
        {
            rangeTiles.Add(map[tileToCheck]);
        }

        return rangeTiles;
    }
}