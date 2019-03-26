using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class Well : MonoBehaviour {

    public String sceneName;
    public int id;

    public Transform resPosition;

    public AudioSource[] sounds;

	// Use this for initialization
	void Start () 
    {
        sceneName = SceneManager.GetActiveScene().name;
        sounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    void OnMouseOver()
    {
        GetComponentInChildren<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
        if (IsNear() && Input.GetMouseButtonUp(1))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell = GetComponent<Well>();
            sounds[0].Play();
        }
    }

    bool IsNear()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 6)
            return true;
        else return false;
    }

    void OnMouseExit()//как только увели мышку
    {
        //убрали подсветку
        GetComponentInChildren<Renderer>().material.shader = Shader.Find("Diffuse");
    }
}
