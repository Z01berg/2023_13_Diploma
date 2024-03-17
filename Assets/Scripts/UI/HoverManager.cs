using System.Collections.Generic;
using System.Diagnostics;
using UI.Events;
using UnityEngine;

namespace UI
{
    public class HoverManager : MonoBehaviour
    {
        [SerializeField] private float verticalPosition;
        [SerializeField] private float hoverScale = 1f;
        [SerializeField] private int hoverCardOrder = 100;

        private Dictionary<Wrapper, Trace> previews = new();

        public void OnCardHover(CardHover cardHover)
        {
            CardZoom(cardHover.card);
        }

        public void CardZoom(Wrapper wrapper)
        {
           
        }
        
    }
}