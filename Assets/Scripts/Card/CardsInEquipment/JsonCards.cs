using System.Collections.Generic;

/**
 * Klasa posrednia przechowujaca dane w formacie deserializowanego pliku Json, z ktorych
 * pozniej tworzone sa karty w grze.
 */

[System.Serializable]
public struct JsonCards
{
    public List<Card> attackCardsList;
    public List<Card> defenceCardsList;
    public List<Card> curseCardList;
    public List<Card> movementCardList;
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
    public int range;
}