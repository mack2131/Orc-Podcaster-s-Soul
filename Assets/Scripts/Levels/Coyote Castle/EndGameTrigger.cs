using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class EndGameTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
        {
            GameObject.Find("LoadingScreenUI").GetComponent<Canvas>().enabled = true;
            SceneManager.LoadScene("Credits");
        }
	}
}
