using TMPro;
using UnityEngine;

namespace UI
{
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
            if (PlaceHolder._isTaken)
            {
                _card = Wrapper.cardInUse;
                ReadCard(_card.GetComponent<CardDisplay>());
            }
        }

        public void ReadCard(CardDisplay display)
        {
            _text.text = $"ID: {display.id.ToString()}\n" +
                         $"Cost: {display.cost.text}\n" +
                         $"Attack: {display.attack.text}\n" +
                         $"Move: {display.move.text}";
        }
    }
    
}