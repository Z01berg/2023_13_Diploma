using System;
using System.Collections.Generic;
using System.Linq;
using CardActions;
using Grid.New;
using Player;
using UI;
using UnityEngine;


public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public float speed = 2;
    public GameObject playerPrefab;

    private RangeFinder _rangeFinder;
    private PathFinder _pathFinder;
    public static List<OverlayTile> _rangeTiles;
    private List<OverlayTile> path;
    private bool _isMoving;

    private Vector2 _playerPosition;
    private PlayerController _playerController;
    private bool _canHideRange = false;


    private void Start()
    {
        _rangeFinder = new RangeFinder();
        _rangeTiles = new List<OverlayTile>();
        path = new List<OverlayTile>();
        _pathFinder = playerPrefab.GetComponent<PathFinder>();

        _playerController = playerPrefab.GetComponent<PlayerController>();
        EventSystem.ShowRange.AddListener(ShowRangeTiles);
        EventSystem.HideRange.AddListener(HideRangeTiles);
    }

    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            OverlayTile overlayTile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

            if (overlayTile == null)
            {
                return;
            }

            if (overlayTile.GetComponent<SpriteRenderer>() != null)
            {
                cursor.transform.position = overlayTile.transform.position;
                cursor.GetComponent<SpriteRenderer>().sortingOrder =
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder;


                if (Input.GetMouseButtonDown(0))
                {
                    overlayTile.ShowTile();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (_playerController.standingOnTile == null)
                    {
                        _playerController.standingOnTile = _playerController.GetCurrentTile();
                    }
                    if (_playerController != null)
                    {
                        _playerController.PlayerMouseMovement(overlayTile);
                    }
                    overlayTile.HideTile();
                }
            }

            // if (path.Count > 0)
            // {
            //     MoveAlongPath();
            // }
        }
    }

    // private void MoveAlongPath()
    // {
    //     var step = speed * Time.deltaTime;
    //     playerPrefab.transform.position =
    //         Vector2.MoveTowards(playerPrefab.transform.position, path[0].transform.position, step);
    //     if (Vector2.Distance(playerPrefab.transform.position, path[0].transform.position) < 0.0001f)
    //     {
    //         PositionCharacterOnTile(path[0]);
    //         path.RemoveAt(0);
    //     }
    // }

    private RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    // private void PositionCharacterOnTile(OverlayTile tile)
    // {
    //     var closestTile =
    //         playerPrefab.transform.position = new Vector3(tile.transform.position.x,
    //             tile.transform.position.y + 0.0001f, tile.transform.position.z);
    //     // playerPrefab.GetComponentInChildren<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
    //     _playerController.standingOnTile = tile;
    // }

    private void ShowRangeTiles(int range)
    {
        _rangeTiles.Clear();
        _rangeTiles = _rangeFinder.GetTilesInRange(
            range, _playerController.standingOnTile);
        foreach (var tile in _rangeTiles)
        {
            tile.ShowRangeTile();
        }

        if (_playerController.enabled)
        {
            _playerController.enabled = false;
        }
    }

    private void HideRangeTiles()
    {
        if (_rangeTiles.Count > 0)
        {
            foreach (var tile in _rangeTiles)
            {
                tile.HideRangeTile();
            }
        }

        if (_playerController != null)
        {
            if (!_playerController.enabled)
            {
                _playerController.enabled = true;
            }
        }
    }

    public Vector2 GetPlayerPosition()
    {
        var playerPos = playerPrefab.GetComponent<PlayerController>();
        return new Vector2(playerPos.standingOnTile.gridLocation.x, playerPos.standingOnTile.gridLocation.y);
    }

    // private void MoveTowardsThePlayer()
    // {
    //     _endedMove = false;
    //     
    //     _playerPosition = _player.position;
    //
    //     _currentPath = _pathfinding.FindPath(_playerPosition);
    //     if (_currentPath != null)
    //     {
    //         StartCoroutine(MoveAlongPath());
    //     }
    // }
}