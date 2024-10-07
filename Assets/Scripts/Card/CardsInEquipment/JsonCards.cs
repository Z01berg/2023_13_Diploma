using System.Collections.Generic;

/**
 * Klasa posrednia przechowujaca dane w formacie deserializowanego pliku Json, z ktorych
 * pozniej tworzone sa karty w grze.
 */

[System.Serializable]
public struct JsonAttackCards
{
    public List<Card> attackCardsList;
}
[System.Serializable]
public struct JsonDefenceCards
{
    public List<Card> defenceCardsList;
}
[System.Serializable]
public struct JsonMovementCards
{
    public List<Card> movementCardsList;
}
[System.Serializable]
public struct JsonCurseCards
{
    public List<Card> curseCardsList;
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
    public int range;
    public int move;

}