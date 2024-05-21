using TMPro;
using UnityEngine;

namespace UI
{
    /**
     * Publiczna klasa CardReader, która odpowiada za sczytywanie informacji a ktualnie wybranej karty
     */
    public class CardReader : MonoBehaviour
    {
        private int _id;
        private int _attack;
        private int _move;
        private int _cost;

        private TextMeshProUGUI _text;
        private Wrapper _card;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (PlaceHolder.isTaken)
            {
                _card = Wrapper.cardInUse;
                ReadCard(_card.GetComponent<CardDisplay>());
            }
            else
            {
                _text.text = "ID:\n" +
                             "Cost:\n" +
                             "Attack:\n" +
                             "Move:\n" +
                             "Range:";
            }
        }

        private void ReadCard(CardDisplay display)
        {
            _text.text = $"ID: {display.id.ToString()}\n" +
                         $"Cost: {display.cost.text}\n" +
                         $"Attack: {display.attack.text}\n" +
                         $"Move: {display.move.text}\n" +
                         $"Range: {display.range.text}";
        }
    }
}