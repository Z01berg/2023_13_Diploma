using Player;
using UI;
using UnityEngine;

namespace CardActions
{
    public class ReceiveAttack : MonoBehaviour
    {
        private GameObject _enemy;
        private HealthBar _healthBar;
        public GameObject gameObjectTimer;
        private Timer _timer;
        private CardsEffectsManager _cardsEffectsManager;
        private HandController _handController;

        private void Start()
        {
            _enemy = transform.parent.gameObject;
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
                    SendDamage(cardInfo);
                }
            }
        }

        private void SendDamage(Wrapper cardInfo)
        {
            var damage = cardInfo.display.cardSO.damage;
            var cost = cardInfo.display.cardSO.cost;
            _healthBar.ChangeHealth(-damage);
            _timer.ChangeActiveTimerValue(cost);
            cardInfo.handController.DestroyCard();
                
            Debug.Log("Damage: " + damage);
            Debug.Log(_healthBar.getHealth());
        }
    }
}