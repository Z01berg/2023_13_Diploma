using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Publiczna clasa HealthBar jest klasą która robi wszystkie działanie na HP
 *
 * ma w sobie informacje o:
 * - znaczenie slidera
 * - tekst na sliderze
 * - do kogo jest podpięty ten HpBar
 * - dostęp do gameObjectu podpiętego objektu
 *
 * Kiedy HealthBar dostaje informacje od eventu że jest urażony to sprawdza czy jest to właściwy obiekt
 * - jeżeli nie to nic nie robi
 * - jeżeli tak to zmienia _currentObject = true
 *
 * Jeżeli _currentObject to można
 * - zmienić health public ChangeHealth
 * - zabić przeciwnika Kill() jeżeli health < 0
 */

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private GameObject _body;
    
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _data;
    private string[] _parts;
    private int _maxValue = 0;
    private int _value = 0;

    private bool _switch = false;
    private bool _currentObject = false;
    private int _timerNumbToDelete;

    public int myTimerIndex = -2;

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
            
            _currentObject = !_currentObject;
            _switch = false;
        }
    }

    private void Split()
    {
        _parts = (_data.text).Split('/');
    }

    private void SetData()
    {
        _maxValue = int.Parse(_parts[1]);
        _value = int.Parse(_parts[0]);
    }

    private void UpdateHealth()
    {
        _value = int.Parse(_parts[0]);
    }

    public void ChangeHealth(int health)
    {
        if (_value + health > _maxValue)
        {
            _value = _maxValue;
        }
        else
        {
            _value += health;
        }
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        _data.text = _value + " / " + _maxValue;
    }

    private void Kill()
    {
        if (_value <= 0)
        {
            room.enemyInRoomList.Remove(this.gameObject);
            EventSystem.DeleteReference.Invoke(myTimerIndex);
            Destroy(_gameObject);
            Destroy(_body);
            Debug.Log("killed: " +myTimerIndex);
        }
    }
    //TODO: Sprawdzic czy ktos z tego korzysta
    private void HandleWhatHP(GameObject recieved, int timerNumber)
    {
        if (recieved == _gameObject)
        {
            _currentObject = true;
        }

        _switch = true;
        _timerNumbToDelete = timerNumber;
    }

    public int getHealth()
    {
        return _value;
    }

    private void SetTimerIndex(int value)
    {
        if (myTimerIndex != -2)
        {
            myTimerIndex = value;
        }
        
    }
}