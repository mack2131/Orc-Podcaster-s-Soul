using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class Intro : MonoBehaviour {

    public Image logo;
    public Image disc;
    public float speed;
    public GameObject comics;
    public AudioSource[] sounds;
    private float counter;

	// Use this for initialization
	void Start () 
    {
        counter = 0;
        sounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        counter += Time.deltaTime;
        if (counter > 5 && GameObject.Find("White Coyote Game") != null)
            Destroy(logo.gameObject);

        if (counter > 25 && GameObject.Find("Disclaimer") != null)
            Destroy(disc.gameObject);

        if (GameObject.Find("Disclaimer") == null)
        {
            counter = 0;
            if(!sounds[0].isPlaying)
                sounds[0].Play();
            //comics.transform.position = new Vector3(comics.transform.position.x, comics.transform.position.y + Time.fixedDeltaTime * speed, comics.transform.position.z);
            comics.transform.Translate(Vector3.up * speed, Space.World);
            if (Input.anyKey /*|| comics.transform.position.y > 3500*/)
                SceneManager.LoadScene("MainMenu");
        }
	}
}
