using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class WhiteCoyoteHub : MonoBehaviour {

    public AnimationClip idle;
    private Animation anim;

    private GameObject player;
    private Canvas dialogCanvas;

    private bool wasOpen;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();//собираем всю анимацию клипы
        dialogCanvas = GameObject.Find("DialogCanvas").GetComponent<Canvas>();

    }

    // Update is called once per frame
    void Update()
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
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[6, 1] == 2)
                OpenDialog(7, 7);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[7, 1] == 2)
                OpenDialog(8, 8);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[8, 1] == 2)
                OpenDialog(9, 9);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[9, 1] == 2)
                OpenDialog(10, 10);

            /* свободная фраза */
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[10, 1] == 2)
                OpenDialog(12, -1);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[23, 1] == 2)
                OpenDialog(28, 24);

            /* свободная фраза */
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[28, 1] == 2)
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
