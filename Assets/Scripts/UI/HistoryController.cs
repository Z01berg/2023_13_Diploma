using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class HistoryController : MonoBehaviour
    {
        [SerializeField] private GameObject historyTile;
        [SerializeField] private GameObject actionDetail;
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

        private void CreateLog(CardsSO cardInfo)
        {
            var newTile = Instantiate(historyTile, transform, true);
            newTile.transform.localScale = new Vector3(1, 1, 1);
            var tileText = newTile.GetComponentInChildren<TMP_Text>();

            if (cardInfo.type == CardType.Attack)
            {
                tileText.color = Color.red;
                tileText.text = "-" + cardInfo.damage;
            }
            else if (cardInfo.type == CardType.Defense)
            {
                tileText.color = Color.green;
                tileText.text = "+" + cardInfo.damage;
            }
            else
            {
                tileText.color = Color.white;
                tileText.text = cardInfo.damage.ToString();
            }

            _tilesCounter++;
            UpdateSize();
            // CreateDetail(cardInfo, newTile);
        }

        private void UpdateSize()
        {
            if (_tilesCounter > maxTilesInContainer)
            {
                var offsetMin = _containerSize.offsetMin;
                offsetMin = new Vector2(offsetMin.x, offsetMin.y - (_tileHeight + 5));
                _containerSize.offsetMin = offsetMin;
            }
        }

        public void CreateDetail(CardsSO cardInfo, GameObject tile)
        {
            actionDetail = Instantiate(actionDetail, transform.parent.transform.parent);
            ActionDetail detail = actionDetail.GetComponent<ActionDetail>();
            detail.Create(cardInfo, tile);
        }
        
    }
}