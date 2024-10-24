using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    private static string _saveGameFilePath = Application.persistentDataPath + "/Saves/save.sav";
    public static void SaveGame()
    {
        FileStream file;
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        if (File.Exists(_saveGameFilePath)) 
        { 
            file = File.OpenWrite(_saveGameFilePath);
        } 
        else
        {
            file = File.Create(_saveGameFilePath);
        }
            
        SaveTemplate saveTemplate = new SaveTemplate();

        file.Close();

        foreach(var item in Inventory.Instance?.items)
        {
            saveTemplate.inventory.Add(item.itemName);
        }

        string save = JsonUtility.ToJson(saveTemplate,true);

        System.IO.File.WriteAllText(_saveGameFilePath, save);
        Debug.Log("game saved");
    }

    public static void LoadGame() 
    {
        var text = File.ReadAllText(_saveGameFilePath);
        var data = JsonUtility.FromJson<SaveTemplate>(text);

        AddressablesUtilities.ItemsWithNames(data.inventory);
        Debug.Log("game loaded");
    }

    public static void NewGame()
    {
        FileStream file;
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        if (File.Exists(_saveGameFilePath))
        {
            file = File.OpenWrite(_saveGameFilePath);
        }
        else
        {
            file = File.Create(_saveGameFilePath);
        }

        SaveTemplate saveTemplate = new SaveTemplate();

        file.Close();

        string save = JsonUtility.ToJson(saveTemplate, true);

        System.IO.File.WriteAllText(_saveGameFilePath, save);
        Debug.Log("game newed");
    }

    public static bool CheckIfSaveExists()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            return false;
        }
        if (!File.Exists(_saveGameFilePath))
        {
            return false;
        }

        return true;
    }
}
