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
 * Funkcja OnEndDrag() w momencie zakonczenia przeciagania jesli przedmiot znajduje sie 
 * w miejscu w kturym rownierz znajduje sie odpowiedni slot, przypisuje ten slot jako rodzica
 * przedmiotu oraz uruchamia funkcje dodajaca karty do okna kart. Jesli przeciaganie zostanie zakonczone
 * w miejscu poza poprawnym slotem przedmiot powroci na odpowiednie miejsce w oknie inventory.
 * 
 * Funkcja AddCardsToDeck() dodaje karty zawarte w przedmiocie dodanym do ekwipunku do okna kart
 * 
 * Funkcja RemoveCardsFromDeck() usuwa karty przypisane do danego przedmiotu jesli zostal on usuniety z ekwipunku
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
    public List<GameObject> cardsList = new List<GameObject>();
    [SerializeField] private GameObject cardSlotPF;
    private GameObject itemsPanel;
    public GameObject itemSlotPF;
    private bool cardsSpawned = false;

    public GameObject _tempOldParent;

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
        _tempOldParent = transform.parent.gameObject;

        // item zawsze wyswietla sie przed oknami inventarza
        transform.SetParent(transform.parent.parent.parent, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemChangePlace();
    }

    public void BackToTempParent(bool doubleClick = false)
    {
        if (doubleClick) return;
        transform.SetParent(_tempOldParent.transform, true);
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

        // tworzenie kart i dodawanie do panelu kart
        foreach(var c in item.cards)
        {
            
            var card = Instantiate(cardPrefab);
            card.GetComponent<CardDisplay>().cardSO = c;
            Destroy(card.GetComponent<CardZoom>());
            Destroy(card.GetComponent<CardUse>());
            card.transform.SetParent(cardsPanel.transform);
            cardsList.Add(card);
            card.transform.localScale = new Vector3(1, 1, 1);

            cardsSpawned = true;
        }

    }

    public void RemoveCardsFromDeck()
    {
        if (!cardsSpawned) return;

        // usuwanie kart
        if (cardsList.Count > 0)
            foreach (var c in cardsList)
            {
                Destroy(c);
                cardsSpawned = false;
            }
        cardsList.Clear();
    }
    public void OnItemChangePlace(bool doubleClicked = false)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        BackToTempParent(doubleClicked);

        // czy zmienil sie rodzic itemu
        if (transform.parent.transform == parentTransform)
        {
            // czy istnieje oryginalny rodzic
            if (originParentTransform == null)
            {
                // tworzenie slotu w panelu itemow
                var sl = Instantiate(itemSlotPF);
                sl.GetComponent<ItemSlot>().allowedItemType = ItemType.any;
                sl.transform.SetParent(itemsPanel.transform);
                originParentTransform = sl.transform;
            }
            // item wraca do panelu itemow i karty sa usuwane
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
}
