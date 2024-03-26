using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Publiczna klasa pozwalajaca na przeciaganie przedmiotow w ekwipunku z okna inventory do okna equipmenr i odwrotnie.
 * 
 * Funkcja OnBeginDrag() uruchamiana jest w momencie rozpoczecia przez gracza przeciagania przedmiotu, i przykleja ona 
 * owy przedmiot do polozenia aktualnego myszki.
 * 
 * Funkcja OnDrag() pilnuje by przeciagany przedmiot znajdowal sie w tym samym miejscu co myszka.
 * 
 * Funkcja OnEndDrag() 
 */

public class UIItemDragNDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Item item;
    public GameObject cardPrefab;
    public Transform parentTransform;
    public Transform originParentTransform;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GameObject cardsPanel;
    private List<GameObject> cardsList = new List<GameObject>();
    [SerializeField] private GameObject cardSlotPF;
    private GameObject itemsPanel;
    public GameObject itemSlotPF;
    private bool cardsSpawned = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindAnyObjectByType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        cardsPanel = GameObject.Find("CardsPanel").gameObject;
        itemsPanel = GameObject.Find("ItemsPanel").gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if(transform.parent.transform == parentTransform)
        {

            if(originParentTransform == null)
            {
                var sl = Instantiate(itemSlotPF);
                sl.transform.SetParent(itemsPanel.transform);
                originParentTransform = sl.transform;
            }
            rectTransform.position = originParentTransform.position;
            rectTransform.SetParent(originParentTransform);
            RemoveCardsFromDeck();
        }
        else
        {
            AddCardsToDeck();
            
        }
        parentTransform = transform.parent.transform;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void AddCardsToDeck()
    {
        if (cardsSpawned) return;

        foreach(var c in item.cards)
        {
            
            var card = Instantiate(cardPrefab);
            card.GetComponent<CardDisplay>().cardSO = c;
            Destroy(card.GetComponent<CardZoom>());
            Destroy(card.GetComponent<CardUse>());
            card.transform.SetParent(cardsPanel.transform);
            cardsList.Add(card);

            cardsSpawned = true;
        }

    }

    public void RemoveCardsFromDeck()
    {
        if (!cardsSpawned) return;

        if (cardsList.Count > 0)
            foreach (var c in cardsList)
            {
                Destroy(c);
                cardsSpawned = false;
            }
        
    }

}
