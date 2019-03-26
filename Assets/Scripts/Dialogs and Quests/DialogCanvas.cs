using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class DialogCanvas : MonoBehaviour {

    public int dialogId;
    public int questId;
    public Text dialogText;
    public Button acceptButton;
    public Button cancelButton;

    public DialogDatabase database;

	// Use this for initialization
	void Start () 
    {
        database = GameObject.Find("Level Manager").GetComponent<DialogDatabase>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        cancelButton.onClick.AddListener(CancelButton);
        acceptButton.onClick.AddListener(AcceptButton);
        FillDialog();
	}

    void CancelButton()
    {
        GetComponent<Canvas>().enabled = false;
    }

    void FillDialog()
    {
        if(dialogId != -1)
            dialogText.text = "\n" + database.FetchDialogById(dialogId).dialog + "\n";
    }

    void AcceptButton()
    {
        if (questId != -1)
        {
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().AcceptQuest(questId);
            GetComponent<Canvas>().enabled = false;
        }
    }
}
