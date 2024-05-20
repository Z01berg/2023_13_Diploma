using System.ComponentModel;
using Player;
using TMPro;
using UI;
using UnityEngine;

namespace CardActions
{
    public class ApplyCardEffect : MonoBehaviour
    {
        [SerializeField] private GameObject popUpPrefab;
        [Header("Timer from Managers")][SerializeField] private GameObject gameObjectTimer;
        private GameObject _enemy;
        private HealthBar _healthBar;
        private Timer _timer;
        private CardsEffectsManager _cardsEffectsManager;

        private void Start()
        {
            // _enemy = transform.parent.gameObject;
            _healthBar = gameObject.GetComponentInParent<HealthBar>();
            _timer = gameObjectTimer.GetComponent<Timer>();
        }


        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0) && PlayerController.getPlayerTurn())
            {
                if (PlaceHolder.isTaken)
                {
                    var cardInfo = Wrapper.GetCardCurrentCardInfo();
                    SendHpModification(cardInfo);
                }
            }
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
            EventSystem.DestroyCard?.Invoke();

            Debug.Log("Change: " + hpChange);
            Debug.Log(_healthBar.getHealth());
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
    }
}