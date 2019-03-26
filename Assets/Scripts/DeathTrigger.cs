using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class DeathTrigger : MonoBehaviour {

    private bool isDead;

	// Use this for initialization
	void Start () 
    {
        isDead = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (this.GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
        {
            isDead = false;
            Death();
        }
	}

    void Death()
    {
        if (!isDead)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().GetHit(10000);
            GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell.resPosition.position;
            isDead = true;
        }

    }
}
