using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{   
    /**
     * Publiczna klasa HandController jest klasa która odpowiada za ustawienie kart w ręce.
     *
     * Przypisuje wszystkim obiektom, które są dziecmi obiektu hand: Canvas, GraphicRaycaster, Wrapper w metodzie SetUpCards();
     * Przypisuje każdemu wrapperowi Configi, w których są informacja związane z eventami
     * 
     * 
     * Ustawia karty w wylicza pozycje kart, ustawia w odpowiednim miejscu, nadaje im odpowednią warstwę.
     * Dodaje każdej karcie Canvas, Wrapper, GraphicRaycaster (pomaga działać Canvasowi)
    */
    public class HandController : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private List<Wrapper> _cards = new(); 
    
        [SerializeField] private ZoomConfig zoomConfig;
        [Header("Constraints")]
        [SerializeField] private bool forceFitContainer; //Decyduje czy karty się na siebie nakładają lub nie
        [Header("Alignment")]
        [SerializeField] private CardAlignment _alignment = CardAlignment.Center; 

        private Wrapper _currDraggedCard;
    
        [SerializeField] private EventsConfig eventsConfig;
        [SerializeField] public GameObject placeHolder;
    
        private RectTransform _placeHolderPosition;
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _placeHolderPosition = placeHolder.GetComponent<RectTransform>();
            InitCards();
        }

        private void InitCards()
        {
            SetUpCards();
            SetCardsAnchor();
        }

        private void Update()
        {
            UpdateCards();
        }
        //
        private void SetUpCards()
        {
            _cards.Clear();
            foreach (Transform card in transform)
            {
                var wrapper = card.GetComponent<Wrapper>();
                if (wrapper == null)
                {
                    wrapper = card.gameObject.AddComponent<Wrapper>();
                }
            
                _cards.Add(wrapper);
            
                AddOtherComponentsIfNeeded(wrapper);

                wrapper.zoomConfig = zoomConfig;
                wrapper.handController = this;
                wrapper.eventsConfig = eventsConfig;
            }
        }

        private void AddOtherComponentsIfNeeded(Wrapper wrapper)
        {
            var canvas = wrapper.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = wrapper.gameObject.AddComponent<Canvas>();
            }

            canvas.overrideSorting = true;
            if (wrapper.GetComponent<GraphicRaycaster>() == null)
            {
                wrapper.gameObject.AddComponent<GraphicRaycaster>();
            }
        }
    
        private void SetCardsAnchor() {
            foreach (Wrapper card in _cards) {
                card.SetAnchor(new Vector2(0, 0.5f), new Vector2(0, 0.5f));
            }
        }
    
        private void UpdateCards() {
            if (transform.childCount != _cards.Count) {
                InitCards();
            }

            if (_cards.Count == 0) {
                return;
            }

            SetCardsPosition();
            // SetCardsRotation(); 
            SetCardsUILayers();
            UpdateCardOrder();
        }

        private void UpdateCardOrder()
        {
            if (_currDraggedCard == null)
            {
                return;
            }
        
            // Get the index of the dragged card depending on its position
            var newCardIdx = _cards.Count(card => _currDraggedCard.transform.position.x > card.transform.position.x);
            var originalCardIdx = _cards.IndexOf(_currDraggedCard);
            _currDraggedCard.transform.SetSiblingIndex(newCardIdx);
            if (newCardIdx != originalCardIdx)
            {
                _cards.RemoveAt(originalCardIdx);
                if (newCardIdx > originalCardIdx && newCardIdx < _cards.Count -1)
                {
                    newCardIdx--;
                }
            }
            // Also reorder in the hierarchy
            _currDraggedCard.transform.SetSiblingIndex(newCardIdx);
        }

        private void SetCardsUILayers()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].uiLayer = zoomConfig.defaultSortOrder + i;
            }
        }

        private void SetCardsPosition() {
            // Oblicza całkowitą szerokość wszystkich kart
            var cardsTotalWidth = _cards.Sum(card => card.Width * card.transform.lossyScale.x);
            // Szerokość kontenera (hand)
            var containerWidth = _rectTransform.rect.width * transform.lossyScale.x;
            if (forceFitContainer && cardsTotalWidth > containerWidth) {
                DistributeChildrenToFitContainer(cardsTotalWidth);
            }
            else {
                DistributeChildrenWithoutOverlap(cardsTotalWidth);
            }
        }

        private void DistributeChildrenToFitContainer(float cardsTotalWidth) // Karty są rozkładane żeby zmiesciły sie w kontenerze (przez to się na siebie nakładaja)
        {
            var width = _rectTransform.rect.width * transform.lossyScale.x;
            var distanceBetweenCards = (width - cardsTotalWidth) / (_cards.Count - 1);
            var currX = transform.position.x - width / 2;
            foreach (Wrapper card in _cards)
            {
                var correctCardWidth = card.Width * card.transform.lossyScale.x;
                card.targetPosition = new Vector2(currX + correctCardWidth / 2, transform.position.y);
                currX += correctCardWidth + distanceBetweenCards;
            }
        }

        private void DistributeChildrenWithoutOverlap(float cardsTotalWidth) // Karty są rozkładane nie zależnie od szerokości kontenera (nie będą się na siebie nakładać)
        {
            var currPosition = GetAnchorPositionByAlignment(cardsTotalWidth);
            foreach (Wrapper card in _cards)
            {
                var adjustedChildWidth = card.Width * card.transform.localScale.x;
                card.targetPosition = new Vector2(currPosition + adjustedChildWidth / 2, transform.position.y);
                currPosition += adjustedChildWidth;
            }
        }
    
        private float GetAnchorPositionByAlignment(float childrenWidth) {
            var containerWidthInGlobalSpace = _rectTransform.rect.width * transform.lossyScale.x;
            switch (_alignment) {
                case CardAlignment.Left:
                    return transform.position.x - containerWidthInGlobalSpace / 2;
                case CardAlignment.Center:
                    return transform.position.x - childrenWidth / 2;
                case CardAlignment.Right:
                    return transform.position.x + containerWidthInGlobalSpace / 2 - childrenWidth;
                default:
                    return 0;
            }
        }

        public Vector2 getPlaceHolderPosition()
        {
            return _placeHolderPosition.transform.position;
        }
    }
}
