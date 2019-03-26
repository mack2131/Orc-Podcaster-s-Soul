using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class ClickButtonMusic : MonoBehaviour {

    public AudioSource sounds;

	// Use this for initialization
	void Start () 
    {
        sounds = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.GetComponent<Button>().onClick.AddListener(PlayMusic);
	}

    void PlayMusic()
    {
        sounds.Play();
    }
}
