using System.Collections.Generic;

[System.Serializable]

public struct JsonEnemy
{
        public List<Enemy> EnemyList;
}

[System.Serializable]
public struct Enemy
{
        public int id;
        public string title;
        public int max_HP;
        public int damage;
        public int move;
        public int startTimer;
        public int startDefence;
        public string spritePath;
}
