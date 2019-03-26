using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class BoginaDarkwood : MonoBehaviour {

    public Transform position;
    public static bool isGiven;

	// Use this for initialization
	void Start () 
    {
        isGiven = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1) && IsNear())
        {
            //GameObject.Find("Quest Manager").GetComponent<QuestManager>().isHaveWoodenLog = true;
            if (!isGiven)
            {
                GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(7);
                GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(22);
                isGiven = true;
            }
        }
    }

    bool IsNear()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, position.transform.position) < 5)
            return true;
        else return false;
    }
}
