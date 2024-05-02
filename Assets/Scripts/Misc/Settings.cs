using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Publiczna classa Settings
 * jest używana tylko dla definiowania publicznych Const znaczeń.
 * Dla grupowania znaczeń z różnych class używamy #region
 */

public static class Settings
{
    #region DUNGEON BUILD SETTINGS
        public const int MAX_DUNGEON_REBUILD_ATTEMPTS_FOR_ROOM_GRAPH = 1000;
        public const int MAX_DUNGEON_BUILD_ATTEMPTS = 10;
    #endregion DUNGEON BUILD SETTINGS
    
    #region ROOM SETTINGS

    public const int MAXCHILDCORRIDORS = 3;

    #endregion
}
