using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class PrologueManager : MonoBehaviour {

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
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
