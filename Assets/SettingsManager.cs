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
        _resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        //_resolutions = _resolutions.Distinct().ToArray();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height + " " + math.round(_resolutions[i].refreshRateRatio.value) + "hz";
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        //options = options.Distinct().ToList();

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();

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
        /*
        if (qualityIndex != 6) // if the user is not using 
                               //any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);
        switch (qualityIndex)
        {
            case 0: // quality level - very low
                _textureDropdown.value = 3;
                _antiAliasingDropdown.value = 0;
                break;
            case 1: // quality level - low
                _textureDropdown.value = 2;
                _antiAliasingDropdown.value = 0;
                break;
            case 2: // quality level - medium
                _textureDropdown.value = 1;
                _antiAliasingDropdown.value = 0;
                break;
            case 3: // quality level - high
                _textureDropdown.value = 0;
                _antiAliasingDropdown.value = 0;
                break;
            case 4: // quality level - very high
                _textureDropdown.value = 0;
                _antiAliasingDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                _textureDropdown.value = 0;
                _antiAliasingDropdown.value = 2;
                break;
        }

        _qualityDropdown.value = qualityIndex;
        */
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
