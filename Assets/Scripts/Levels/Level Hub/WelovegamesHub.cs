using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class WelovegamesHub : MonoBehaviour {

    public AnimationClip idle;
    private Animation anim;

    private GameObject player;
    private Canvas dialogCanvas;

    private bool wasOpen;

	// Use this for initialization
	void Start () 
    {
        wasOpen = false;
        anim = GetComponent<Animation>();//собираем всю анимацию клипы
        dialogCanvas = GameObject.Find("DialogCanvas").GetComponent<Canvas>();
		
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
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[5, 1] == 2)
                OpenDialog(6, 6);

            /* свободная фраза */
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[6, 1] == 2)
                OpenDialog(11, -1);

            if(GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[16, 1] == 2)
                OpenDialog(20, 17);

            /* свободная фраза */
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[20, 1] == 2)
                OpenDialog(11, -1);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[25, 1] == 2)
                OpenDialog(33, 27);

            /* свободная фраза */
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[27, 1] == 2)
                OpenDialog(11, -1);
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
