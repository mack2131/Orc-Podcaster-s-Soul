using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class TutorialLevelManager : MonoBehaviour {

    public Transform playerPosition;

    void Awake()
    {
        if (SaveLoad.wasLoaded)
        {
            playerPosition.transform.position = SaveLoad.savedGame.PLAYERPOSITION;
            SaveLoad.positionAndSceneLoaded = false;
        }
    }

    void OnLevelWasLoaded()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
        {
            for (int i = 1; i < players.Length; i++)
                Destroy(players[i]);
        }
        //players[0].transform.position = playerPosition.transform.position;
    }

	// Use this for initialization
	void Start () 
    {
        LoadLevelState();
        GameObject.FindGameObjectWithTag("Player").transform.position = playerPosition.transform.position;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell == null)
            GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell = GameObject.Find("Start Revive Well").GetComponent<Well>(); ;
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    void LoadLevelState()
    {
        if (SaveLoad.savedGame != null)
        {
            GameObject[] wells = GameObject.FindGameObjectsWithTag("Well");
            for (int i = 0; i < wells.Length; i++)
            {
                if (wells[i].GetComponent<Well>().id == SaveLoad.savedGame.WELL)
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell = wells[i].GetComponent<Well>();
            }
        }

        int[,] quests = new int[GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests.Length / 2, 2];
        quests = GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests;
        for (int i = 0; i < quests.Length / 2; i++)
        {
            switch (i)
            {
                case 0:/*ПЕРВЫЙ КВЕСТ Учимся двигаться*/
                    {
                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("Spirit").transform.position = GameObject.Find("Spirit Position 2").transform.position;
                            Destroy(GameObject.Find("Rocks 1").gameObject);
                            GameObject.Find("Spirit Position 2").GetComponent<Collider>().enabled = false;
                        }
                        if (quests[i, 1] == 1)
                        {
                            GameObject.Find("Spirit").transform.position = GameObject.Find("Spirit Position 2").transform.position;
                            Destroy(GameObject.Find("Rocks 1").gameObject);
                        }
                        break;
                        /*=======================================*/
                    }
                case 1:/*ВТОРОЙ КВЕСТ Учимся открывать сундуки*/
                    {
                        if (quests[i, 1] == 2)
                        {
                            Destroy(GameObject.Find("Rocks 2").gameObject);
                        }
                        if (quests[i, 1] == 1)
                        {
                            Destroy(GameObject.Find("Rocks 2").gameObject);
                            if (!GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(8))
                                Inventory.lootedLootChests[0] = 0;
                            if (!GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(9))
                                Inventory.lootedLootChests[1] = 0;
                        }
                        break;
                        /*=======================================*/
                    }
                case 2:/*Третий квест Убить гусей*/
                    {
                        if (quests[i, 1] == 2 || quests[i, 1] == 1)
                        {
                            Destroy(GameObject.Find("Rocks 3").gameObject);
                            GameObject.Find("Spirit").transform.position = GameObject.Find("Spirit Position 3").transform.position;
                        }
                        break;
                        /*=======================================*/
                    }
                case 3:/*Четвертый квест Прочитать манускрипт*/
                    {
                        if (quests[i, 1] == 2 || quests[i, 1] == 1)
                        {
                            Destroy(GameObject.Find("Rocks 4").gameObject);
                            GameObject.Find("Spirit").transform.position = GameObject.Find("Spirit Position 4").transform.position;
                        }
                        break;
                        /*=======================================*/
                    }
                case 4:/*Пятый квест Кристаллический путь*/
                    {
                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("Spirit").transform.position = Vector3.MoveTowards(GameObject.Find("Spirit").transform.position, GameObject.Find("Spirit Position 5").transform.position, Time.deltaTime * 200);
                            Destroy(GameObject.Find("Rocks 6").gameObject);
                            Destroy(GameObject.Find("Rocks 5").gameObject);
                        }
                        if (quests[i, 1] == 1 && GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell.id == 1)
                        {
                            Destroy(GameObject.Find("Rocks 6").gameObject);
                            Destroy(GameObject.Find("Rocks 5").gameObject);
                            GameObject.Find("Spirit").transform.position = Vector3.MoveTowards(GameObject.Find("Spirit").transform.position, GameObject.Find("Spirit Position 5").transform.position, Time.deltaTime * 200);
                        }
                        if (quests[i, 1] == 1 && GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell.id != 1)
                        {
                            Destroy(GameObject.Find("Rocks 5").gameObject);
                            GameObject.Find("Spirit").transform.position = Vector3.MoveTowards(GameObject.Find("Spirit").transform.position, GameObject.Find("Spirit Position 5").transform.position, Time.deltaTime * 200);
                        }
                        break;
                    }
                case 5:/*Шестой квест Знакомый в деревне*/
                    {
                        if (quests[i, 1] == 1)
                        {
                            GameObject.Find("Spirit").transform.position = GameObject.Find("Spirit Position 5").transform.position;
                            GameObject.Find("Spirit Position 5").GetComponent<Collider>().enabled = false;
                            Destroy(GameObject.Find("Rocks 7").gameObject);
                            playerPosition.position = GameObject.Find("New Player Start Position").transform.position;
                        }
                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("Spirit").transform.position = GameObject.Find("Spirit Position 5").transform.position;
                            GameObject.Find("Spirit Position 5").GetComponent<Collider>().enabled = false;
                            Destroy(GameObject.Find("Rocks 7").gameObject);
                            playerPosition.position = GameObject.Find("New Player Start Position").transform.position;
                        }
                        break;
                    }
            }//конец switch

        }//конец цикла for
    }//конец функции LoadState()
}
