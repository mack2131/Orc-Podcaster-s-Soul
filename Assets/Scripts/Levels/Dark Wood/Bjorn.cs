using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class Bjorn : MonoBehaviour {

    public AnimationClip idle;
    private Animation anim;

    private GameObject player;
    private Canvas dialogCanvas;

    private bool wasOpen;

    private bool freeDialog;

	// Use this for initialization
	void Start () 
    {
        wasOpen = false;
        anim = GetComponent<Animation>();//собираем всю анимацию клипы
        dialogCanvas = GameObject.Find("DialogCanvas").GetComponent<Canvas>();
        freeDialog = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        FindPlayer();
        anim.CrossFade(idle.name);//играем анимацию покоя
        if (!IsNear() && wasOpen)
            CloseDialog();
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1) && IsNear())
        {
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[10, 1] == 2)
                OpenDialog(13, 11);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[11, 1] == 2)
                OpenDialog(14, 12);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[12, 1] == 2)
                OpenDialog(15, 13);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[13, 1] == 2)
                OpenDialog(16, 14);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[14, 1] == 2)
                OpenDialog(17, 15);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[15, 1] == 2)
                OpenDialog(18, 16);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[16, 1] == 2 ||
                GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[16, 1] == 1)
                freeDialog = true;

            if (freeDialog)
                OpenDialog(19, -1);
        }
    }

    bool IsNear()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 5)
            return true;
        else return false;
    }

    void FindPlayer()
    {
        if (player == null)
            GameObject.FindGameObjectWithTag("Player");

        if (dialogCanvas == null)
            GameObject.Find("DialogCanvas");
    }

    void OpenDialog(int dialogId, int questId)
    {
        wasOpen = true;
        dialogCanvas.GetComponent<DialogCanvas>().questId = questId;
        dialogCanvas.GetComponent<DialogCanvas>().dialogId = dialogId;
        dialogCanvas.enabled = true;
    }

    void CloseDialog()
    {
        wasOpen = false;
        dialogCanvas.enabled = false;
    }
}
