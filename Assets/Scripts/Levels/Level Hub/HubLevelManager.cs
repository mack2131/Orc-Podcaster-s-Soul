using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class HubLevelManager : MonoBehaviour {
    
    public Transform playerPosition;

    public GameObject GlassesQuestMob;
    public GameObject KnucklesLake;
    public GameObject questWoodenLog;
    public GameObject portalToLib;

    private bool questWoodenLogSpawn;
    private bool glassGooseSpawn;
    private bool KnucklesLakeSpwn;
    private bool portalToLibSpawn;

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
        players[0].transform.position = playerPosition.transform.position;
    }

	// Use this for initialization
	void Start () 
    {
        LoadLevelState();
        GameObject.FindGameObjectWithTag("Player").transform.position = playerPosition.transform.position;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell == null)
            GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell = GameObject.Find("Start Revive Well").GetComponent<Well>(); 
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
        for (int i = 5; i < quests.Length / 2; i++)
        {
            switch (i)
            {
                case 5://Шестой квест Знакомый в деревне
                    {
                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 6://Седьмой квест Поговорить с Белым Койотом
                    {
                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 7://восьмой квест Принести дрова Койoту
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Quest Wooden Log(Clone)") == null && !questWoodenLogSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(10))
                            {
                                Instantiate(questWoodenLog);
                                questWoodenLogSpawn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().questWoodenLogSpawn = true;
                            }

                            if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(10))
                            {
                                GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = true;
                            }
                        }

                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 8://Квесть девятый Тряпка для очков
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Glasses Quest Mob(Clone)") == null && !glassGooseSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(11))
                            {
                                Debug.Log("you here");
                                Instantiate(GlassesQuestMob);
                                glassGooseSpawn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().glassGooseSpawn = true;
                            }

                            if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(11))
                                GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = true;
                        }

                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 9:/*квест 10 - убить угандских войнов на молочном озере*/
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Knuckles Quest Lake(Clone)") == null && !KnucklesLakeSpwn)
                            {
                                Instantiate(KnucklesLake);
                                KnucklesLakeSpwn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().KnucklesLakeSpwn = true;
                            }
                        }
                        break;
                    }
                case 17:/*квест 18 - Вернуться к Денису */
                    {
                        if (quests[i, 1] == 1)
                            GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = true;
                        if (quests[i, 1] == 2)
                                GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = false;
                        break;
                    }
                case 23:/*квест 24 - Узнать про библиотеку */
                    {
                        if (quests[i, 1] == 1)
                            GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = true;

                        if (quests[i, 1] == 2)
                            GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = false;
                        break;
                    }
                case 24:/*квест 25 - Путь в библиотеку */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Portal to Library(Clone)") == null && !portalToLibSpawn)
                            {
                                Instantiate(portalToLib);
                                portalToLibSpawn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().portalToLibSpawn= true;
                            }
                        }

                        if (quests[i, 1] == 2)
                        {
                            Instantiate(portalToLib);
                            portalToLibSpawn = true;
                            GameObject.Find("Quest Manager").GetComponent<QuestManager>().portalToLibSpawn = true;
                        }
                        break;
                    }
                case 26:/*квест 27 - WEHATETHISGAME */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("WhiteCoyote") != null)
                                Destroy(GameObject.Find("WhiteCoyote").gameObject);

                            if (GameObject.Find("Welovegames Position") != null)
                                GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = true;
                        }

                        if (quests[i, 1] == 2)
                        {
                            if (GameObject.Find("WhiteCoyote") != null)
                                Destroy(GameObject.Find("WhiteCoyote").gameObject);

                            GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                    }
                case 27:/*квест 28 - Игра за трон */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Quest Doors") != null)
                                Destroy(GameObject.Find("Quest Doors").gameObject);
                        }
                        break;
                    }
            }//конец switch

        }//конец цикла for
    }//конец функции LoadState()
}
