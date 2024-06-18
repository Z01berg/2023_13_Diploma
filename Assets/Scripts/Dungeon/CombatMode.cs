using UnityEngine;

namespace Dungeon
{
    public static class CombatMode
    {
        public static bool isPlayerInCombat;

        public static void SetFalse()
        {
            isPlayerInCombat = false;
        }
        public static void SetTrue()
        {
            isPlayerInCombat = false;
        }
        
        public static bool GetIsPlayerInCombat()
        {
            return isPlayerInCombat;
        }
        
        
    }
    
    
}