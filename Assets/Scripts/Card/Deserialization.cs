using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Deserialization : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private JsonCards objects;
    string Path = "Assets/ScriptableObjectAssets/Card/";
    void Start()
    {
        objects = JsonUtility.FromJson<JsonCards>(jsonFile.text);

        foreach (var obj in objects.attackCardsList)
        {
            var s = ScriptableObject.CreateInstance<CardsSO>();

            s.id = obj.id;
            s.isActive = obj.isActive;
            s.cardQuality = obj.cardQuality;
            s.title = obj.title;
            s.description = obj.description;
            s.cost = obj.cost;
            s.damage = obj.damage;
            s.move = obj.move;
            s.backgroundPath = obj.backgroundPath;
            s.spritePath = obj.spritePath;

            AssetDatabase.CreateAsset(s, Path + s.title + ".asset");
            AssetDatabase.SaveAssets();
        }
    }

}
