using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static DefaultInputs;

public class PlayerInputsController : MonoBehaviour, IDefaultMausenKeysActions
{

    private DefaultInputs _controls;
    private GraphicRaycaster _raycaster;

    private void Start()
    {
        _raycaster = GameObject.Find("UI").GetComponent<GraphicRaycaster>();
    }
    private void OnEnable()
    {
        PrepareInputs();
        _controls.DefaultMausenKeys.Move.performed += OnMove;
        _controls.DefaultMausenKeys.Equipment.performed += OnEquipment;
        _controls.DefaultMausenKeys.Menu.performed += OnMenu;
        _controls.DefaultMausenKeys.DoubleClick.performed += OnDoubleClick;
        _controls.DefaultMausenKeys.Move.canceled += OnMoveCancelled;
        _controls.DefaultMausenKeys.Help.performed += OnHelp;
    }

    private void OnDisable()
    {
        _controls.Disable();
        _controls.DefaultMausenKeys.Move.performed -= OnMove;
        _controls.DefaultMausenKeys.Equipment.performed -= OnEquipment;
        _controls.DefaultMausenKeys.Menu.performed -= OnMenu;
        _controls.DefaultMausenKeys.DoubleClick.performed -= OnDoubleClick;
        _controls.DefaultMausenKeys.Move.canceled -= OnMoveCancelled;
        _controls.DefaultMausenKeys.Help.performed -= OnHelp;
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

    public void OnMoveCancelled(InputAction.CallbackContext context)
    {
        EventSystem.MovePlayer?.Invoke(Vector2.zero);
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        EventSystem.OpenClosePauseMenu?.Invoke();
        Inventory.Instance?.items.Add(AddressablesUtilities.GetRandomItem());
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

    public void OnDoubleClick(InputAction.CallbackContext context)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData d = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        d.position = Mouse.current.position.ReadValue();
        _raycaster.Raycast(d, results);
        foreach (RaycastResult r in results)
        {
            if(r.gameObject.CompareTag("ItemSlot"))
            {
                r.gameObject.GetComponent<ItemSlot>().DoubleClicked();
                break;
            }
        }
    }

    public void OnHelp(InputAction.CallbackContext context)
    {
        AddressablesUtilities.LockAllItems();
        EventSystem.ShowHelpSheet?.Invoke();
    }
}
