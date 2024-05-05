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

}