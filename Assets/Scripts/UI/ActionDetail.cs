using System;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class ActionDetail : MonoBehaviour
    {

        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private GameObject _detailPrefab;
        private CardsSO _cardInfo;
        private CardDisplay _display;
        private GameObject _detailGameObject;
        private GameObject _tile;

        private void Start()
        {
            _display = _detailPrefab.GetComponentInChildren<CardDisplay>();
            _detailGameObject = Instantiate(_detailPrefab);
        }

        // private void Update()
        // {
        //     _detailGameObject.transform.position =
        //         new Vector3(_tile.transform.position.x + 50, _tile.transform.position.y);
        // }

        public void Create(CardsSO card, GameObject historyTile)
        {
            _cardInfo = card;
            _tile = historyTile;
            CreateCard();
        }
        private void CreateCard()
        {
            var newCard = Instantiate(_cardPrefab, transform, true);
            _display.cardSO = _cardInfo;
            // newCard.transform.position = new Vector2(transform.position.x - _overlay, transform.position.y);
            newCard.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
    }
}