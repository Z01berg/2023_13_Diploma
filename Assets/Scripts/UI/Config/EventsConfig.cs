using System;
using UnityEngine.Events;
using UI.Events;

namespace UI
{
    /**
     * Klasa ta przechowuje eventy, na które podpiete so do kart i invokowane we Wrapperze i HandContollerze
    */
    [Serializable]
    public class EventsConfig
    {
        public UnityEvent<CardPlayed> onCardPlay; //To jeszcze trzeba dorobić
        public UnityEvent<CardHover> cardHover;
        public UnityEvent<CardUnhover> cardUnHover;
        public UnityEvent<CardDestroy> cardDestroy; //To też
    }
}