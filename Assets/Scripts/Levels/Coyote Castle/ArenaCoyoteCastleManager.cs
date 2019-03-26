using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class ArenaCoyoteCastleManager : MonoBehaviour {

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
                case 27:/*квест 28 - Игра за трон */
                    {
                        if (quests[i, 1] == 2)
                            GameObject.Find("Coyote Arena Position (1)").GetComponent<Collider>().enabled = false;
                        break;
                        /*=======================================*/
                    }
                case 28:/*квест 29 - Побег */
                    {
                        break;
                    }
            }//конец switch

        }//конец цикла for
    }//конец функции LoadState()
}
