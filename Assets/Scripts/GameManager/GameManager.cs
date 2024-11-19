using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grid.New;
using Player;
using UnityEngine;

/**
 * Publiczna klasa GameManager jest odpowiedzialna za zarządzanie grą.
 *
 * Ma możliwość:
 * - Zarządzania stanami gry.
 * - Uruchamiania poziomów.
 * - Przechowywania referencji do poziomów.
 * - Przechowywania referencji do gracza.
 * - Przechowywania referencji do punktu ruchu gracza.
 * - Przechowywania referencji do obiektu budującego poziom.
 */

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerMovePoint; //TODO: DELETE and fix this shit
    private CameraProperties _camera;
    
    // [SerializeField] private GameObject _dungeonBuilder; 
    private PlayerController _playerController;
    
    #region Header DUNGEON LEVELS

    [Space(10)]
    [Header("DUNGEON LEVELS")]

    #endregion Header DUNGEON LEVELS
    #region Tooltip

    [Tooltip("Populate with the dungeon level scriptable objects")]

    #endregion Tooltip
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip

    [Tooltip("Populate with the starting dungeon level for testing, first level = 0")]

    #endregion Tooltip
    [SerializeField] private int currenDungeonLevelListIndex = 0;

    private Room currentRoom;
    private Room previousRoom;
    //PlayerDeatailSO

    [HideInInspector] public GameState GameState;
    
    private void Start()
    {
        GameState = GameState.gameStarted;
        _playerController = _player.GetComponent<PlayerController>();
        _camera = FindObjectOfType<CameraProperties>();
        EventSystem.NewLevel.AddListener(ChangeGameState_NewLevel);
    }
    
    private void LateUpdate()
    {
        HandleGameStates();

        //TODO: For testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeGameState_NewLevel();
            _camera.virtualCamera.Follow = _player.transform;
        }
    }
    
    private void HandleGameStates()
    {
        switch (GameState)
        {
            case GameState.gameStarted:
                PlayDungeonLevel(currenDungeonLevelListIndex);
                
                EventSystem.InitInv.Invoke();
                GameState = GameState.playingLevel;
                break;
            
            case GameState.bossStage:
                if (currenDungeonLevelListIndex < dungeonLevelList.Count - 1)
                {
                    ChangeLevel();
                    PlayDungeonLevel(currenDungeonLevelListIndex);
                    EventSystem.ZeroTimer.Invoke();
                    GameState = GameState.playingLevel;
                }
                
                if (currenDungeonLevelListIndex == dungeonLevelList.Count - 1)
                {
                    GameState = GameState.gameStarted;
                }
                break;
            
            case GameState.gameLost:
                //TODO: Handle game lost Maybe Highscore
                break;
        }
    }

    private async Task PlayDungeonLevel(int dungeonLevelListIndex)
    {
        EventSystem.NewLevel.Invoke();
        bool dungeonBuiltSucessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSucessfully)
        {
         Debug.LogError("Couldn't build dungeon from specified rooms and more graphs");   
        }

        var spawnPosition = HelperUtilities.GetSpawnPositionNearestToPlayer(_player.transform.position);

        _player.transform.position =
            new Vector3((currentRoom.LowerBounds.x + currentRoom.UpperBounds.x) / 2f,
                (currentRoom.LowerBounds.y + currentRoom.UpperBounds.y) / 2f, 0f);

        _player.transform.position = spawnPosition;

        _playerMovePoint.transform.position = _player.transform.position;
        var spawnTile = GetSpawnOverlayTile(spawnPosition);
        _playerController.standingOnTile = spawnTile;
    }

    private void ChangeLevel()
    {
        currenDungeonLevelListIndex++;
        SaveSystem.SaveGame();
    } 
    
    private void ChangeGameState_NewLevel()
    {
        GameState = GameState.bossStage;
    }
    
    public OverlayTile GetSpawnOverlayTile(Vector3 spawnPosition)
    {
        RaycastHit2D? hit = GetFocusedOnTile(spawnPosition);

        if (hit.HasValue)
        {
            OverlayTile overlayTile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

            return overlayTile;
        }
        return null;
    }

    public RaycastHit2D? GetFocusedOnTile(Vector3 spawnPosition)
    {
        Vector2 spawnPosition2d = new Vector2(spawnPosition.x, spawnPosition.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(spawnPosition2d, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    #region Validation
    #if UNITY_EDITOR
    
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
        }
    #endif
    #endregion Validation

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;
    }
    
}
