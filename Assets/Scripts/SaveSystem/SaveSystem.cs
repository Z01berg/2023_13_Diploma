using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/**
 * Publiczna klasa zapisujaca i wczytujaca postepy gracza. Plik zapisywany jest w formacie json.
 * Klasa zawiera funkcje:
 *  - SaveGame ktora zapisuje gre
 *  - LoadGame ktora wczytuje gre
 *  - NewGame ktora resetuje postepy gracza
 *  - CheckIfSaveExists ktora sprawdza czy istnieje plik z zapisem. Funkcja uzywana przez 
 *    main menu do sprawdzenia czy powinien zostac wyswietlony przycisk "continue"
 */

public static class SaveSystem
{
    private static string _saveGameFilePath = Application.persistentDataPath + "/Saves/save.sav";
    public static void SaveGame()
    {
        FileStream file;
        // czy istnieje folder i stworz go jesli nie
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        // czy istnieje plik json i stworz go jesli nie
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

        var eq = Equipment.Instance;

        if (eq.leftHand != null) saveTemplate.leftHand = eq.leftHand.itemName;
        if (eq.rightHand != null) saveTemplate.rightHand = eq.rightHand.itemName;
        if (eq.chest != null) saveTemplate.chest = eq.chest.itemName;
        if (eq.boots != null) saveTemplate.boots = eq.boots.itemName;
        if (eq.head != null) saveTemplate.head = eq.head.itemName;
        if (eq.legs != null) saveTemplate.legs = eq.legs.itemName;
        if (eq.item1 != null) saveTemplate.item1 = eq.item1.itemName;
        if (eq.item2 != null) saveTemplate.item2 = eq.item2.itemName;
        if (eq.item3 != null) saveTemplate.item3 = eq.item3.itemName;
        if (eq.item4 != null) saveTemplate.item4 = eq.item4.itemName;
        if (eq.item5 != null) saveTemplate.item5 = eq.item5.itemName;
        if (eq.item6 != null) saveTemplate.item6 = eq.item6.itemName;

        string save = JsonUtility.ToJson(saveTemplate,true);

        System.IO.File.WriteAllText(_saveGameFilePath, save);
        Debug.Log("game saved");
    }

    public static void LoadGame() 
    {
        var text = File.ReadAllText(_saveGameFilePath);
        var data = JsonUtility.FromJson<SaveTemplate>(text);

        // wloz przedmioty do inventarza i ekwipunku
        AddressablesUtilities.ItemsWithNames(data);
    }

    public static void NewGame()
    {
        FileStream file;
        // czy istnieje folder i stworz go jesli nie
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        // czy istnieje plik json i stworz go jesli nie
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
    }

    public static bool CheckIfSaveExists()
    {
        // czy istnieje folder
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            return false;
        }
        // czy istnieje plik json
        if (!File.Exists(_saveGameFilePath))
        {
            return false;
        }

        return true;
    }
}
