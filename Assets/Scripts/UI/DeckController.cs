using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = System.Random;

namespace UI
{
    public class DeckController : MonoBehaviour
    {
        private List<CardsSO> _cards;
        private Stack<GameObject> _deck = new Stack<GameObject>();
        private HandController _handController;
        private Equipment _equipment;
        

        public bool canCreate = true;

        [SerializeField] private TMP_Text _createDeckText;
        [SerializeField] GameObject _hand;
        [SerializeField] private GameObject _inventory;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private int _overlay = 50;
        

        private Random _rng = new Random();

        private void Awake()
        {
            _handController = _hand.GetComponent<HandController>();
            _equipment = _inventory.GetComponent<Equipment>();
        }

        public void CreateDeck()
        {
            if (canCreate)
            {
                _cards = _equipment.cards;
                Shuffle();
                CreateCards();
                canCreate = false;
            }
        }
        

        private void CreateCards()
        {
            var diff = _overlay;
            foreach (var card in _cards)
            {
                var newCard = Instantiate(_cardPrefab, transform, true);
                newCard.GetComponent<CardDisplay>().cardSO = card;
                newCard.transform.position = new Vector2(transform.position.x - _overlay, transform.position.y);
                _deck.Push(newCard);
                _overlay += diff;
            }

            Destroy(_createDeckText);
            _overlay = diff;
        }

        private void UpdateDeck()
        {
            if (_cards.Count != _equipment.cards.Count)
            {
                _deck.Clear();
                _cards = _equipment.cards;
                Shuffle();
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
                var card = _deck.Pop();
                card.transform.SetParent(_hand.transform);
                HandController.currentCardNumber++;
            }
            
            
        }

        public bool IsDeckCreated()
        {
            return !canCreate;
        }
    }
}