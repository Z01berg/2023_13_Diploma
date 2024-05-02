using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
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

    [HideInInspector] public GameState GameState;
    
    private void Start()
    {
        GameState = GameState.gameStarted;
    }
    
    private void Update()
    {
        HandleGameStates();

        // For testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameState = GameState.gameStarted;
        }
    }

    // Handle game state
    private void HandleGameStates()
    {
        switch (GameState)
        {
            case GameState.gameStarted:
                // Play first Level
                PlayDungeonLevel(currenDungeonLevelListIndex);
                
                GameState = GameState.playingLevel;
                break;
        }
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        throw new System.NotImplementedException();
    }

    #region Validation
    #if UNITY_EDITOR
    
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
        }
    #endif
    #endregion Validation
}
