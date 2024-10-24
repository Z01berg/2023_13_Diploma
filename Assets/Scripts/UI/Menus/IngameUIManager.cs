using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IngameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _inv;
    [SerializeField] private GameObject _minimap;
    [SerializeField] private GameObject _menuView;

    [SerializeField] private GameObject _pauseView;
    [SerializeField] private GameObject _gameOverView;
    private Animator _menuMenuAnimator;

    [HideInInspector] public static bool menuOpen = false;
    private bool _gameOver = false;

    private bool _locked = false;


    private void Start()
    {
        _menuMenuAnimator = _menuView.GetComponent<Animator>();

        EventSystem.OpenClosePauseMenu.AddListener(ChangePauseMenuState);
        EventSystem.OpenCloseInventory.AddListener(ChangeInventoryState);

        _minimap.SetActive(true);

        _pauseView.SetActive(false);
        _gameOverView.SetActive(false);
        _inv.SetActive(false);
        _menuView.SetActive(false);
    }

    public void OpenInventory()
    {
        if (_locked) return;
        if (_inv.activeSelf) return;
        _minimap.SetActive(false);
        menuOpen = true;
        _menuView.SetActive(true);
        _pauseView.SetActive(false);
        _menuMenuAnimator.SetTrigger("Inventory");
    }

    public void CloseMenu()
    {
        if (_locked) return;
        menuOpen = false;
        if (_inv.activeSelf)
        {
            _inv.GetComponent<UIInventory>().SaveState();
            _inv.SetActive(false);
        }
        _pauseView.SetActive(false);
        Time.timeScale = 1;
        EventSystem.HideHand?.Invoke(false);
        _menuMenuAnimator.SetTrigger("Close");
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OpenPause()
    {
        if (_locked) return;
        if (_pauseView.activeSelf) return;
        _minimap.SetActive(false);
        menuOpen = true;
        _menuView.SetActive(true);
        _inv.SetActive(false);
        _menuMenuAnimator.SetTrigger("Quit");
    }

    private void ChangeInventoryState()
    {
        if (_locked) return;
        if (!_inv.activeSelf)
        {
            Time.timeScale = 0;
            EventSystem.HideHand?.Invoke(true);
            OpenInventory();
            return;
        }
        CloseMenu();
    }

    private void ChangePauseMenuState()
    {
        if (_locked) return;
        if (!_pauseView.activeSelf)
        {
            Time.timeScale = 0;
            EventSystem.HideHand?.Invoke(true);
            OpenPause();
            return;
        }
        CloseMenu();
    }

    public void ChangeInventoryVisible(bool visible = true)
    {
        _inv.SetActive(visible);
    }

    public void SetMenuInvisible()
    {
        _minimap.SetActive(true);
        _menuView.SetActive(false);
    }

    public void ChangePauseVisible(bool visible = true)
    {
        _pauseView.SetActive(visible);
    }
}
