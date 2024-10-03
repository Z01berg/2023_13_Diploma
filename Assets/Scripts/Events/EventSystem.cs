using Player;
using UnityEngine;
using UnityEngine.Events;

/**
 * Publiczna clasa EventSystem jest wykorzystywana dla eventów jako punkt lączący ze wszystkich class.
 * 
 * Żeby rozdzielić eventy wykorzystujemy regiony z opisem Nazwa: Od Kogo -> Do Kogo
 */

public static class EventSystem
{
    #region PlayerMove: Timer.cs -> PlayerController.cs

        public static UnityEvent<bool> PlayerMove = new UnityEvent<bool>();
    
    #endregion
    
    //TODO Find if somebody use this
    #region EnemyMove: Timer.cs -> EnemyControllerTests.cs

        public static UnityEvent<bool, Vector3> EnemyMove = new UnityEvent<bool,Vector3>();
    
    #endregion
    
    #region MovePlayer: PlayerInputsController.cs -> EnemyCotrollerTests.cs

    public static UnityEvent MoveEnemy = new UnityEvent();

    #endregion
    
    #region MovePlayer: EnemyCotrollerTests.cs -> Timer.cs

    public static UnityEvent<int> FinishEnemyTurn = new UnityEvent<int>();

    #endregion
    
    #region Delete Reference: HealthBar.cs -> TimerData.cs

    public static UnityEvent<int> DeleteReference = new UnityEvent<int>();
    
    #endregion
    
    #region WhatHP: Timer.cs -> HealthBar.cs

        public static UnityEvent<GameObject, int> WhatHP = new UnityEvent<GameObject, int>();
    
    #endregion

    #region DestroyCard: ReceiveAttack.cs -> HandController

    public static UnityEvent DestroyCard = new UnityEvent();

    #endregion

    #region ShowCheatEngine: Timer.cs -> UIScrypt

    public static UnityEvent ShowCheatEngine = new UnityEvent();

    #endregion
    
    #region ShowCheatEngine: Timer.cs -> UIScrypt

    public static UnityEvent ShowHelpSheet = new UnityEvent();

    #endregion

    #region DisableHand: PauseMenuManager.cs -> HandController.cs

    public static UnityEvent<bool> HideHand = new UnityEvent<bool>();

    #endregion

    #region Open/CloseInventory: PlayerInputController.cs -> InventoryActions

    public static UnityEvent OpenCloseInventory = new UnityEvent();

    #endregion

    #region Open/ClosePauseMenu: PlayerInputsController.cs -> PauseMenuManager

    public static UnityEvent OpenClosePauseMenu = new UnityEvent();

    #endregion

    #region MovePlayer: PlayerInputsController.cs -> PlayerController

    public static UnityEvent<Vector2> MovePlayer = new UnityEvent<Vector2>();

    #endregion
    
    
    #region InstatiatedRoom: InstatiatedRoom.cs -> Timer.cs

    public static UnityEvent InstatiatedRoom = new UnityEvent();

    #endregion
    
    #region ChangeHealthPlayer: EnemyController.cs -> PlayerController.cs

    public static UnityEvent<int> ChangeHealthPlayer = new UnityEvent<int>();

    #endregion
    
    #region MovePlayer: PlayerInputsController.cs -> HistoryConroller.cs

    public static UnityEvent<CardsSO> LogAction = new UnityEvent<CardsSO>();

    #endregion

    #region MovePlayer: PlayerInputsController.cs -> HistoryConroller.cs

    public static UnityEvent<int> AssignTimerIndex = new UnityEvent<int>();

    #endregion
    
    #region DrawACard: InstanciateRoom.cs,  -> deckController.cs

    public static UnityEvent DrawACard = new UnityEvent();

    #endregion
    
    #region DrawACard: wrapper.cs -> MouseController.cs

    public static UnityEvent ShowRange = new UnityEvent();

    #endregion

    #region Event Skip Text: ShowCase.cs -> TypeWriterEffect.cs
    
    public static UnityEvent SkipText = new UnityEvent();
    
    #endregion
    
    #region Event Not Skipable Text : TypeWriterEffect.cs -> ShowCase.cs 
    
    public static UnityEvent<bool> SkipedText = new UnityEvent<bool>();
    
    #endregion
    
}