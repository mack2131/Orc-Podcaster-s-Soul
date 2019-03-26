using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class WoodenLogQuest : MonoBehaviour {


	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1) && IsNear())
        {
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().isHaveWoodenLog = true;
            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(10);
            Destroy(gameObject);
        }
    }

    bool IsNear()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 5)
            return true;
        else return false;
    }
}
