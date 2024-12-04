using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeon;
using TMPro;
using UI;
using UnityEngine;

    /**
 * Publiczna klasa Timer ma za zadanie zarządzać funkcjonalnością zegarów oraz nimi samymi.
 *
 * Ma możliwość:
 * - Dodawania tekstu, tagu i odnośnika do obiektu HP dla nowego zegara.
 * - Obsługiwanie wejścia z klawiatury w celu manipulacji zegarami.
 * - Aktualizacji tekstów zegarów.
 * - Uruchamiania odliczania zegarów.
 * - Obliczania priorytetu dla aktywnego zegara.
 * - Ma ograniczenia dla Timer Value (0 - 99)
 */

    public class TimersManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _turn;
        [SerializeField] private GameObject _deck;
        [SerializeField] private GameObject _player;

        private static List<TimerData> _timers = new List<TimerData>();

        private int _activeTimerIndex = 0; // whose turn
        public int ActiveTimerIndex => _activeTimerIndex;

        private bool _counting = false; // after pressing enter

        private float _timeToPause = 0.4f; // for timer animations

        private bool _cheat = false; // enable cheat mode on "R CTRL" for changing timer meanings

        private bool _createdDeck = false;

        private Animator _animator;
        private DeckController _deckController;
        private EnemyController _enemyController;
        private bool _canDraw = false; // TODO: Change this selection

        public Dictionary<string, int> TagPriority = new Dictionary<string, int>
        {
            { "Item", 3 },
            { "Player", 2 },
            { "Boss", 1 },
            { "Enemy", 0 },
            { "N/A", -1 }
        };

        void Start()
        {
            _enemyController = FindObjectOfType<EnemyController>();
            _deckController = _deck.GetComponent<DeckController>();

            EventSystem.InstatiatedRoom.AddListener(AddToTimer);
            EventSystem.DeleteReference.AddListener(DeleteTimer);
            EventSystem.FinishEnemyTurn.AddListener(FinishTurn);
            EventSystem.InitInv.AddListener(ChangeBool);
            EventSystem.ZeroTimer.AddListener(ResetCurrentTimer);
            EventSystem.StartCountdown.AddListener(EnteredRoom);
        EventSystem.FinishTurnPlayer.AddListener(HandleTimerInput);
        }

        public void AddTextFromSetTimer(TMP_Text newText, string tag, GameObject HP)
        {
            int initialValue = (tag == "Enemy") ? UnityEngine.Random.Range(5, 10) : 0;

            TimerData timerData = new TimerData(initialValue, tag, HP, -1, newText, tag);

            if (tag == "Player")
            {
                if (_timers.Exists(t => t.Tag == "Player"))
                {
                    Debug.LogWarning("Player timer already exists. Skipping addition.");
                    return;
                }

                _timers.Insert(0, timerData);
            }
            else
            {
                _timers.Add(timerData);
            }
        }

        void EnteredRoom()
        {
            _counting = true;
        }

        private void ResetCurrentTimer()
        {
            ChangeActiveTimerValue(0);
        }

        private void ChangeBool()
        {
            _createdDeck = true;
        }

        private void FinishTurn(int arg0)
        {
            SetTimerValue(_activeTimerIndex, arg0);
            EnteredRoom();
        }

        void AddToTimer()
        {
            foreach (var timer in _timers)
            {
                if (timer.Tag == "Enemy" && timer.HP != null)
                {
                    Transform enemyTransform = timer.HP.transform.parent?.parent;
                    if (enemyTransform != null)
                    {
                        _enemyController = enemyTransform.GetComponent<EnemyController>();
                        timer.EnemyId = (_enemyController != null) ? _enemyController.GetEnemyId() : -1;
                    }
                    else
                    {
                        timer.EnemyId = -1;
                    }
                }
            }

            _timers.Sort((a, b) =>
            {
                if (a.Tag == "Player") return -1;
                if (b.Tag == "Player") return 1;
                return 0;
            });

            for (int i = 0; i < _timers.Count; i++)
            {
                EventSystem.AssignTimerIndex.Invoke(i);
            }
        }

        void Update()
        {
            HandleTimerInput();

            if (_counting)
            {
                StartCountdown();
            }
        }

        void HandleTimerInput(bool endTurnButton = false)
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                _cheat = !_cheat;
                EventSystem.ShowCheatEngine.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Numlock)) //TODO: TEST DELETE PLS
            {
                for (int i = 0; i < _timers.Count; i++)
                {
                    Debug.LogError("Tag " + _timers[i].Tag + "\nEnemyID " + _timers[i].EnemyId + "\nID " + _timers[i].Id + "\nValue " + _timers[i].Value + "\nHp " + _timers[i].HP);
                }
            }
        
            if (_cheat)
            {
                if (Input.GetKeyDown(KeyCode.Comma))
                    ChangeActiveTimer(1);
                if (Input.GetKeyDown(KeyCode.Period))
                    ChangeActiveTimer(-1);
                if (Input.GetKeyDown(KeyCode.Quote))
                    ChangeActiveTimerValue(1);
                if (Input.GetKeyDown(KeyCode.Slash))
                    ChangeActiveTimerValue(-1);
            }

            if (((Input.GetKeyDown(KeyCode.Return) || _createdDeck) || endTurnButton) && _deckController.IsDeckCreated())
            {
                EnteredRoom();
                _createdDeck = false;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                // PlayedAttackCard(); // Nie ma jak przkazać argumentów
            }
        }

        void StartCountdown()
        {
            bool anyTimerReachedZero = false;

            for (int i = 0; i < _timers.Count; i++)
            {
                if (_timers[i].Value == 0)
                {
                    anyTimerReachedZero = true;
                    _counting = false;
                    CalculatePriority();

                    if (_timers[_activeTimerIndex].Tag == "Player")
                    {
                        EventSystem.PlayerMove.Invoke(true);
                        if (_canDraw && CombatMode.isPlayerInCombat)
                        {
                            EventSystem.DrawACard.Invoke();
                        }

                        _canDraw = true;
                    }
                    else
                    {
                        EventSystem.PlayerMove.Invoke(false);
                    }

                    if (_timers[_activeTimerIndex].Tag == "Enemy")
                    {
                        Vector3 movePosition = new Vector3(
                            _player.transform.position.x,
                            _player.transform.position.y,
                            -6
                        );
                        EventSystem.EnemyMove.Invoke(_timers[_activeTimerIndex].EnemyId, movePosition);
                    }
                    else
                    {
                        EventSystem.EnemyMove.Invoke(-1, new Vector3(_player.transform.position.x, _player.transform.position.y, -6));
                    }

                    _turn.text = "Turn: " + _timers[_activeTimerIndex].Tag;

                    return;
                }
            }

            if (!anyTimerReachedZero)
            {
                for (int i = 0; i < _timers.Count; i++)
                {
                    if (_timers[i].Value > 0)
                    {
                        _timers[i].Value--;
                    }
                    _timeToPause = 0.4f;
                }

                UpdateTexts();
                StartCoroutine(AnimationPauseCoroutine());
            }
        }

        IEnumerator AnimationPauseCoroutine()
        {
            yield return new WaitForSeconds(_timeToPause);
        
            if (_timeToPause > 0.01)
            {
                _timeToPause -= 0.01f;
            }
            else
            {
                _timeToPause = 0.01f;
            }

            StartCountdown();
        }

        public void SetTimerValue(int index, int newValue)
        {
            if (index >= 0 && index < _timers.Count)
            {
                _timers[index].Value = Mathf.Clamp(newValue, 0, 99);
            }
            else
            {
                Debug.LogWarning($"SetTimerValue: Index {index} is out of range.");
            }
            UpdateTexts();
        }

        void ChangeActiveTimer(int change)
        {
            _activeTimerIndex += change;
            if (_activeTimerIndex < 0)
            {
                _activeTimerIndex = _timers.Count - 1;
            }
            else if (_activeTimerIndex >= _timers.Count)
            {
                _activeTimerIndex = 0;
            }

            UpdateTexts();
        }

        public void ChangeActiveTimerValue(int change)
        {
            if (_activeTimerIndex >= 0 && _activeTimerIndex < _timers.Count)
            {
                int newValue = _timers[_activeTimerIndex].Value + change;
                _timers[_activeTimerIndex].Value = Mathf.Clamp(newValue, 0, 99);
                UpdateTexts();
            }
            else
            {
                Debug.LogWarning("ChangeActiveTimerValue: Aktywny timer jest poza zakresem.");
            }
        }

        void UpdateTexts()
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                TMP_Text text = _timers[i].Text;
                text.color = (i == _activeTimerIndex && !_counting) ? Color.green : Color.white;
                text.text = _timers[i].Value.ToString();
            }
        }

        void CalculatePriority()
        {
            int highestPriority = -1;
            int highestPriorityIndex = -1;

            for (int i = 0; i < _timers.Count; i++)
            {
                string currentTag = _timers[i].Tag;
                int currentValue = _timers[i].Value;

                if (TagPriority.TryGetValue(currentTag, out int currentPriority))
                {
                    if (currentPriority > highestPriority && currentValue == 0)
                    {
                        highestPriority = currentPriority;
                        highestPriorityIndex = i;
                    }
                }
            }

            if (highestPriorityIndex != -1)
            {
                _activeTimerIndex = highestPriorityIndex;
                UpdateTexts();
            }
            else
            {
                _activeTimerIndex = _timers.FindIndex(timer => timer.Tag == "N/A");
                if (_activeTimerIndex == -1 && _timers.Count > 0)
                {
                    _activeTimerIndex = 0;
                }
                UpdateTexts();
            }
        }

        public void PlayedAttackCard(GameObject hpBarOwner, GameObject hpSliderGameObj)
        {

            var timerData = FindEnemyIDByHpBar(hpSliderGameObj);
            Debug.Log("enemyid: " + timerData);
            Debug.Log("activeTimerIndex: " + _timers[_activeTimerIndex].EnemyId);
            EventSystem.WhatHP.Invoke(hpBarOwner, timerData);
            // EventSystem.WhatHP.Invoke(_timers[_activeTimerIndex].HP, _timers[_activeTimerIndex].EnemyId);
        }

        private int FindEnemyIDByHpBar(GameObject hpBar)
        {
            return _timers.FirstOrDefault(timerData => timerData.HP == hpBar)!.EnemyId;
        }

        void DeleteTimer(int enemyIdToDelete)
        {
            Debug.LogWarning("Wyszedłem: " + enemyIdToDelete);
            for (int i = 0; i < _timers.Count; i++)
            {
                if (enemyIdToDelete == _timers[i].EnemyId) // tu się pierdoli
                {
                    _timers.RemoveAt(i);
                    Debug.LogWarning("Wyszedłem" + enemyIdToDelete);
                }
            }
            
            
            /*
            if (timerToDelete == _activeTimerIndex)
            {
                if (_timers.Count > 0)
                {
                    _activeTimerIndex = Mathf.Clamp(_activeTimerIndex, 0, _timers.Count - 1);
                }
            }
*/
            UpdateTexts();
            //AddToTimer(); WHY??????
        }
        
        public TimerData GetTimerDataByIndex(int index)
        {
            if (index >= 0 && index < _timers.Count)
            {
                return _timers[index];
            }
            return null;
        }

    }


