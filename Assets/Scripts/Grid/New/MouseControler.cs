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

    public GameObject playerPrefab;

    private RangeFinder _rangeFinder;
    public static List<OverlayTile> _rangeTiles;
    private bool _isMoving;

    private Vector2 _playerPosition;
    private PlayerController _playerController;
    private bool _canHideRange = false;



    private void Start()
    {
        _rangeFinder = new RangeFinder();
        _rangeTiles = new List<OverlayTile>();
        _playerController = playerPrefab.GetComponent<PlayerController>();
        EventSystem.ShowRange.AddListener(ShowRangeTiles);
        EventSystem.HideRange.AddListener(HideRangeTiles);
    }


    private void Update()
    {
        // if (Wrapper.cardInUse)
        // {
        //     String range = Wrapper.cardInUse.GetComponent<CardDisplay>().range.text;
        //     GetRangeTiles(int.Parse(range));
        //     _canHideRange = true;
        //     if (_playerController.enabled)
        //     {
        //         _playerController.enabled = false;
        //     }
        // }
        // else if (_canHideRange && !Wrapper.cardInUse)
        // {
        //     if (!_playerController.enabled)
        //     {
        //         _playerController.enabled = true;
        //         foreach (var tile in _rangeTiles)
        //         {
        //             tile.HideTile();
        //         }
        //     }
        // }
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
                    overlayTile.gameObject.GetComponent<OverlayTile>().HideTile();
                }
            }
        }
    }

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

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        var closestTile =
            playerPrefab.transform.position = new Vector3(tile.transform.position.x,
                tile.transform.position.y + 0.0001f, tile.transform.position.z);
        playerPrefab.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        _playerController.standingOnTile = tile;
    }

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
        if (!_playerController.enabled)
        {
            _playerController.enabled = true;
        }
    }

    public Vector2 GetPlayerPosition()
    {
        var playerPos = playerPrefab.GetComponent<PlayerController>();
        return new Vector2(playerPos.standingOnTile.gridLocation.x, playerPos.standingOnTile.gridLocation.y);
    }
}