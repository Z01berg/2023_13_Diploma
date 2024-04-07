using UnityEngine;

/**
 *  Public class Doorway reprezentuje drzwi w grze.
    
    Zawiera informacje o:
    - pozycji drzwi
    - orientacji drzwi
    - prefabrykacie drzwi
    - pozycji początkowej do kopiowania
    - szerokości kafelków do skopiowania
    - wysokości kafelków do skopiowania
    - statusie połączenia drzwi
    - statusie dostępności drzwi
    
    Działa w trybie edytora:
    - umożliwia ustawienie początkowej pozycji do kopiowania kafelków
    - umożliwia ustawienie szerokości i wysokości kafelków do kopiowania
    
    Możliwe akcje:
    - pobranie pozycji drzwi
    - pobranie orientacji drzwi
    - pobranie prefabrykatu drzwi
    - ustawienie początkowej pozycji do kopiowania kafelków
    - ustawienie szerokości i wysokości kafelków do kopiowania
 */

[System.Serializable]

public class Doorway
{
    public Vector2Int Position;
    public Orientation Orientation;
    public GameObject DoorPrefab;

    #region Header

    [Header("The Upper Left Position To Start Copying From")]

    #endregion
    public Vector2Int DoorwayStartCopyPosition;

    #region Header

    [Header("The width of tiles in the doorway to copy over")]

    #endregion
    public int DoorwayCopyTileWidth;
    
    #region Header

    [Header("The height of tiles in the doorway to copy over")]

    #endregion
    public int DoorwayCopyTileHeight;

    [HideInInspector] public bool IsConnected = false;
    [HideInInspector] public bool IsUnavailable = false;
}