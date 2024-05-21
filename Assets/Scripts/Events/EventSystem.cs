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
    #region EnemyMove: Timer.cs -> .cs

        public static UnityEvent<bool> EnemyMove = new UnityEvent<bool>();
    
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
    
    #region MovePlayer: PlayerInputsController.cs -> HistoryConroller.cs

    public static UnityEvent<CardsSO> LogAction = new UnityEvent<CardsSO>();

    #endregion
}