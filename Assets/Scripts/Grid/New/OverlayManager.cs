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


    public Dictionary<Vector2Int, OverlayTile> map;

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
    public void CreateOverlaysForRoom(List<Tilemap> tilemaps)
    {
        //var tileMap = newRoom.transform.GetComponentsInChildren<Tilemap>(); //OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
        // var motherTileMap = CreateMotherTileMap(tilemaps);
        // var motherTileMap = tilemaps[0];
        // Debug.Log(tilemaps[0].cellBounds);
        // Debug.Log(tilemaps[1].cellBounds);
        // Debug.Log(tilemaps[2].cellBounds);
        // Debug.Log(tilemaps[3].cellBounds);
        // Debug.Log(motherTileMap.cellBounds.position);
        // Debug.Log(motherTileMap.cellBounds.max);
        // Debug.Log(motherTileMap.cellBounds.min);
        
        map = new Dictionary<Vector2Int, OverlayTile>();
        
        
         // BoundsInt bounds = tm.cellBounds;
         // Debug.Log($"max x: {bounds.max.x}, min x: {bounds.min.x}");
         // Debug.Log($"max y: {bounds.max.y}, min y: {bounds.min.y}");
         foreach (var tm in tilemaps)
         {
             BoundsInt bounds = tm.cellBounds;
                

             for (int z = bounds.max.z; z > bounds.min.z; z--)
             {
                 for (int y = bounds.min.y; y < bounds.max.y; y++)
                 {
                     for (int x = bounds.min.x; x < bounds.max.x; x++)
                     {
                         var tileLocation = new Vector3Int(x, y, z);
                         var tileKey = new Vector2Int(x, y);
                         if (tm.HasTile(tileLocation - new Vector3Int(0, 0, 1)) && tm.gameObject
                                 .GetComponent<TilemapRenderer>().sortingLayerName.Equals("Ground"))
                         {
                             if (!map.ContainsKey(tileKey))
                             {
                                 var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                 var cellWorldPosition = tm.GetCellCenterWorld(tileLocation);
                                 overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y,
                                     cellWorldPosition.z + 1);
                                 overlayTile.GetComponent<SpriteRenderer>().sortingOrder =
                                     tm.GetComponent<TilemapRenderer>().sortingOrder;
                                 overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation =
                                     new Vector3Int(x, y, z);
                                 map.Add(tileKey, overlayTile.gameObject.GetComponent<OverlayTile>());
                             }
                         }
                     }
                 }
             }
         }

    }

    private Tilemap CreateMotherTileMap(List<Tilemap> tilemaps)
    {
         Tilemap motherTileMap = new Tilemap();
        // foreach (var tilemap in tilemaps)
        // {
        //     if (tilemap == null) 
        //         continue;
        //     
        //     BoundsInt bounds = tilemap.cellBounds;
        //     foreach (var tilePosition in bounds.allPositionsWithin)
        //     {
        //
        //         Vector3Int  localPosition = new Vector3Int(tilePosition.x, tilePosition.y, tilePosition.z);
        //         TileBase tile = tilemap.GetTile(localPosition);
        //         
        //     }
        // }

        if (tilemaps.Count == 0)
        {
            
        }

        Vector3Int max = new Vector3Int(int.MaxValue, int.MaxValue, 0);
        Vector3Int min = new Vector3Int(int.MinValue, int.MinValue, 0);

        foreach (var tm in tilemaps)
        {
            if (tm != null)
            {
                BoundsInt bounds = tm.cellBounds;

                min = Vector3Int.Min(min, bounds.min);
                max = Vector3Int.Min(max, bounds.max);
            }
        }

        BoundsInt combinedBounds = new BoundsInt(min, max - min);
        
        return motherTileMap;
    }

    public List<OverlayTile> GetRangeTiles(Vector2Int playerOccupiedTile)
    {
        var rangeTiles = new List<OverlayTile>();
        
        Vector2Int tileToCheck = new Vector2Int(playerOccupiedTile.x + 1, playerOccupiedTile.y);
        if (map.ContainsKey(tileToCheck))
        {
            rangeTiles.Add(map[tileToCheck]);
        }  
        
        tileToCheck = new Vector2Int(playerOccupiedTile.x, playerOccupiedTile.y + 1);
        if (map.ContainsKey(tileToCheck))
        {
            rangeTiles.Add(map[tileToCheck]);
        }  
        
        tileToCheck = new Vector2Int(playerOccupiedTile.x - 1, playerOccupiedTile.y);
        if (map.ContainsKey(tileToCheck))
        {
            rangeTiles.Add(map[tileToCheck]);
        }  
        
        tileToCheck = new Vector2Int(playerOccupiedTile.x, playerOccupiedTile.y - 1);
        if (map.ContainsKey(tileToCheck))
        {
            rangeTiles.Add(map[tileToCheck]);
        }

        return rangeTiles;
    }
}