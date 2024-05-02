using System;
using UI;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace CardActions
{
    public class CardsEffectsManager : MonoBehaviour
    {
        private Wrapper _card;
        private Attack _attack;
        
        private void Update()
        {
            if (PlaceHolder.isTaken)
            {
                if (_card == Wrapper.cardInUse)
                {
                    return;
                }
                _card = Wrapper.cardInUse;
            }
        }
        
        public CardsSO GetCardsSo()
        {
            var cardInfo = Wrapper.cardInUse.GetComponent<CardDisplay>().cardSO;
            return cardInfo;
        }

        
    }
}