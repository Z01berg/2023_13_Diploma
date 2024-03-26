using UnityEditor;
using UnityEngine;

/**
 * Publiczna klasa s³uzaca do twozenia kart i zapisywania w odpowiednim pliku JSON.
 * Klasa posiada zmienne odpowiadaj¹ce atrybutom kart.
 * Po wpisaniu odpowiednich danych i przyciœniêciu "Export" w inspektorze objektu JsonCardCreator
 * uruchamiana jest publiczna funkcja CreateCard() serializuj¹ca dane w formacie Json i zapisuj¹ca do odpowiedniego pliku.
 * Funkcja tworzy równierz Scriptable object aby nie trzeba by³o importowaæ ponownie wszystkich kart.
 */

public enum CardType
{
    attack,movement,curse
}

public class CardCreator : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    private JsonCards objects;

    public CardType cardType;

    public bool isActive = true;
    public string title;
    [TextArea]
    public string description;
    public int cost;
    public int damage;
    public int move;
    public int range;
    public int cardQuality;
    public Sprite background;
    public Sprite sprite;
    

    public void CreateCard()
    {

        objects = JsonUtility.FromJson<JsonCards>(jsonFile.text);

        Card card = new Card();
        card.isActive = isActive;
        card.title = title;
        card.description = description;
        card.cost = cost;
        card.damage = damage;
        card.move = move;
        card.backgroundPath = AssetDatabase.GetAssetPath(background);
        card.range = range;
        card.spritePath = AssetDatabase.GetAssetPath(sprite);
        card.cardQuality = cardQuality;
        card.backgroundPath = card.backgroundPath.Substring(card.backgroundPath.IndexOf("Graphics"));
        card.spritePath = card.spritePath.Substring(card.spritePath.IndexOf("Graphics"));

        card.id = objects.attackCardsList[objects.attackCardsList.Count - 1].id + 1;

        objects.attackCardsList.Add(card);

        string json = JsonUtility.ToJson(objects, true);
        System.IO.File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), json);
    }
}
