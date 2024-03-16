using System;
using UI;
using UI.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wrapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
    IPointerUpHandler 
{
    public Vector2 targetPosition;
    public float targetVerticalDisplacement;

    public ZoomConfig zoomConfig;
    public int uiLayer;
    public HandController handController;
    
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private bool isPressed;
    private bool isHovered;
    private Vector2 dragStartPos;

    public EventsConfig eventsConfig;


    public float width
    {
        get => _rectTransform.rect.width * _rectTransform.localScale.x;
    }
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
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
    
    private void UpdatePosition() {
        if (!isPressed) {
            var target = new Vector2(targetPosition.x, targetPosition.y + targetVerticalDisplacement);
            if (isHovered && zoomConfig.overrideYPosition != -1) {
                target = new Vector2(target.x, zoomConfig.overrideYPosition);
            }

            var distance = Vector2.Distance(_rectTransform.position, target);
            _rectTransform.position = Vector2.Lerp(_rectTransform.position, target, 1000);
        }
        else {
            var delta = ((Vector2)Input.mousePosition + dragStartPos);
            _rectTransform.position = new Vector2(delta.x, delta.y);
        }
    }

    private void UpdateScale()
    {
        var targetZoom = (isPressed || isHovered) && zoomConfig.zoomOnHover ? zoomConfig.multiplier : 1;
        var delta = Mathf.Abs(_rectTransform.localScale.x - targetZoom);
        var newZoom = Mathf.Lerp(_rectTransform.localScale.x, targetZoom, 1);
        _rectTransform.localScale = new Vector3(newZoom, newZoom, 1);
    }

    private void UpdateUILayer()
    {
        if (!isHovered && !isPressed)
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
        if (isPressed)
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
        if (isPressed)
        {
            return;
        }

        _canvas.sortingOrder = uiLayer;
        isHovered = false;
        eventsConfig.cardUnhover.Invoke(new CardUnhover(this));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            eventsConfig?.cardUnhover?.Invoke(new CardUnhover(this));
        }

        if (Input.GetMouseButtonDown(1))
        {
            isPressed = false;
            
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
