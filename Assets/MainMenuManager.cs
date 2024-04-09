using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuView;
    [SerializeField] private GameObject _compendiumView;
    [SerializeField] private GameObject _settingsView;
    [SerializeField] private GameObject _creditsView;
    [SerializeField] private GameObject _optionsView;
    [SerializeField] private GameObject _keysView;

    private void Start()
    {
        _mainMenuView.SetActive(true);
        _settingsView.SetActive(false);
        _creditsView.SetActive(false);
        _compendiumView.SetActive(false);
    }

    public void OpenSettings()
    {
        _settingsView.SetActive(true);
        _mainMenuView.SetActive(false);
    }

    public void CloseSettings()
    {
        _settingsView.SetActive(false);
        _mainMenuView.SetActive(true);
    }

    public void OpenCredits()
    {
        _creditsView.SetActive(true);
        _mainMenuView.SetActive(false);
    }

    public void CloseCredits()
    {
        _creditsView.SetActive(false);
        _mainMenuView.SetActive(true);
    }

    public void OpenCompendium()
    {
        _compendiumView.SetActive(true);
        _mainMenuView.SetActive(false);
    }

    public void CloseCompendium()
    {
        _compendiumView.SetActive(false);
        _mainMenuView.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenOptions()
    {
        _optionsView.transform.SetAsLastSibling();
    }

    public void OpenKeys()
    {
        _keysView.transform.SetAsLastSibling();
    }
}
