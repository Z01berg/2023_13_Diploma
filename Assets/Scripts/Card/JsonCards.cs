using System.Collections.Generic;

[System.Serializable]
public struct JsonCards
{
    public List<Card> attackCardsList;
    //public int id;
    //public bool isActive;
    //public int cardQuality;
    //public string title;
    //public string description;
    //public int cost;
    //public int damage;
    //public int move;
    //public string backgroundPath;
    //public string spritePath;
}

[System.Serializable]
public struct Card
{
    public int id;
    public bool isActive;
    public int cardQuality;
    public string title;
    public string description;
    public int cost;
    public int damage;
    public int move;
    public string backgroundPath;
    public string spritePath;
}