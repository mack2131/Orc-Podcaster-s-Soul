using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class SpiritTutorial : MonoBehaviour {

    public AnimationClip idle;
    private Animation anim;

    private GameObject player;
    private Canvas dialogCanvas;

	// Use this for initialization
	void Start () 
    {
		anim = GetComponent<Animation>();//собираем всю анимацию клипы
        dialogCanvas = GameObject.Find("DialogCanvas").GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        FindPlayer();
        anim.CrossFade(idle.name);//играем анимацию покоя
        if(!IsNear())
            CloseDialog();
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1) && IsNear())
        {
            /*смотрим массив квестов, если квест выполнен, переходим на следующий*/
            if(GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[0, 1] != 2)
                OpenDialog(0, 0);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[0, 1] == 2)
                OpenDialog(1, 1);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[1, 1] == 2)
                OpenDialog(2, 2);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[2, 1] == 2)
                OpenDialog(3, 3);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[3, 1] == 2)
                OpenDialog(4, 4);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[4, 1] == 2)
                OpenDialog(5, 5);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[5, 1] == 2 ||
                GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[5, 1] == 1)
                OpenDialog(32, -1);
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
        dialogCanvas.GetComponent<DialogCanvas>().questId = questId;
        dialogCanvas.GetComponent<DialogCanvas>().dialogId = dialogId;
        dialogCanvas.enabled = true;
    }

    void CloseDialog()
    {
        dialogCanvas.enabled = false;
    }
}
