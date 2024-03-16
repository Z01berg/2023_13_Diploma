using System;
using UnityEngine.Events;
using UI.Events;

namespace UI
{
    [Serializable]
    public class EventsConfig
    {
        
        public UnityEvent<CardPlayed> onCardPlay;
        public UnityEvent<CardHover> cardHover;
        public UnityEvent<CardUnhover> cardUnhover;
        public UnityEvent<CardDestroy> cardDestroy;
    }
}