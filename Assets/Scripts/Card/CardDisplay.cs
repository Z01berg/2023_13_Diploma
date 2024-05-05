using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Publiczna klasa w ktorej zapisane sa dane pobrane z przypisanej do karty ScriptableObject
 * Dane te sa nastepnie wyswietlane w odpowiednim dla nich miejscu w momencie powstania karty.
 * 
 * Zmienne zawarte w klasie odpowiadaja danym w kazdej z kart.
 */

public class CardDisplay : MonoBehaviour
{
    public int id;
    public CardsSO cardSO;
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text cost;
    public TMP_Text attack;
    public TMP_Text move;
    public TMP_Text range;
    public Image BG;
    public Image banner;

    void Start()
    {
        //cardSO.Print();
        id = cardSO.id;
        title.text = cardSO.title;
        description.text = cardSO.description;
        cost.text = cardSO.cost.ToString();
        attack.text = cardSO.damage.ToString();
        move.text = cardSO.move.ToString();
        range.text = cardSO.range.ToString();

        BG.sprite = Resources.Load<Sprite>(cardSO.backgroundPath);
        banner.sprite = Resources.Load<Sprite>(cardSO.spritePath);
    }

    
}
