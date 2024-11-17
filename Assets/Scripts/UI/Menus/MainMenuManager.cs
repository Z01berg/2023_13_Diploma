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
    [SerializeField] private GameObject _continueButton;

    private bool _levelLoading = false;

    private void Start()
    {
        if (!SaveSystem.CheckIfSaveExists())
        {
            _continueButton.SetActive(false);
        }
        _mainMenuView.SetActive(true);
        _settingsView.SetActive(false);
        _creditsView.SetActive(false);
        _compendiumView.SetActive(false);
    }

    public void OpenSettings()
    {
        if(_levelLoading) return;
        _settingsView.SetActive(true);
        _mainMenuView.SetActive(false);
    }

    public void CloseSettings()
    {
        if (_levelLoading) return;
        _settingsView.SetActive(false);
        _mainMenuView.SetActive(true);
    }

    public void OpenCredits()
    {
        if (_levelLoading) return;
        _creditsView.SetActive(true);
        _mainMenuView.SetActive(false);
    }

    public void CloseCredits()
    {
        if (_levelLoading) return;
        _creditsView.SetActive(false);
        _mainMenuView.SetActive(true);
    }

    public void OpenCompendium()
    {
        if (_levelLoading) return;
        _compendiumView.SetActive(true);
        _mainMenuView.SetActive(false);
    }

    public void CloseCompendium()
    {
        if (_levelLoading) return;
        _compendiumView.SetActive(false);
        _mainMenuView.SetActive(true);
    }

    public void QuitGame()
    {
        if (_levelLoading) return;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ContinueGame()
    {
        if (_levelLoading) return;

        StartCoroutine(ContinueGameC());
    }

    public IEnumerator ContinueGameC()
    {
        _levelLoading = true;
        var asyncLevelLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLevelLoad.isDone)
        {
            yield return null;
        }
    }

    public void StartGame()
    {
        if (_levelLoading) return;

        StartCoroutine(NewGame());
    }
    public IEnumerator NewGame()
    {
        _levelLoading = true;
        SaveSystem.NewGame();
        var asyncLevelLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLevelLoad.isDone)
        {
            yield return null;
        }
    }
    public void OpenOptions()
    {
        if (_levelLoading) return;
        _optionsView.transform.SetAsLastSibling();
    }

    public void OpenKeys()
    {
        if (_levelLoading) return;
        _keysView.transform.SetAsLastSibling();
    }
}
