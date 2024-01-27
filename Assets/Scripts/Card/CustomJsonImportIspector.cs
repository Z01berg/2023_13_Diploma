using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
