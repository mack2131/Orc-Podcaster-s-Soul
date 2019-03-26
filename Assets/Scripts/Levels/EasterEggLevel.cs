using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggLevel : MonoBehaviour {

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
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = playerPosition.transform.position;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell == null)
            GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell = GameObject.Find("Start Revive Well").GetComponent<Well>();
    }

    // Update is called once per frame
    void Update()
    {
    }

}
