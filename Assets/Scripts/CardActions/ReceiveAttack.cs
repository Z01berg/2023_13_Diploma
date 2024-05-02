using System;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardActions
{
    public class ReceiveAttack : MonoBehaviour
    {
        private GameObject _enemy;
        private HealthBar _healthBar;
        private Timer _timer;
        private CardsEffectsManager _cardsEffectsManager;

        private void Start()
        {
            _enemy = transform.parent.gameObject;
            _healthBar = gameObject.GetComponentInParent<HealthBar>();
            _timer = gameObject.GetComponentInParent<Timer>();
        }
        
        
        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (PlaceHolder.isTaken)
                {
                    var cardInfo = Wrapper.GetCardCurrentCardInfo();
                    var damage = cardInfo.damage;
                    var cost = cardInfo.cost;
                    SendDamage(damage, cost);
                }
            }
        }

        private void SendDamage(int damage, int cost)
        {
            Debug.Log("Damage: " + damage);
            Debug.Log(_healthBar.getHealth());
            EventSystem.WhatHP.Invoke(_healthBar.getGameObject(), cost);
            //TODO: Dogadac z Zakharem jak to według niego powinno działac
        }
    }
}