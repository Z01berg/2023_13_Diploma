using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public CardsSO cardSO;
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text cost;
    public TMP_Text attack;
    public TMP_Text move;
    public Image BG;
    public Image banner;

    void Start()
    {
        //cardSO.Print();

        title.text = cardSO.title;
        description.text = cardSO.description;
        cost.text = cardSO.cost.ToString();
        attack.text = cardSO.damage.ToString();
        move.text = cardSO.move.ToString();

        BG.sprite = Resources.Load(cardSO.backgroundPath) as Sprite;
        banner.sprite = Resources.Load(cardSO.spritePath) as Sprite;
    }

    
}
