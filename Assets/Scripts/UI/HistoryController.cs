using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class HistoryController : MonoBehaviour
    {
        [SerializeField] private GameObject historyTile;
        [SerializeField] private int maxTilesInContainer = 6;
        private int _tilesCounter = 0;
        private float _tileHeight;
        private RectTransform _containerSize;


        private void Start()
        {
            _containerSize = gameObject.GetComponent<RectTransform>();
            _tileHeight = historyTile.GetComponent<RectTransform>().sizeDelta.y;
            EventSystem.LogAction.AddListener(CreateLog);
        }

        private void CreateLog(CardsSO card)
        {
            var newTile = Instantiate(historyTile, transform, true);
            var tileText = newTile.GetComponentInChildren<TMP_Text>();

            if (card.type == CardType.Attack)
            {
                tileText.color = Color.red;
                tileText.text = "-" + card.damage;
            }
            else if (card.type == CardType.Defense)
            {
                tileText.color = Color.green;
                tileText.text = "+" + card.damage;
            }
            else
            {
                tileText.color = Color.white;
                tileText.text = card.damage.ToString();
            }

            _tilesCounter++;
            UpdateSize();
        }

        private void UpdateSize()
        {
            if (_tilesCounter > maxTilesInContainer)
            {
                var sizeDelta = _containerSize.sizeDelta;
                sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + _tileHeight + 5);
                _containerSize.sizeDelta = sizeDelta;
            }
        }
    }
}