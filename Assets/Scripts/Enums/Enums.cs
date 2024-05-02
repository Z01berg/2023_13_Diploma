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
