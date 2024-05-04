using System.Collections.Generic;
using System.Linq;
using UI.Config;
using UI.Events;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    /**
     * Publiczna klasa HandController jest klasą która odpowiada za ustawienie kart w ręce.
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

        [SerializeField] private ZoomConfig _zoomConfig;

        [Header("Constraints")] [SerializeField]
        private bool _forceFitContainer; //Decyduje czy karty się na siebie nakładają lub nie

        public static int cardLimit = 5;

        public static int currentCardNumber;

        [Header("Alignment")] [SerializeField] private AnimationConfig _animationConfig;

        private Wrapper _currDraggedCard;

        [SerializeField] private EventsConfig _eventsConfig;
        [SerializeField] private GameObject _placeHolder;

        private RectTransform _placeHolderPosition;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _placeHolderPosition = _placeHolder.GetComponent<RectTransform>();
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

                wrapper.zoomConfig = _zoomConfig;
                wrapper.handController = this;
                wrapper.eventsConfig = _eventsConfig;
                wrapper.animationConfig = _animationConfig;
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

        private void SetCardsAnchor()
        {
            foreach (Wrapper card in _cards)
            {
                card.SetAnchor(new Vector2(0, 0.5f), new Vector2(0, 0.5f));
            }
        }

        private void UpdateCards()
        {
            if (transform.childCount != _cards.Count)
            {
                InitCards();
            }

            if (_cards.Count == 0)
            {
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
                if (newCardIdx > originalCardIdx && newCardIdx < _cards.Count - 1)
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
                _cards[i].uiLayer = _zoomConfig.defaultSortOrder + i;
            }
        }

        private void SetCardsPosition()
        {
            // Oblicza całkowitą szerokość wszystkich kart
            var cardsTotalWidth = _cards.Sum(card => card.Width * card.transform.lossyScale.x);
            // Szerokość kontenera (hand)
            var containerWidth = _rectTransform.rect.width * transform.lossyScale.x;
            if (_forceFitContainer && cardsTotalWidth > containerWidth)
            {
                DistributeChildrenToFitContainer(cardsTotalWidth);
            }
            else
            {
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

        private void
            DistributeChildrenWithoutOverlap(
                float cardsTotalWidth) // Karty są rozkładane nie zależnie od szerokości kontenera (nie będą się na siebie nakładać)
        {
            var currPosition = transform.position.x - cardsTotalWidth / 2;
            foreach (Wrapper card in _cards)
            {
                var adjustedChildWidth = card.Width * card.transform.localScale.x;
                card.targetPosition = new Vector2(currPosition + adjustedChildWidth / 2, transform.position.y);
                currPosition += adjustedChildWidth;
            }
        }

        public void DestroyCard()
        {
            var card = Wrapper.cardInUse;
            if (card == Wrapper.cardInUse && PlaceHolder.isTaken)
            {
                _cards.Remove(card);
                _eventsConfig.cardDestroy?.Invoke(new CardDestroy(card));
                PlaceHolder.isTaken = false;
                currentCardNumber--;
                Destroy(card.gameObject);
            }
            else
            {
                Debug.Log("Wybierz kartę zanim jej użyjesz");
            }
        }

        public Vector2 getPlaceHolderPosition()
        {
            return _placeHolderPosition.transform.position;
        }
    }
}