using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Windows;

/**
 * Publiczna klasa pozwalajaca na zmiane roznych ustawien aplikacji np. rozdzielczosc. 
 * Ustawienia sa zapisywane w pliku .json wewnatrz StreammingAssets
 * Klasa zawiera kilka publicznych metod:
 *  - SetVolume ustawia poziom glosnosci dzwiekow gry
 *  - SetFullscreen zmienia tryb miedzy fullscreen i windowed
 *  - SetResolution zmienia rozdzielczosc
 *  - SetTextureQuality zmienia jakosc tekstur
 *  - SetAntiAliasing zmienia ustawienia antyaliasingu
 *  - SetQuality zmienia ustawienia jakosci grafiki
 *  - SaveSettings zapisuje wartosci ustaiwen do pliku .json
 *  - LoadSettings laduja wartosci ustawien z pliku .json
 */

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] private TMP_Dropdown _textureDropdown;
    [SerializeField] private TMP_Dropdown _antiAliasingDropdown;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private float _currentVolume;
    [SerializeField] private Resolution[] _resolutions;
    [SerializeField] private Toggle _fullscreenToggle;

    [SerializeField] private AppSettings _appSettings;
    private void Start()
    {

        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        // pobieranie wszystkich mozliwych rozdzielczosci
        _resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            // przerabianie opcji na stringi
            string option = _resolutions[i].width + " x " + _resolutions[i].height + " " + math.round(_resolutions[i].refreshRateRatio.value) + "hz";
            options.Add(option);

            // ustawianie rozdzielczosci na poczatku gry na aktualnie zapisana
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();

        // pobieranie wszystkich mozliwych jakosci
        var qualNames = QualitySettings.names;

        _qualityDropdown.ClearOptions();
        _qualityDropdown.AddOptions(qualNames.ToList());

        string[] antiAliasingModes = { "Disabled", "2x MSAA", "4x MSAA", "8x MSAA" };
        _antiAliasingDropdown.ClearOptions();
        _antiAliasingDropdown.AddOptions(antiAliasingModes.ToList());

        LoadSettings(currentResolutionIndex);
    }

    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat("Volume", volume);
        _currentVolume = volume;
    }


    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Application.targetFrameRate = Convert.ToInt32(math.round(resolution.refreshRateRatio.value));
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.globalTextureMipmapLimit = textureIndex;
        _qualityDropdown.value = 6;
    }
    public void SetAntiAliasing(int antiAliasingIndex)
    {
        QualitySettings.antiAliasing = antiAliasingIndex;
        _qualityDropdown.value = 6;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SaveSettings()
    {
        _appSettings.qualitySettingPreference = _qualityDropdown.value;
        _appSettings.resolutionPreference = _resolutionDropdown.value;
        _appSettings.textureQualityPreference = _textureDropdown.value;
        _appSettings.antiAliasingPreference = _antiAliasingDropdown.value;
        _appSettings.fullscreenPreference = Screen.fullScreen;
        _appSettings.volumePreference = _currentVolume;

        string json = JsonUtility.ToJson(_appSettings, true);

        System.IO.File.WriteAllText(Application.streamingAssetsPath + "/AppSettings.txt", json);
        
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (System.IO.File.Exists(Application.streamingAssetsPath + "/AppSettings.txt"))
        {
            _appSettings = JsonUtility.FromJson<AppSettings>(System.IO.File.ReadAllText(Application.streamingAssetsPath + "/AppSettings.txt"));

        }
        else
        {
            _appSettings = new AppSettings
            {
                qualitySettingPreference = 5,
                resolutionPreference = currentResolutionIndex,
                textureQualityPreference = 0,
                antiAliasingPreference = 1,
                fullscreenPreference = true,
                volumePreference = 0.5f
            };
        }
        _resolutionDropdown.value = _appSettings.resolutionPreference;
        _textureDropdown.value = _appSettings.textureQualityPreference;
        _antiAliasingDropdown.value = _appSettings.antiAliasingPreference;
        _volumeSlider.value = _appSettings.volumePreference;
        _currentVolume = _appSettings.volumePreference;
        Screen.fullScreen = _appSettings.fullscreenPreference;
        _fullscreenToggle.isOn = _appSettings.fullscreenPreference;

    }
}
