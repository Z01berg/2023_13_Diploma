using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static DefaultInputs;

/**
 * Publiczna klasa ktorej metody sa wolane przez input system kiedy okreslone przyciski sa naciskane.
 * Klasa zawiera metody:
 *  - PrepareInputs aktywuje klase DefaultMausenKeys
 *  - OnMove porusza graczem
 *  - OnMoveCancelled zatrzymuje gracza
 *  - OnMenu otwiera i zamyka menu pauzy
 *  - OnEquipment otwiera i zamyka ekwipunek
 *  - OnDoubleClick przemieszcza itemy kiedy zostana nacisniete dwa razy
 *  - OnHelp otwiera i zamyka okno pomocy
 *  - OnMap otwiera i zamyka mape
 *  Karzda funkcja zarzadzajaca otwieraniem i zamykaniem okien ma soj odpowiednik wolany kiedy przycisk aktywujacy dana metode zostanie zwolniony.
 */

public class PlayerInputsController : MonoBehaviour, IDefaultMausenKeysActions
{

    private DefaultInputs _controls;
    private GraphicRaycaster _raycaster;
    private bool _buttonPressed;

    private void Start()
    {
        _buttonPressed = false;
        _raycaster = GameObject.Find("UI").GetComponent<GraphicRaycaster>();
    }
    private void OnEnable()
    {
        PrepareInputs();
        _controls.DefaultMausenKeys.Move.performed += OnMove;
        _controls.DefaultMausenKeys.Equipment.performed += OnEquipment;
        _controls.DefaultMausenKeys.Equipment.canceled += OnEquipmentCanceled;
        _controls.DefaultMausenKeys.Menu.performed += OnMenu;
        _controls.DefaultMausenKeys.Menu.canceled += OnMenuCanceled;
        _controls.DefaultMausenKeys.DoubleClick.performed += OnDoubleClick;
        _controls.DefaultMausenKeys.Move.canceled += OnMoveCancelled;
        _controls.DefaultMausenKeys.Help.performed += OnHelp;
        _controls.DefaultMausenKeys.Map.performed += OnMap;
        _controls.DefaultMausenKeys.Map.canceled += OnMapCanceled;
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
        _controls.DefaultMausenKeys.Map.performed -= OnMap;
        _controls.DefaultMausenKeys.Equipment.canceled -= OnEquipmentCanceled;
        _controls.DefaultMausenKeys.Menu.canceled -= OnMenuCanceled;
        _controls.DefaultMausenKeys.Map.canceled -= OnMapCanceled;
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
        if (_buttonPressed) return;
        _buttonPressed = true;
        EventSystem.OpenClosePauseMenu?.Invoke();
    }
    public void OnMenuCanceled(InputAction.CallbackContext context)
    {
        _buttonPressed = false;
    }
    public void OnEquipment(InputAction.CallbackContext context)
    {
        if (_buttonPressed) return;
        _buttonPressed = true;
        EventSystem.OpenCloseInventory?.Invoke();
    }
    public void OnEquipmentCanceled(InputAction.CallbackContext context)
    {
        _buttonPressed = false;
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
        EventSystem.ShowHelpSheet?.Invoke();
    }

    public void OnMap(InputAction.CallbackContext context)
    {
        if (_buttonPressed) return;
        _buttonPressed = true;
        EventSystem.OpenMap?.Invoke();
    }
    public void OnMapCanceled(InputAction.CallbackContext context)
    {
        _buttonPressed = false;
    }
}
