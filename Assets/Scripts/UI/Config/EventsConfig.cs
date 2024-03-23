using System;
using UnityEngine.Events;
using UI.Events;

namespace UI
{
    [Serializable]
    public class EventsConfig
    {
        
        public UnityEvent<CardPlayed> onCardPlay; //To jeszcze trzeba dorobić
        public UnityEvent<CardHover> cardHover;
        public UnityEvent<CardUnhover> cardUnHover;
        public UnityEvent<CardDestroy> cardDestroy; //To też
    }
}