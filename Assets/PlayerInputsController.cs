using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultInputs;

public class PlayerInputsController : MonoBehaviour, IDefaultMausenKeysActions
{

    private DefaultInputs _controls;
    private void OnEnable()
    {
        PrepareInputs();
        _controls.DefaultMausenKeys.Move.performed += OnMove;
        _controls.DefaultMausenKeys.Equipment.performed += OnEquipment;
        _controls.DefaultMausenKeys.Menu.performed += OnMenu;
    }

    private void OnDisable()
    {
        _controls.Disable();
        _controls.DefaultMausenKeys.Move.performed -= OnMove;
        _controls.DefaultMausenKeys.Equipment.performed -= OnEquipment;
        _controls.DefaultMausenKeys.Menu.performed -= OnMenu;
    }

    private void PrepareInputs()
    {
        _controls = new DefaultInputs();
        _controls.DefaultMausenKeys.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        EventSystem.MovePlayer?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        EventSystem.OpenClosePauseMenu?.Invoke();
    }

    public void OnEquipment(InputAction.CallbackContext context)
    {
        EventSystem.OpenCloseInventory?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        Debug.Log("Watcha got there. Not implemented action? Would be a shame if someone implemented it.");
#endif
    }
}
