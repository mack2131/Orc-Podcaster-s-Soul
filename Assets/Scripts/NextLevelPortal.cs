using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class NextLevelPortal : MonoBehaviour {

    public String sceneToLoadName;
    public AudioSource[] sounds;

	// Use this for initialization
	void Start () 
    {
        sounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButton(1) && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 7)
        {
            if (sceneToLoadName.Length != 0)
            {
                GameObject.Find("LoadingScreenUI").GetComponent<Canvas>().enabled = true;
                GameObject.FindGameObjectWithTag("Player Start Position").transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                sounds[0].Play();
                SceneManager.LoadScene(sceneToLoadName);
            }
        }
    }
}
