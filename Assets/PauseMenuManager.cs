using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseView;
    private bool _menuOpen = false;

    void Start()
    {
        _pauseView.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _menuOpen = !_menuOpen;
            _pauseView.SetActive(_menuOpen);
            if(_menuOpen )
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void ClosePause()
    {
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
}
