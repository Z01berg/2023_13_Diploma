using System;
using UI.Config;
using UI.Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UI
{
    public class Wrapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        /**
         * Publiczna metoda wrapper kontroluję kartę, do której należy (dlatego jest przypisywana ze wstrony HandContoller)
         * Uruchamia wszystkie eventy tej karty, zmiany jej pozycji, zmiany rozmiaru, zmiany skali itp.
         */
        public Vector2 targetPosition;

        public float targetVerticalDisplacement;
        private Vector2 _dragStartPos;
        
        public int uiLayer;
        public HandController handController;

        private RectTransform _rectTransform;
        private Canvas _canvas;
        
        // public bool isPressed;
        public bool isHovered;
        public bool isDragged;

        public ZoomConfig zoomConfig;
        public EventsConfig eventsConfig;
        public AnimationConfig animationConfig;

        public static Wrapper cardInUse;

        public CardDisplay display;

        private float _requiredHoldTime; // seconds
        private float _timeHeld = 0f;
        private bool _isHeld;

        public float Width
        {
            get => _rectTransform.rect.width * _rectTransform.localScale.x;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            display = GetComponent<CardDisplay>();
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _requiredHoldTime = handController.requiredHoldTime;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                DeselectCard();
            }
            UpdatePosition();
            // UpdateScale();
            UpdateUILayer();
            PressAndHold();
        }

        private void PressAndHold()
        {
            if (_isHeld)
            {
                _timeHeld += Time.deltaTime;
                if (_timeHeld >= _requiredHoldTime)
                {
                    isDragged = true;
                }
            }
        }

        private void UpdatePosition()
        {

            if (Time.deltaTime == 0)
            {
               return; 
            }
            if (cardInUse != this)
            {
                var target = new Vector2(targetPosition.x, targetPosition.y + targetVerticalDisplacement);
                
                if (isHovered && zoomConfig.overrideYPositionOfChosenCard != -1)
                {
                    target = new Vector2(target.x, zoomConfig.overrideYPositionOfChosenCard);
                }

                if (cardInUse != null && zoomConfig.overrideYPositionOfChosenCard != -1 )
                {
                    if (!cardInUse.isDragged)
                    {
                        target = new Vector2(target.x, zoomConfig.overrideYPositionOfUnUsedCard);
                    }
                }
                
                var position = _rectTransform.position;
                var distance = Vector2.Distance(position, target);
                
                position = Vector2.Lerp(position, target,
                    animationConfig.hoveringCardsSpeed / distance * Time.deltaTime);
                _rectTransform.position = position;
                
                
            }
            else
            {
                if (!isDragged)
                {
                    var target = new Vector2(targetPosition.x, targetPosition.y + targetVerticalDisplacement);
                    if (zoomConfig.overrideYPositionOfChosenCard != -1)
                    {
                        target = new Vector2(target.x, zoomConfig.overrideYPositionOfChosenCard);
                    }

                    // var inUsePosition = handController.getPlaceHolderPosition();
                    var position = _rectTransform.position;
                    var distance = Vector2.Distance(position, target);

                    position = Vector2.Lerp(position, target,
                        animationConfig.drawingCardsSpeed / distance * Time.deltaTime);
                    _rectTransform.position = position;
                }
                else
                {
                    var delta = ((Vector2)Input.mousePosition + _dragStartPos);
                    _rectTransform.position = new Vector2(delta.x, delta.y);
                }
            }
        }

        private void UpdateScale()
        {
            if (Time.deltaTime == 0)
            {
                return;
            }
            
            var targetZoom = (cardInUse == this || isHovered) && zoomConfig.zoomOnHover ? zoomConfig.multiplier : 1;
            var delta = Mathf.Abs(_rectTransform.localScale.x - targetZoom);
            var newZoom = Mathf.Lerp(_rectTransform.localScale.x, targetZoom,
                animationConfig.zoomSpeed / delta * Time.deltaTime);
            _rectTransform.localScale = new Vector3(newZoom, newZoom, 1);
        }

        private void UpdateUILayer()
        {
            if (!isHovered && cardInUse != this)
            {
                _canvas.sortingOrder = uiLayer;
            }
        }

        public void SetAnchor(Vector2 min, Vector2 max)
        {
            _rectTransform.anchorMin = min;
            _rectTransform.anchorMax = max;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (cardInUse == this)
            {
                return;
            }
            if (isDragged)
            {
                return;
            }

            if (zoomConfig.bringToFrontOnHover)
            {
                _canvas.sortingOrder = zoomConfig.zoomedSortOrder;
            }

            eventsConfig?.cardHover?.Invoke(new CardHover(this));
            isHovered = true;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (cardInUse == this)
            {
                return;
            }
            if (isDragged)
            {
                return;
            }

            _canvas.sortingOrder = uiLayer;
            isHovered = false;
            eventsConfig.cardUnHover.Invoke(new CardUnhover(this));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isHeld = true;
                if(isDragged)
                {
                    _dragStartPos = new Vector2(transform.position.x - eventData.position.x,
                        transform.position.y - eventData.position.y);
                    eventsConfig?.cardUnHover?.Invoke(new CardUnhover(this));
                    return;
                }

                handController.OnCardDragStart(this);
                
                if (PlaceHolder.isTaken)
                {
                    // cardInUse.isPressed = false;
                    cardInUse.isHovered = false;
                    eventsConfig?.cardUnHover?.Invoke(new CardUnhover(this));
                    EventSystem.HideRange.Invoke();
                }
                eventsConfig?.cardHover?.Invoke(new CardHover(this));
                // isPressed = true;
                // isHovered = true;
                cardInUse = this;
                PlaceHolder.isTaken = true;
                int range = Convert.ToInt32(GetComponent<CardDisplay>().range.text);
                EventSystem.ShowRange.Invoke(range);
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (cardInUse == this)
                {
                    DeselectCard();
                }
            }
        }

        void DeselectCard()
        {
                eventsConfig?.cardUnHover?.Invoke(new CardUnhover(this));
                // isPressed = false;
                isHovered = false;
                cardInUse = null;
                PlaceHolder.isTaken = false;
                EventSystem.HideRange.Invoke();
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragged = false;
            _isHeld = false;
            _timeHeld = 0f;
            handController.OnCardDragEnd();
        }


        private void OnDestroy()
        {
            EventSystem.HideRange.Invoke();
        }

        public static Wrapper GetCardCurrentCardInfo()
        {
            return cardInUse;
        }
    }
}