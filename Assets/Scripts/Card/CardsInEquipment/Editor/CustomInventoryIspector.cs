
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * Klasy publiczne zmieniaj¹ce wyglad inspektora unity objektu JsonCardsLoader.
 * 
 * CustomJsonImportInspector - dodaje przycisk po ktorego nacisnieciu odpala sie 
 * funkcja importujaca karty.
 * 
 * CustomJsonExportInspector - dodaje przycisk po nacisnieciu ktorego odpalana jest funkcja
 * zapisujaca utworzona karte do pliku Json
 */

[CustomEditor(typeof(Inventory))]
public class CustomInventoryIspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Inventory ds = (Inventory)target;

        if (GUILayout.Button("AddItem"))
        {
            AddressablesUtilities.GetRandomItem();
            
        }
        if (GUILayout.Button("SaveItems"))
        {
            SaveSystem.SaveGame();
        }
        if (GUILayout.Button("LockItems"))
        {
            SaveSystem.NewGame();
        }
        if (GUILayout.Button("LoadItems"))
        {
            SaveSystem.LoadGame();
        }
    }
}
