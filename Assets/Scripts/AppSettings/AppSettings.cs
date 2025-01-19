using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Publiczna klasa sluzaca do zapisu ustawien do pliku .json. Klasa jest latwo serializowana.
 */

public class AppSettings
{
    public int qualitySettingPreference;
    public int resolutionPreference;
    public int textureQualityPreference;
    public int antiAliasingPreference;
    public bool fullscreenPreference;
    public float volumePreference;
}
