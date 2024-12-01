using System.Collections.Generic;

/**
 * File Enums.cs zawiera w sobie wszystkie enumy z całego projektu
 */

// Wykorzystuje się przy korytażasz
public enum Orientation
{
   north,
   east,
   south,
   west,
   none
}

// Wykorzystuje się przy tworzeniu Timerów
public enum CharacterType
{
   Player,
   Enemy,
   Item,
   Boss,
   None
}
//TODO: Przebudować to bardziej orientowaną do naszego produktu
// Wykorzystuje się przy zarządzaniu stanami gry
public enum GameState
{
    gameStarted,
    playingLevel,
    engagingEnemies,
    bossStage,
    engagingBoss,
    levelCompleted,
    gameWon,
    gameLost,
    gamePaused,
    dngeonOverviewMap,
    restartGame
}

// Wykorzystywane do oznaczania typu karty
public enum CardType
{
    Attack,
    Defense,
    Movement,
    Curse
}

// Wykorzystywane do oznaczenia animacji onHit
public enum HitAnimation
{
    Arrow,
    Blood,
    Boom,
    Fire_Meteor,
    Electric,
    Physical
}
