using System;
using System.ComponentModel;
using System.Linq;
using Grid.New;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace CardActions
{
    public class ApplyCardEffect : MonoBehaviour
    {
        [SerializeField] private GameObject popUpPrefab;
        [Header("Timer from Managers")][SerializeField] public GameObject gameObjectTimer;
        private HealthBar _healthBar;
        private Timer _timer;
        private CardsEffectsManager _cardsEffectsManager;
        private Image _image; 
        public OverlayTile standingOnTile;

        private bool isInRange = false; // TODO przygotowanie pod animator


        private void Start()
        {
            _timer = gameObjectTimer.GetComponent<Timer>();
            _healthBar = gameObject.GetComponentInParent<HealthBar>();
            // _image = gameObject.GetComponent<SpriteRenderer>()
        }

        private void Update()
        {
            if (standingOnTile != GetCurrentTile())
            {
                standingOnTile = GetCurrentTile();

                if (isOnRangedTile())
                {
                    isInRange = true;
                }
                else if (isInRange)
                {
                    isInRange = false;
                }
            }
        }

        private void OnMouseDown()
        {
            if (!Input.GetMouseButtonDown(0) || !PlayerController.getPlayerTurn()) return;
            if (isOnRangedTile()) return;
            if (!PlaceHolder.isTaken) return;
            
            var cardInfo = Wrapper.GetCardCurrentCardInfo();
            SendHpModification(cardInfo);
        }

        private void SendHpModification(Wrapper cardInfo)
        {
            var card = cardInfo.display.cardSO;
            var cost = card.cost;
            var hpChange = card.damage;
            switch (card.type)
            {
                case CardType.Attack:
                    hpChange = hpChange * -1;
                    break;
                case CardType.Defense:
                    break;
            }
            
            _healthBar.ChangeHealth(hpChange);
            _timer.ChangeActiveTimerValue(cost);
            ShowPopUpDamage(hpChange);
            EventSystem.DestroyCard.Invoke();
            EventSystem.LogAction?.Invoke(card);

            Debug.Log("Change: " + hpChange);
            Debug.Log(_healthBar.GetHealth());
        }

        private void ShowPopUpDamage(int hpChange)
        {
            var popUpPosition = new Vector2(transform.position.x, transform.position.y + 1);
            GameObject popUp = Instantiate(popUpPrefab, popUpPosition, Quaternion.identity);
            var text = popUp.GetComponentInChildren<TMP_Text>();
            string popUpText;
            if (hpChange > 0)
            {
                popUpText = "+" + hpChange;
                text.color = Color.green;
            }
            else
            {
                popUpText = hpChange.ToString();
                text.color = Color.red;
            }

            text.text = popUpText;
        }
        public OverlayTile GetCurrentTile()
        {
            RaycastHit2D? hit = GetFocusedOnTile(transform.position);

            if (hit.HasValue)
            {
                OverlayTile overlayTile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

                return overlayTile;
            }
            return null;
        }
        public RaycastHit2D? GetFocusedOnTile(Vector3 spawnPosition)
        {
            Vector2 spawnPosition2d = new Vector2(spawnPosition.x, spawnPosition.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(spawnPosition2d, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        private bool isOnRangedTile()
        {
            return !MouseController._rangeTiles.Contains(standingOnTile);
        }
    }
}