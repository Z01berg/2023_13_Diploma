using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public int id;
    public CardsSO cardSO;
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text cost;
    public TMP_Text attack;
    public TMP_Text move;
    public int range;
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
        range = cardSO.range;

        BG.sprite = Resources.Load<Sprite>(cardSO.backgroundPath);
        banner.sprite = Resources.Load<Sprite>(cardSO.spritePath);
    }

    
}
