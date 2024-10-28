using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grid.New
{
    public class RangeFinder
    {
        public List<OverlayTile> GetTilesInRange(int range, OverlayTile playersTile)
        {
            // var StartingTile = OverlayManager.Instance.map[location];
            var rangeTiles = new List<OverlayTile>();

            int stepCount = 0;
            var tilesForPreviousStep = new List<OverlayTile> { playersTile };

            rangeTiles.Add(playersTile);
            
            while (stepCount < range)
            {
                var surroundingTiles = new List<OverlayTile>();

                foreach (var tile in tilesForPreviousStep)
                {
                    surroundingTiles.AddRange(OverlayManager.Instance.GetRangeTiles(new Vector2(tile.gridLocation.x, tile.gridLocation.y)));
                }
                
                rangeTiles.AddRange(surroundingTiles);
                tilesForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }
            

            return rangeTiles.Distinct().ToList();
        }
    }
}