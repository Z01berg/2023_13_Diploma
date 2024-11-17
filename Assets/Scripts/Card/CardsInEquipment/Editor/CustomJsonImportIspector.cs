
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

//[CustomEditor(typeof(Deserialization))]
public class CustomJsonImportIspector : Editor
{/*
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Deserialization ds = (Deserialization)target;

        if (GUILayout.Button("IMPORT"))
        {
            ds.Import();
        }
    }*/
}
/*
[CustomEditor(typeof(CardCreator))]
public class CustomJsonCardExportInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardCreator cc = (CardCreator)target;

        if (GUILayout.Button("Export"))
        {
            cc.CreateCard();
        }
        
    }
    
}
*/