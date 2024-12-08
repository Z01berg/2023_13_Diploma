using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grid.New
{
    public class PathFinder : MonoBehaviour
    {
        private Dictionary<Vector2, OverlayTile> searchableTiles;

        public List<OverlayTile> FindPathOutsideOfCombat(OverlayTile start, OverlayTile end)
        {
            List<OverlayTile> possibleNodes = new List<OverlayTile>();
            List<OverlayTile> chosenNodes = new List<OverlayTile>();

            possibleNodes.Add(start);

            while (possibleNodes.Count > 0)
            {
                OverlayTile currentOverlayTile = possibleNodes.OrderBy(x => x.F).First();

                possibleNodes.Remove(currentOverlayTile);
                chosenNodes.Add(currentOverlayTile);

                if (currentOverlayTile == end)
                {
                    return GetFinishedList(start, end);
                }

                foreach (var tile in GetNeightbourOverlayTiles(currentOverlayTile))
                {
                    if (chosenNodes.Contains(tile) || Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                    {
                        continue;
                    }

                    tile.G = GetManhattenDistance(start, tile);
                    tile.H = GetManhattenDistance(end, tile);

                    tile.Previous = currentOverlayTile;
                    
                    if (!possibleNodes.Contains(tile))
                    {
                        possibleNodes.Add(tile);
                    }
                }
            }

            return new List<OverlayTile>();
        }
        
        public List<OverlayTile> FindPathInCombat(OverlayTile start, OverlayTile end, List<OverlayTile> inRangeTiles)
        {
            if (!inRangeTiles.Contains(end))
            {
                return new List<OverlayTile>();
            }
            searchableTiles = new Dictionary<Vector2, OverlayTile>();

            List<OverlayTile> possibleNodes = new List<OverlayTile>();
            HashSet<OverlayTile> chosenNodes = new HashSet<OverlayTile>();

            if (inRangeTiles.Count > 0)
            {
                foreach (var item in inRangeTiles)
                {
                    searchableTiles.Add(item.grid2DLocation, OverlayManager.Instance.map[item.grid2DLocation]);
                }
            }

            possibleNodes.Add(start);

            while (possibleNodes.Count > 0)
            {
                OverlayTile currentOverlayTile = possibleNodes.OrderBy(x => x.F).First();

                possibleNodes.Remove(currentOverlayTile);
                chosenNodes.Add(currentOverlayTile);

                if (currentOverlayTile == end)
                {
                    return GetFinishedList(start, end);
                }

                foreach (var tile in GetNeightbourOverlayTiles(currentOverlayTile))
                {
                    if (chosenNodes.Contains(tile) || Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                    {
                        continue;
                    }

                    tile.G = GetManhattenDistance(start, tile);
                    tile.H = GetManhattenDistance(end, tile);

                    tile.Previous = currentOverlayTile;


                    if (!possibleNodes.Contains(tile))
                    {
                        possibleNodes.Add(tile);
                    }
                }
            }

            return new List<OverlayTile>();
        }

        private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
        {
            List<OverlayTile> finishedList = new List<OverlayTile>();
            OverlayTile currentTile = end;

            while (currentTile != start)
            {
                finishedList.Add(currentTile);
                currentTile = currentTile.Previous;
            }

            finishedList.Reverse();

            return finishedList;
        }

        private float GetManhattenDistance(OverlayTile start, OverlayTile tile)
        {
            return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
        }

        private List<OverlayTile> GetNeightbourOverlayTiles(OverlayTile currentOverlayTile)
        {
            var map = OverlayManager.Instance.map;

            List<OverlayTile> neighbours = new List<OverlayTile>();

            Vector2 tileToCheck = new Vector2((float)Math.Round(currentOverlayTile.gridLocation.x + 1,3), (float)Math.Round(currentOverlayTile.gridLocation.y,3));

            if (map.ContainsKey(tileToCheck))
            {
                neighbours.Add(map[tileToCheck]);
            }

            tileToCheck = new Vector2((float)Math.Round(currentOverlayTile.gridLocation.x - 1,3), (float)Math.Round(currentOverlayTile.gridLocation.y,3));

            if (map.ContainsKey(tileToCheck))
            {
                neighbours.Add(map[tileToCheck]);
            }

            tileToCheck = new Vector2((float)Math.Round(currentOverlayTile.gridLocation.x ,3), (float)Math.Round(currentOverlayTile.gridLocation.y + 1,3));

            if (map.ContainsKey(tileToCheck))
            {
                neighbours.Add(map[tileToCheck]);
            }

            tileToCheck = new Vector2((float)Math.Round(currentOverlayTile.gridLocation.x ,3), (float)Math.Round(currentOverlayTile.gridLocation.y - 1,3));

            if (map.ContainsKey(tileToCheck))
            {
                neighbours.Add(map[tileToCheck]);
            }

            return neighbours.Distinct().ToList();
        }
    }
}