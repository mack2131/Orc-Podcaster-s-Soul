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

public class SentPlayerData : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (SaveLoad.positionAndSceneLoaded == false &&
            SaveLoad.inventoryLoaded == false &&
            SaveLoad.equipmentLoaded == false &&
            /*SaveLoad.storageLoaded == false &&*/
            /*SaveLoad.sceneStateLoaded == false &&*/
            SaveLoad.playerQuestLoaded == false)
            SaveLoad.wasLoaded = false;
	}

    public void SentData()
    {
        PlayerData.data.PLAYERPOSITION = transform.position;
        PlayerData.data.CURRENTSCENE = SceneManager.GetActiveScene().name;

        PlayerData.data.HEALTH = GetComponent<Fighter>().health;

        PlayerData.data.MONEY = Inventory.money;

        Inventory.SendInventoryState();

        PlayerEquipment.SendEquipmentState();

        if (GameObject.Find("Vending Machine") != null)
            Storage.SendStorageDataToSave();
        else Debug.Log("no storage");

        PlayerData.data.PLAYERLEVEL = GetComponent<LevelingSystem>().level;
        PlayerData.data.PLAYEREXP = GetComponent<LevelingSystem>().currentExp;

        GameObject.Find("Quest Manager").GetComponent<QuestManager>().SentQuestState();

        PlayerData.data.WELL = GetComponent<Fighter>().resWell.id;
        if (LevelingSystem.dogBossKill)
            PlayerData.data.BOSSONEKILLED = 1;
        if (LevelingSystem.kBossKill)
            PlayerData.data.BOSSTWOKILLED = 1;
        if (LevelingSystem.coyoteBossKill)
            PlayerData.data.BOSSTHREEKILLED = 1;
    }
}
