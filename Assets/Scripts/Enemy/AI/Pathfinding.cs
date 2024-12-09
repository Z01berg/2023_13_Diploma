using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap topTilemap;

    private Dictionary<Vector3Int, Node> _nodes = new Dictionary<Vector3Int, Node>();
    private HashSet<Vector3Int> _busyTilesSet = new HashSet<Vector3Int>();

    private List<Vector3> _currentPath;


    /*
    private void Start()
    {
        CreateNodes();
    }
    */
    private class Node
    {
        public Vector3 worldPosition;
        public Vector3Int gridPosition;
        public Node parent;
        public float gCost;
        public float hCost;

        public float FCost => gCost + hCost;

        public Node(Vector3 worldPosition, Vector3Int gridPosition)
        {
            this.worldPosition = worldPosition;
            this.gridPosition = gridPosition;
        }
    }


    private void CreateNodesForEnemies()
    {
        _nodes.Clear();
        _busyTilesSet.Clear();
        
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;
            Vector3Int tilePosition = groundTilemap.WorldToCell(enemyPosition);
            _busyTilesSet.Add(tilePosition);
        }

        foreach (var pos in groundTilemap.cellBounds.allPositionsWithin)
        {
            var worldPos = groundTilemap.GetCellCenterWorld(pos);
            bool isTileBusy = _busyTilesSet.Contains(pos);
            if (groundTilemap.HasTile(pos) && !topTilemap.HasTile(pos) && !isTileBusy)
            {
                _nodes[pos] = new Node(worldPos, pos);
            }
        }
        // Debug.Log($"Created {_nodes.Count} nodes");
    }

    public List<Vector3> FindPath(Vector3 targetPosition)
    {
        if (groundTilemap == null || topTilemap == null)
        {
            // Debug.LogWarning("Ground or top tilemap not set.");
            return null;
        }

        
        CreateNodesForEnemies();
        

        var startCell = groundTilemap.WorldToCell(transform.position);
        var targetCell = groundTilemap.WorldToCell(targetPosition);

        Vector3 thisObjectWorldPosition = transform.position;
        
        _nodes.Add(
            groundTilemap.WorldToCell(thisObjectWorldPosition), 
            new Node(thisObjectWorldPosition, groundTilemap.WorldToCell(thisObjectWorldPosition))
            );

        if (!_nodes.ContainsKey(startCell) || !_nodes.ContainsKey(targetCell))
        {
            // Debug.LogWarning("Start or target not selected");
            return null;
        }

        var openNodeList = new List<Node>();
        var closedNodeSet = new HashSet<Node>();
        openNodeList.Add(_nodes[startCell]);

        while (openNodeList.Count > 0)
        {
            var currentNode = openNodeList[0];
            for (int i = 1; i < openNodeList.Count; i++)
            {
                if (openNodeList[i].FCost < currentNode.FCost ||
                    openNodeList[i].FCost == currentNode.FCost &&
                    openNodeList[i].hCost < currentNode.hCost)
                {
                    currentNode = openNodeList[i];
                }
            }

            openNodeList.Remove(currentNode);
            closedNodeSet.Add(currentNode);

            if (currentNode == _nodes[targetCell])
            {
                return RetracePath(_nodes[startCell], _nodes[targetCell]);
            }

            foreach (var neighbor in GetNeighbors(currentNode))
            {
                if (closedNodeSet.Contains(neighbor) || neighbor == null)
                {
                    continue;
                }

                var newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openNodeList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, _nodes[targetCell]);
                    neighbor.parent = currentNode;

                    if (!openNodeList.Contains(neighbor))
                    {
                        openNodeList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }


    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Vector3>();
        var currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbors(Node node)
    {
        var neighbors = new List<Node>();
        var directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        foreach (var dir in directions)
        {
            var neighborPosition = node.gridPosition + dir;
            if (_nodes.ContainsKey(neighborPosition) && _nodes[neighborPosition] != null)
            {
                neighbors.Add(_nodes[neighborPosition]);
            }
        }

        return neighbors;
    }

    private float GetDistance(Node nodeA, Node nodeB)
    {
        var distanceX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        var distanceY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        return distanceX + distanceY;
    }

    /*
    private bool IsGroundTileBusy(Vector3Int tilePosition)
    {
        _busyTilesSet.Clear();

        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            Vector3 
        }
        
        Vector3 worldTileCenter = groundTilemap.GetCellCenterWorld(tilePosition);
        Collider2D[] colliders2D = Physics2D.OverlapPointAll(worldTileCenter);

        foreach (Collider2D collider in colliders2D)
        {
            if (collider.GetComponentInParent<EnemyController>() != null)
            {
                return true;
            }
        }
        return false;
    }
    */
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (var node in _nodes.Values)
        {
            if (node != null)
            {
                Gizmos.DrawWireSphere(node.worldPosition, 0.1f);
            }
        }
    }
    
    // public void ChaseTarget(Vector3 targetPosition)
    // {
    //     Vector3 startPosition = transform.position;
    //
    //     _currentPath = FindPath(targetPosition);
    //     _currentWaypointIndex = 0;
    // }
}


