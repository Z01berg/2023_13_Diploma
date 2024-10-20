using System;
using System.Collections.Generic;
using Dungeon;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class DeckController : MonoBehaviour
    {
        private List<CardsSO> _cards;
        private List<GameObject> _attackDeck = new List<GameObject>();
        private List<GameObject> _defenceDeck = new List<GameObject>();
        private List<GameObject> _movementDeck = new List<GameObject>();
        private HandController _handController;
        private Equipment _equipment;


        [NonSerialized] private bool _deckExists = false;

        [SerializeField] private TMP_Text _createDeckText;
        [SerializeField] GameObject _hand;
        [SerializeField] private GameObject _inventory;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private int _overlay = 50;
        private CoverPosition _coverPosition;
            
        /*Stworzyć 3 decki na kazdy rodzaj karty: attack, def, move. W momencie dobierania dobierać po dwie karty ataku, po dwie obrony i jedną ruchu.
         Utworzyć 3 heapy. Sprawdzać aktualną rękę i zliczać rodzaje kart. Na podstawie ich liczności dobierać karty  */

        // private Random _rng = new Random();

        private void Awake()
        {
            _handController = _hand.GetComponent<HandController>();
            _equipment = _inventory.GetComponent<Equipment>();
            _coverPosition = GameObject.Find("Cover").GetComponent<CoverPosition>();
            EventSystem.DrawACard.AddListener(DrawACard);
        }

        private void Update()
        {
            if (_attackDeck.Count == 0 && _defenceDeck.Count == 0 && _movementDeck.Count == 0 && _deckExists)
            {
                // CreateDeck();
            }
        }

        public void ManageDeck()
        {
            UpdateCards();
            Debug.Log(CombatMode.isPlayerInCombat);
            if (_attackDeck.Count != 0 && !CombatMode.GetIsPlayerInCombat())
            {
                UpdateDeck(_attackDeck);
            }
            if (_defenceDeck.Count != 0 && !CombatMode.GetIsPlayerInCombat())
            {
                UpdateDeck(_defenceDeck);
            }
            if (_movementDeck.Count != 0 && !CombatMode.GetIsPlayerInCombat())
            {
                UpdateDeck(_movementDeck);
            }

            if (!CombatMode.GetIsPlayerInCombat())
            {
                CreateDeck();
            }
            
        }

        private void UpdateDeck(List<GameObject> deck)
        {
            deck.Clear();
            DestroyCreatedCards();
        }


        private void CreateDeck()
        {
            var diff = _overlay;
            
            foreach (var card in _cards)
            {
                var newCard = Instantiate(_cardPrefab, transform, true);
                newCard.GetComponent<CardDisplay>().cardSO = card;
                var position = transform.position;
                newCard.transform.position = new Vector2(position.x - _overlay, position.y);
                newCard.transform.localScale = new Vector3(1, 1, 1);
                switch (card.type)
                {
                    case CardType.Attack: _attackDeck.Add(newCard);
                        break;
                    case CardType.Defense: _defenceDeck.Add(newCard);
                        break;
                    case CardType.Movement: _movementDeck.Add(newCard);
                        break;
                }
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
        }

        private void DestroyCreatedCards()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        // private void Shuffle()
        // {
        //     int n = _cards.Count;
        //     while (n > 1)
        //     {
        //         n--;
        //         int k = _rng.Next(n + 1);
        //         (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
        //     }
        // }

        // public void DrawACard() 
        // {
        //     if (_attackDeck.Count == 0 || _defenceDeck.Count == 0 || _movementDeck.Count == 0)
        //     {
        //         Debug.Log("Brak kart");
        //         return;
        //     }
        //
        //     while (HandController.currentCardNumber < HandController.cardLimit)
        //     {
        //         if (_attackDeck.Count == 0 || _defenceDeck.Count == 0 || _movementDeck.Count == 0)
        //         {
        //             return;
        //         }
        //
        //         // _coverPosition.UpdatePosition(_deck.Peek().transform);
        //         // var card = _deck.Pop();
        //         // card.transform.SetParent(_hand.transform);
        //         HandController.currentCardNumber++;
        //     }
        // }
        
        public void DrawACard() 
        {
            // if (_attackDeck.Count == 0 || _defenceDeck.Count == 0 || _movementDeck.Count == 0)
            // {
            //     Debug.Log("Brak kart");
            // }

            while (HandController.currentCardNumber < 5)
            {
                    
                if (HandController.currentAttackCardNumber < HandController.attackCardLimit)
                {
                    SendCardToHand(CardType.Attack);
            
                }
                
                if (HandController.currentDefenceNumber < HandController.attackCardLimit)
                {
                    SendCardToHand(CardType.Defense);
            
                }
                
                if (HandController.currentDefenceNumber < HandController.attackCardLimit)
                {
                    SendCardToHand(CardType.Movement);
                    continue;
                }
                
                HandController.currentCardNumber++;
            }
        }

        private void SendCardToHand(CardType type)
        {
            GameObject card;
            switch (type)
            {
                case CardType.Attack:
                    card = _attackDeck[Random.Range(0, _attackDeck.Count)];
                    _attackDeck.Remove(card);
                    card.transform.SetParent(_hand.transform);
                    HandController.currentAttackCardNumber++;
                    break;
                case CardType.Defense:
                    card = _defenceDeck[Random.Range(0, _defenceDeck.Count)];
                    _defenceDeck.Remove(card);
                    card.transform.SetParent(_hand.transform);
                    HandController.currentDefenceNumber++;
                    break;
                case CardType.Movement:
                    card = _movementDeck[Random.Range(0, _movementDeck.Count)];
                    _movementDeck.Remove(card);
                    card.transform.SetParent(_hand.transform);
                    HandController.currentMovementNumber++;
                    break;
            }
        }
        

        public bool IsDeckCreated()
        {
            return _deckExists;
        }
    }
}