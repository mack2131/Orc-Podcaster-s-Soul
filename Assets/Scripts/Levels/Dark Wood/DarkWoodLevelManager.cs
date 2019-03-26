using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class DarkWoodLevelManager : MonoBehaviour {

    public Transform playerPosition;

    public GameObject honeyJar;

    public bool honeyJarSpawn;

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
        for (int i = 10; i < quests.Length / 2; i++)
        {
            switch (i)
            {
                case 10:/* квест 11 - Путь в темный лес */
                    {
                        if (quests[i, 1] == 1)
                            GameObject.Find("Bjorn Position").GetComponent<Collider>().enabled = true;

                        if (quests[i, 1] == 2)
                            GameObject.Find("Bjorn Position").GetComponent<Collider>().enabled = false;
                        break;
                        /*=======================================*/
                    }
                case 11:/* квест 12 - Космический путешественник */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Quest Pine") != null)
                                Destroy(GameObject.Find("Quest Pine").gameObject);
                        }
                        if (quests[i, 1] == 2)
                        {
                            if (GameObject.Find("Quest Pine") != null)
                                Destroy(GameObject.Find("Quest Pine").gameObject);
                            GameObject.Find("Ship Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 12:/* квест 13 - Найти мед */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Honey Jar(Clone)") == null && !honeyJarSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(25))
                            {
                                Instantiate(honeyJar);
                                honeyJarSpawn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().honeyJarSpawn = true;
                            }

                            if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(25))
                            {
                                GameObject.Find("Bjorn Position").GetComponent<Collider>().enabled = true;
                            }
                        }

                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("Bjorn Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 13:/* квест 14 - Соусная сила */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Quest Pine 2") != null)
                                Destroy(GameObject.Find("Quest Pine 2").gameObject);
                        }
                        if (quests[i, 1] == 2)
                        {
                            if (GameObject.Find("Quest Pine 2") != null)
                                Destroy(GameObject.Find("Quest Pine 2").gameObject);
                            GameObject.Find("Boss examine position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 14:/*квест 15 - В поисках Богини */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (!GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(7))
                                BoginaDarkwood.isGiven = false;
                        }
                        break;
                    }
                case 15:/*квест 16 - Огромный монстр */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Boss Stone Enter") != null)
                                Destroy(GameObject.Find("Boss Stone Enter").gameObject);
                        }

                        if (quests[i, 1] == 2)
                        {
                            if (GameObject.Find("Boss Stone Enter") != null)
                                Destroy(GameObject.Find("Boss Stone Enter").gameObject);

                            if (GameObject.Find("Zhu-Zha Boss") != null)
                                Destroy(GameObject.Find("Zhu-Zha Boss").gameObject);

                            if (GameObject.Find("Boss Stone Exit") != null)
                                Destroy(GameObject.Find("Boss Stone Exit").gameObject);

                            GameObject.Find("Start Boss Fight Trigger").GetComponent<Collider>().enabled = false;
                        }
                        break;
                    }
            }//конец switch

        }//конец цикла for
    }//конец функции LoadState()
}
