using System.Collections.Generic;
using System.Linq;
using Grid.New;
using UnityEngine;

namespace CardActions
{
    public class RangeFinder
    {
        public List<OverlayTile> GetTilesInRange(Vector2 location, int range)
        {
            var StartingTile = OverlayManager.Instance.map[location];
            var RangeTiles = new List<OverlayTile>();

            int stepCount = 0;

            var tilesForPreviousStep = new List<OverlayTile>();
            tilesForPreviousStep.Add(StartingTile);

            while (stepCount < range)
            {
                var surroundingTiles = new List<OverlayTile>();

                foreach (var item in tilesForPreviousStep)
                {
                    surroundingTiles.AddRange(OverlayManager.Instance.GetRangeTiles(new Vector2(item.gridLocation.x, item.gridLocation.y)));
                }
                
                RangeTiles.AddRange(surroundingTiles);
                tilesForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }

            return RangeTiles.Distinct().ToList();
        }
    }
}