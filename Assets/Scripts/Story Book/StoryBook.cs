using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;

public class StoryBook : MonoBehaviour {

    public int storyId;
    public Canvas storyUI;
    public Button cancelButton;

    private string storyText;
    private StoryBooksDatabase database;

    private Vector3 posOffset = new Vector3();
    private Vector3 temPos = new Vector3();

	// Use this for initialization
	void Start () 
    {
        database = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<StoryBooksDatabase>();
        storyText = database.FetchStoryById(storyId).story;
        posOffset = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Flying();
        cancelButton.onClick.AddListener(Cancel);

        if (!IsNear())
            CloseStoryUI();
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && IsNear())
        {
            OpenStoryUI();
            Inventory.readedBooks[storyId] = 1;
        }
    }

    bool IsNear()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 5)
            return true;
        else return false;
    }

    void OpenStoryUI()
    {
        storyUI.enabled = true;
        storyUI.GetComponentInChildren<Text>().text = "\n" + storyText;
    }

    void CloseStoryUI()
    {
        storyUI.enabled = false;
        storyUI.GetComponentInChildren<Text>().text = "";
    }

    public void Cancel()
    {
        CloseStoryUI();
    }

    void Flying()/*движение вверх-вниз объекта*/
    {
        temPos = posOffset;
        temPos.y += Mathf.Sin((Time.fixedTime * Mathf.PI * 1f) * 0.5f);
        transform.position = temPos;
    }
}
