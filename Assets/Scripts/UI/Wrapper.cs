using UI.Config;
using UI.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Wrapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        /**
         * Publiczna metoda wrapper kontroluję kartę, do której należy (dlatego jest przypisywana ze wstrony HandContoller)
         * Uruchamia wszystkie eventy tej karty, zmiany jej pozycji, zmiany rozmiaru, zmiany skali itp.
         */
        public Vector2 targetPosition;

        public float targetVerticalDisplacement;

        public int uiLayer;
        public HandController handController;

        private RectTransform _rectTransform;
        private Canvas _canvas;
        private bool _isPressed;
        private bool _isHovered;

        public ZoomConfig zoomConfig;
        public EventsConfig eventsConfig;
        public AnimationConfig animationConfig;

        public static Wrapper cardInUse;

        private CardDisplay _display;

        public float Width
        {
            get => _rectTransform.rect.width * _rectTransform.localScale.x;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _display = GetComponent<CardDisplay>();
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            UpdatePosition();
            UpdateScale();
            UpdateUILayer();
        }

        private void UpdatePosition()
        {
            if (!_isPressed)
            {
                var target = new Vector2(targetPosition.x, targetPosition.y + targetVerticalDisplacement);
                if (_isHovered && zoomConfig.overrideYPosition != -1)
                {
                    target = new Vector2(target.x, zoomConfig.overrideYPosition);
                }

                var distance = Vector2.Distance(_rectTransform.position, target);
                _rectTransform.position = Vector2.Lerp(_rectTransform.position, target,
                    animationConfig.positionChangeSpeed / distance * Time.deltaTime);
            }
            else
            {
                var inUsePosition = handController.getPlaceHolderPosition();
                var distance = Vector2.Distance(_rectTransform.position, inUsePosition);
                _rectTransform.position = Vector2.Lerp(_rectTransform.position, inUsePosition,
                    animationConfig.positionChangeSpeed / distance * Time.deltaTime);
            }
        }

        private void UpdateScale()
        {
            var targetZoom = (_isPressed || _isHovered) && zoomConfig.zoomOnHover ? zoomConfig.multiplier : 1;
            var delta = Mathf.Abs(_rectTransform.localScale.x - targetZoom);
            var newZoom = Mathf.Lerp(_rectTransform.localScale.x, targetZoom,
                animationConfig.zoomSpeed / delta * Time.deltaTime);
            _rectTransform.localScale = new Vector3(newZoom, newZoom, 1);
        }

        private void UpdateUILayer()
        {
            if (!_isHovered && !_isPressed)
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
            if (_isPressed)
            {
                return;
            }

            if (zoomConfig.bringToFrontOnHover)
            {
                _canvas.sortingOrder = zoomConfig.zoomedSortOrder;
            }

            eventsConfig?.cardHover?.Invoke(new CardHover(this));
            _isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isPressed)
            {
                return;
            }

            _canvas.sortingOrder = uiLayer;
            _isHovered = false;
            eventsConfig.cardUnHover.Invoke(new CardUnhover(this));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (PlaceHolder.isTaken)
                {
                    cardInUse._isPressed = false;
                    cardInUse._isHovered = false;
                }

                _isPressed = true;
                cardInUse = this;
                _isHovered = false;
                eventsConfig?.cardUnHover?.Invoke(new CardUnhover(this));
                PlaceHolder.isTaken = true;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (_isPressed)
                {
                    _isPressed = false;
                    PlaceHolder.isTaken = false;
                }
            }
        }
    }
}