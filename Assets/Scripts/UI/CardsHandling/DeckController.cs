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
            // if ((_attackDeck.Count == 0 || _defenceDeck.Count == 0 || _movementDeck.Count == 0) && _deckExists)
            // {
            //     RemoveCardsFromDeck();
            //
            //     CreateDeck();
            // }
        }

        public void ManageDeck()
        {
            UpdateCards();
            Debug.Log(CombatMode.isPlayerInCombat);
            if (_attackDeck.Count != 0 && !CombatMode.GetIsPlayerInCombat())
            {
                UpdateDeck();
            }
            if (_defenceDeck.Count != 0 && !CombatMode.GetIsPlayerInCombat())
            {
                UpdateDeck();
            }
            if (_movementDeck.Count != 0 && !CombatMode.GetIsPlayerInCombat())
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
            RemoveCardsFromDeck();
            UpdateCards();
            CreateDeck();
        }


        private void RemoveCardsFromDeck()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            _movementDeck.Clear();
            _attackDeck.Clear();
            _defenceDeck.Clear();
        }
        
        private void CreateDeck()
        {
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
                
            }

            _deckExists = true;
            Destroy(_createDeckText);
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

        
        public void DrawACard() 
        {
            if (_attackDeck.Count == 0 || _defenceDeck.Count == 0 || _movementDeck.Count == 0)
            {
                UpdateDeck();
            }
            do
            {
                if (HandController.currentAttackCardNumber < HandController.attackCardLimit)
                {
                    SendCardToHand(CardType.Attack);
                }

                if (HandController.currentDefenceCardNumber < HandController.defenceCardLimit)
                {
                    SendCardToHand(CardType.Defense);
                }

                if (HandController.currentMovementCardNumber < HandController.movementCardLimit)
                {
                    SendCardToHand(CardType.Movement);
                }

                HandController.currentCardNumber++;
            }while (HandController.currentCardNumber < HandController.cardLimit);
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
                    HandController.currentDefenceCardNumber++;
                    break;
                case CardType.Movement:
                    card = _movementDeck[Random.Range(0, _movementDeck.Count)];
                    _movementDeck.Remove(card);
                    card.transform.SetParent(_hand.transform);
                    HandController.currentMovementCardNumber++;
                    break;
            }
        }
        

        public bool IsDeckCreated()
        {
            return _deckExists;
        }
        
        
        
    }
}