using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * A class that lets you visualize imported, or exported cards in UNITY inspector.
 */

[CustomEditor(typeof(Deserialization))]
public class CustomJsonImportIspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Deserialization ds = (Deserialization)target;

        if (GUILayout.Button("IMPORT"))
        {
            ds.Import();
        }
    }
}

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