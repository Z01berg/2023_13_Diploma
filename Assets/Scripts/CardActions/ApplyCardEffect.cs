using System;
using System.ComponentModel;
using System.Linq;
using Grid.New;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CardActions
{
    public class ApplyCardEffect : MonoBehaviour
    {
        [SerializeField] private GameObject popUpPrefab;
        [SerializeField] private Animator _animator;
        
        [SerializeField] public GameObject timersController;
        private HealthBar _healthBarScript;
        private GameObject _hpSliderGameObj;
        private TimersManager _timersManager;
        private CardsEffectsManager _cardsEffectsManager;
        private Image _image;
        
        public OverlayTile standingOnTile;

        private bool isInRange = false; // TODO przygotowanie pod animator


        private void Start()
        {
            _timersManager = timersController.GetComponent<TimersManager>();
            _healthBarScript = gameObject.GetComponentInParent<HealthBar>();
            _hpSliderGameObj = _healthBarScript._slider.gameObject;
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
            _timersManager.PlayedAttackCard(_healthBarScript.gameObject, _hpSliderGameObj);
        }

        private void SendHpModification(Wrapper cardInfo)
        {
            var card = cardInfo.display.cardSO;
            var cost = card.cost;
            var hpChange = card.damage;
            hpChange = (card.type == CardType.Attack ? hpChange * -1 : hpChange);

            _healthBarScript.ChangeHealth(hpChange);
            _timersManager.ChangeActiveTimerValue(cost);
            AnimateSourceEntity(card.type); 
            ShowPopUpDamage(hpChange);
            EventSystem.DestroyCard.Invoke();
            EventSystem.LogAction?.Invoke(card);
        }

        private void AnimateSourceEntity(CardType cardType)
        {
            var sourceTimerData = _timersManager.GetTimerDataByIndex(_timersManager.ActiveTimerIndex);

            if (sourceTimerData != null && sourceTimerData.HP != null)
            {
                Transform root = sourceTimerData.HP.transform.root;
        
                Transform animationTransform = root.Find("AnimationOnHit&&OnCast");
                
                if (animationTransform != null)
                {
                    Animator sourceAnimator = animationTransform.GetComponent<Animator>();
                    if (sourceAnimator != null)
                    {
                        HandleTypeAction(cardType, sourceAnimator);
                    }
                }
            }
        }

        private void HandleTypeAction(CardType cardType, Animator _animator)
        {
            switch (cardType)
            {
                case CardType.Attack:
                    _animator.SetTrigger("Attack");
                    break;
                
                case CardType.Defense:
                    _animator.SetTrigger("Armor");
                    break;
                
                case CardType.Curse:
                    _animator.SetTrigger("Curse");
                    break;
                
                case CardType.Movement:
                    _animator.SetTrigger("Move");
                    break;
            }
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
                ShowHitAnimation();
            }
            else
            {
                popUpText = hpChange.ToString();
                text.color = Color.red;
                ShowHitAnimation();
            }

            text.text = popUpText;
        }
        
        private void ShowHitAnimation()
        {
            _animator.SetInteger("checker", GetRandomNumber());
            _animator.SetTrigger(GetAnimationOnHit());
        }
        
        public String GetAnimationOnHit()
        {   
            Array values = Enum.GetValues(typeof(HitAnimation));
        
            HitAnimation randomAnimation = (HitAnimation)values.GetValue(Random.Range(0, values.Length));
        
            return randomAnimation.ToString();
        }
        
        int GetRandomNumber()
        {
            int random = Random.Range(1, 10); 
            if (random == 5)
                random++; 
            
            return random;
        }

        
        private OverlayTile GetCurrentTile()
        {
            RaycastHit2D? hit = GetFocusedOnTile(transform.position);

            if (hit.HasValue)
            {
                OverlayTile overlayTile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

                return overlayTile;
            }
            return null;
        }
        private RaycastHit2D? GetFocusedOnTile(Vector3 spawnPosition)
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