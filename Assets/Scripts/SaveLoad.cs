using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;

public static class SaveLoad {

    public static PlayerData savedGame;

    public static bool wasLoaded = false;

    public static bool positionAndSceneLoaded = false;
    public static bool inventoryLoaded = false;
    public static bool equipmentLoaded = false;
    public static bool storageLoaded = false;
    public static bool playerStatsLoaded = false;
    public static bool playerQuestLoaded = false;
    public static bool sceneStateLoaded = false;

    public static void SaveAsJson()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SentPlayerData>().SentData();
        string json = JsonUtility.ToJson(PlayerData.data);
        File.WriteAllText(Application.persistentDataPath + "/SavedGame.orc", json);
    }

    public static void LoadAsJson()
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath + "/SavedGame.orc");
        savedGame = JsonUtility.FromJson<PlayerData>(jsonString);
        
        wasLoaded = true;
        positionAndSceneLoaded = true;
        inventoryLoaded = true;
        equipmentLoaded = true;
        storageLoaded = true;
        playerStatsLoaded = true;
        playerQuestLoaded = true;
        sceneStateLoaded = true;
    }
}
