using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class teleportToTutorialLevelTest : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnTriggerEnter(Collider trigger)
    {
        GameObject.Find("LoadingScreenUI").GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene("Tutorial");
    }
}
