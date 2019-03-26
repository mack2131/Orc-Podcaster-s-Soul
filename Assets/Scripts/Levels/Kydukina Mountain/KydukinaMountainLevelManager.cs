using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
public class KydukinaMountainLevelManager : MonoBehaviour {

    public Transform playerPosition;

    public GameObject cottail;
    public GameObject blackM;
    public GameObject treeJuice;
    public GameObject mountainInv;

    public bool cottailSpawn;
    public bool blackMSpawn;
    public bool treeJuiceSpawn;
    public bool mountainInvSpawn;

    public GameObject easterEggPortal;
    private Vector3 eapPos;

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

        easterEggPortal = GameObject.Find("Portal to Easter Egg");
        eapPos = easterEggPortal.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        EasterEgg();
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
        for (int i = 17; i < quests.Length / 2; i++)
        {
            switch (i)
            {
                case 17:/*квест 18 - Найти Баб Ягу */
                    {
                        if (quests[i, 1] == 1)
                            GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = true;

                        if (quests[i, 1] == 2)
                            GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = false;
                        break;
                        /*=======================================*/
                    }
                case 18:/*квест 19 - Плюшевая шишка */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Quest Cottail(Clone)") == null && !cottailSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(28))
                            {
                                Instantiate(cottail);
                                cottailSpawn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().cottailSpawn = true;
                            }

                            if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(28))
                            {
                                GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = true;
                            }
                        }

                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 19:/*квест 20 - Черный гриб */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Quest Mushroom(Clone)") == null && !blackMSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(29))
                            {
                                Instantiate(blackM);
                                blackMSpawn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().blackMSpawn = true;
                            }

                            if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(29))
                            {
                                GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = true;
                            }
                        }

                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 20:/*квест 21 - Сок из дерева */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Tree Juice(Clone)") == null && !treeJuiceSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(30))
                            {
                                Instantiate(treeJuice);
                                treeJuiceSpawn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().treeJuiceSpawn = true;
                            }

                            if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(30))
                            {
                                GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = true;
                            }
                        }

                        if (quests[i, 1] == 2)
                        {
                            GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = false;
                        }
                        break;
                        /*=======================================*/
                    }
                case 21:/*квест 22 - Карлики */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Mountain Invansion(Clone)") == null && !mountainInvSpawn)
                            {
                                Instantiate(mountainInv);
                                mountainInvSpawn = true;
                                GameObject.Find("Quest Manager").GetComponent<QuestManager>().mountainInvSpawn = true;
                            }
                        }
                        break;
                    }
                case 22:/*квест 23 - Второй ключ */
                    {
                        if (quests[i, 1] == 1)
                        {
                            if (GameObject.Find("Mountain Quest Rocks") != null)
                                Destroy(GameObject.Find("Mountain Quest Rocks").gameObject);
                        }

                        if (quests[i, 1] == 2)
                        {
                            if (GameObject.Find("Mountain Quest Rocks") != null)
                                Destroy(GameObject.Find("Mountain Quest Rocks").gameObject);

                            GameObject.Find("Mountain Examine Trigger").GetComponent<Collider>().enabled = false;
                        }
                        break;
                    }
            }//конец switch

        }//конец цикла for
    }//конец функции LoadState()

    void EasterEgg()
    {
        if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(3) ||
            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(6))
            easterEggPortal.transform.position = new Vector3(252.1697f, 132.16f, 251.6494f);
        else easterEggPortal.transform.position = eapPos;
    }
}
