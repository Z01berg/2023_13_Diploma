using System;
using System.Collections.Generic;
using Dungeon;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace UI
{
    public class DeckController : MonoBehaviour
    {
        private List<CardsSO> _cards;
        private Stack<GameObject> _deck = new Stack<GameObject>();
        private HandController _handController;
        private Equipment _equipment;


        [NonSerialized] private bool _deckExists = false;

        [SerializeField] private TMP_Text _createDeckText;
        [SerializeField] GameObject _hand;
        [SerializeField] private GameObject _inventory;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private int _overlay = 50;
        private CoverPosition _coverPosition;


        private Random _rng = new Random();

        private void Awake()
        {
            _handController = _hand.GetComponent<HandController>();
            _equipment = _inventory.GetComponent<Equipment>();
            _coverPosition = GameObject.Find("Cover").GetComponent<CoverPosition>();
            EventSystem.DrawACard.AddListener(DrawACard);
        }

        private void Update()
        {
            if (_deck.Count == 0 && _deckExists)
            {
                CreateDeck();
            }
        }

        public void ManageDeck()
        {
            UpdateCards();
            Debug.Log(CombatMode.isPlayerInCombat);
            if (_deck.Count != 0 && !CombatMode.GetIsPlayerInCombat())
            {
                UpdateDeck();
            }

            if (!CombatMode.GetIsPlayerInCombat())
            {
                CreateDeck();
            }
            
        }

        private void UpdateDeck()
        {
            _deck.Clear();
            DestroyCreatedCards();
        }


        private void CreateDeck()
        {
            var diff = _overlay;
            foreach (var card in _cards)
            {
                var newCard = Instantiate(_cardPrefab, transform, true);
                newCard.GetComponent<CardDisplay>().cardSO = card;
                newCard.transform.position = new Vector2(transform.position.x - _overlay, transform.position.y);
                newCard.transform.localScale = new Vector3(1, 1, 1);
                _deck.Push(newCard);
                _overlay += diff;
                _coverPosition.UpdatePosition(newCard.transform);
            }

            _deckExists = true;
            Destroy(_createDeckText);
            _overlay = diff;
        }

        private void UpdateCards()
        {
            _cards = _equipment.cards;
            Shuffle();
        }

        private void DestroyCreatedCards()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void Shuffle()
        {
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = _rng.Next(n + 1);
                (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
            }
        }

        public void DrawACard()
        {
            if (_deck.Count == 0)
            {
                Debug.Log("Brak kart");
                return;
            }

            while (HandController.currentCardNumber < HandController.cardLimit)
            {
                if (_deck.Count == 0)
                {
                    return;
                }

                _coverPosition.UpdatePosition(_deck.Peek().transform);
                var card = _deck.Pop();
                card.transform.SetParent(_hand.transform);
                HandController.currentCardNumber++;
            }
        }

        public bool IsDeckCreated()
        {
            return _deckExists;
        }
    }
}