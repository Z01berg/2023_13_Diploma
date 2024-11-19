using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Public class HealthBar handles all HP-related actions.
 *
 * It contains information about:
 * - The slider value
 * - The text on the slider
 * - The object this HP bar is linked to
 * - Access to the linked GameObject
 *
 * When HealthBar receives damage information from an event, it checks if it's the correct object.
 * - If not, it does nothing
 * - If yes, it sets _currentObject to true
 *
 * If _currentObject is true, you can:
 * - Change health using ChangeHealth
 * - Kill the enemy using Kill() if health < 0
 */
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _data;
    [SerializeField] private GameObject _body;

    private int _maxValue = 0;
    private int _value = 0;

    private bool _switch = false;
    private bool _currentObject = false;
    private int _timerNumbToDelete = 99;

    public int myTimerIndex;

    [HideInInspector] public InstantiatedRoom room;

    private void Start()
    {
        Split();
        SetData();
        UpdateHealthText();
        EventSystem.AssignTimerIndex.AddListener(SetTimerIndex);
        EventSystem.WhatHP.AddListener(HandleWhatHP);
    }

    void Update()
    {
        Kill();

        if (_switch && _currentObject)
        {
            ChangeHealth(-1);
            UpdateHealthText();

            _currentObject = false;
            _switch = false;
        }
    }

    private void Split()
    {
        string[] parts = _data.text.Split('/');
        if (parts.Length >= 2)
        {
            _value = int.Parse(parts[0]);
            _maxValue = int.Parse(parts[1]);
        }
        else
        {
            Debug.LogError("HealthBar data format incorrect. Expected 'current/max'.");
        }

        _slider.maxValue = _maxValue;
        _slider.value = _value;
    }

    private void SetData()
    {
        _slider.maxValue = _maxValue;
        _slider.value = _value;
    }

    private void UpdateHealth()
    {
        _slider.value = _value;
    }

    public void ChangeHealth(int health)
    {
        _value += health;
        _value = Mathf.Clamp(_value, 0, _maxValue);
        _slider.value = _value;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        _data.text = $"{_value}/{_maxValue}";
    }

    private void Kill()
    {
        if (_value <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                EventSystem.OpenGameover?.Invoke();
            }

            Debug.Log("killed: " + myTimerIndex);
        
            if (room != null)
            {
                room.enemyInRoomList.Remove(this.gameObject);
            }
            
            EventSystem.DeleteReference.Invoke(_timerNumbToDelete);

            Destroy(_body);
        }
    }


    private void HandleWhatHP(GameObject received, int timerNumber)
    {
        if (received == gameObject)
        {
            _currentObject = true;
            _timerNumbToDelete = timerNumber;
        }

        _switch = true;
    }

    public int GetHealth()
    {
        return _value;
    }

    private void SetTimerIndex(int value)
    {
        if (myTimerIndex == -1) 
        {
            myTimerIndex = value;
            Debug.Log($"Timer index set: {myTimerIndex}");
        }
    }

}
