using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseView;
    [SerializeField] private GameObject _gameOverView;
    private static bool _menuOpen = false;
    private bool _gameOver = false;

    void Start()
    {
        EventSystem.OpenClosePauseMenu.AddListener(ChangePauseMenuState);
        _pauseView.SetActive(false);
        _gameOverView.SetActive(false);

    }

    private void ChangePauseMenuState()
    {
        if (_gameOver)
        {
            return;
        }

        _menuOpen = !_menuOpen;
        _pauseView.SetActive(_menuOpen);
        EventSystem.HideHand?.Invoke(_menuOpen);
        if (_menuOpen)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ClosePause()
    {
        EventSystem.HideHand?.Invoke(false);
        _menuOpen = false;
        _pauseView.SetActive(_menuOpen);
        Time.timeScale = 1;
    }

    public void LeaveToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void LeaveGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GameOver()
    {
        _gameOver = true;
        _gameOverView.SetActive(true);
    }

    public static bool getMenuOpen()
    {
        return _menuOpen;
    }
}
