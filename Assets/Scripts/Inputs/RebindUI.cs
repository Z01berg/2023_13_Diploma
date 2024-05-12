using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindUI : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputActionReference;
    [Range(0f, 10f)][SerializeField] private int _selectedBinding;
    [SerializeField] private InputBinding.DisplayStringOptions _displayStringOptions;
    [Header("Binding Info")]
    [SerializeField] private InputBinding _inputBinding;
    private int _bindingIndex;
    private string _actionName;
    [Header("UI Fields")]
    [SerializeField] private TMP_Text _actionText;
    [SerializeField] private Button _rebindButton;
    [SerializeField] private TMP_Text _rebindText;
    [SerializeField] private Button _resetButton;

    private void OnEnable()
    {
        _rebindButton.onClick.AddListener(() => Rebind());
        _resetButton.onClick.AddListener(() => ResetBinding());

        if(_inputActionReference != null)
        {
            InputManager.LoadBindingOverride(_actionName);
            GetBindingInfo();
            UpdateUI();
        }

        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
    }

    private void OnDisable()
    {
        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCanceled -= UpdateUI;
    }

    private void OnValidate()
    {
        if(_inputActionReference == null)
        {
            return;
        }
        GetBindingInfo();
        UpdateUI();
    }

    private void GetBindingInfo()
    {
        if(_inputActionReference.action != null)
        {
            _actionName = _inputActionReference.action.name;
        }

        if(_inputActionReference.action.bindings.Count > _selectedBinding)
        {
            _inputBinding = _inputActionReference.action.bindings[_selectedBinding];
            _bindingIndex = _selectedBinding;
        }
    }

    private void UpdateUI()
    {
        if(_actionText == null)
        {
            _actionText.text = _actionName;
        }

        if(_rebindText != null)
        {
            if(Application.isPlaying)
            {
                _rebindText.text = InputManager.GetBindingName(_actionName, _bindingIndex);
            }
            else
            {
                _rebindText.text = _inputActionReference.action.GetBindingDisplayString(_bindingIndex);
            }
        }
    }

    private void Rebind()
    {
        InputManager.StartRebindProcess(_actionName, _bindingIndex, _rebindText);
    }
    private void ResetBinding()
    {
        InputManager.ResetBindings(_actionName, _bindingIndex);
        UpdateUI();
    }
}
