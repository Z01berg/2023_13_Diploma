using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEditor.Progress;

/**
 * Klasa publiczna monitorujaca pola ekwipunku i dodajaca karty podstawowe do reki gracza 
 * w razie gdyby nie wybral nic w koryms z pol ekwipunku.
 * 
 * Klasa zawiera pola:
 * - _cards: zawierajace karty utworzone dla danego okna
 * - _cardPrefab: do ktorego przypisany jest prefab karty
 * - _cardsPanel: przypisany jest do niego panel kart w oknie ekwipunku
 * - _cardsList: lista kart podstawowych danego slotu w postaci ScriptableObject
 * - _defaultAdded: wartosc wskazujaca na to czy karty podstatwowe zostaly dodane czy nie
 * 
 * Klasa zawiera funkcje:
 * - DisplayCards(): tworzy i dodaje karty do ekwipunku
 * - HideCards(): usuwa karty z ekwipunku
 * - AssignCard(): pozwala na przypisanie nowej karty podstawowej do slotu
 * - ClearList(): usowa karty z listy _cardsList
 */

public class DefaultCards : MonoBehaviour
{
    public List<CardsSO> _cards = new();
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardsPanel;
    public List<GameObject> _cardsList = new();
    private bool _defaultAdded = true;

    private ItemSlot _slotScript;
    [SerializeField] private int _defaultCardsAmmount = 3;

    private AsyncOperationHandle<IList<CardsSO>> _loadHandle;

    private void Awake()
    {
        _slotScript = GetComponent<ItemSlot>();

        List<string> tags = new List<string>() { "DefaultCard"};

        switch (_slotScript.allowedItemType)
        {
            case ItemType.cheast or ItemType.head:
                tags.Add("DefenceCard");
                break;
            case ItemType.legs or ItemType.boots:
                tags.Add("MovementCard");
                break;
            case ItemType.hand:
                tags.Add("AttackCard");
                break;
            case ItemType.additional:
                tags.Add("TrinketCard");
                break;
        }

        LoadCards(tags);
        _loadHandle.WaitForCompletion();

        if(transform.childCount < 1)
        {
            DisplayCards();
        }
    }

    private void Update()
    {
        if (transform.childCount > 0 && _defaultAdded)
        {
            HideCards();
            _defaultAdded = false;
        }
        else if(transform.childCount < 1 && !_defaultAdded)
        {
            DisplayCards();
            _defaultAdded = true;
        }
    }

    private void DisplayCards()
    {
        _cardsList.Clear();
        foreach (var c in _cards)
        {
            var card = Instantiate(cardPrefab);
            card.GetComponent<CardDisplay>().cardSO = c;
            Destroy(card.GetComponent<CardZoom>());
            Destroy(card.GetComponent<CardUse>());
            card.transform.SetParent(cardsPanel.transform);
            card.transform.localScale = new Vector3(1, 1, 1);
            _cardsList.Add(card);
        }
    }

    private void HideCards()
    {
        foreach(var c in _cardsList)
        {
            Destroy(c);
        }
        _cardsList.Clear();
    }

    private void LoadCards(List<string> keys)
    {
        _cards.Clear();

        _loadHandle = Addressables.LoadAssetsAsync<CardsSO>(
            keys,
            addressable =>
            {
                for (int i = 0; i < _defaultCardsAmmount; i++) 
                {
                    _cards.Add(addressable);
                }
                Debug.Log(_cards.Count);
            }, Addressables.MergeMode.Intersection,
            false);
    }
}
