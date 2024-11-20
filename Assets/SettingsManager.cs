using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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

    private void Start()
    {
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        _resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();
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

    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference",
                   _qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference",
                   _resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference",
                   _textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference",
                   _antiAliasingDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference",
                   Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference",
                   _currentVolume);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
        { 
            _qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        }
        else
        {
            _qualityDropdown.value = 3;
        }

        if (PlayerPrefs.HasKey("ResolutionPreference"))
        {
            _resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        }
        else
        {
            _resolutionDropdown.value = currentResolutionIndex;
        }

        if (PlayerPrefs.HasKey("TextureQualityPreference"))
        {
            _textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        }
        else
        {
            _textureDropdown.value = 0;
        }

        if (PlayerPrefs.HasKey("AntiAliasingPreference"))
        {
            _antiAliasingDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
        }
        else
        {
            _antiAliasingDropdown.value = 1;
        }

        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        }
        else
        {
            Screen.fullScreen = true;
        }

        if (PlayerPrefs.HasKey("VolumePreference"))
        {
            _volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        }
        else
        {
            _volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        }
    }
}
