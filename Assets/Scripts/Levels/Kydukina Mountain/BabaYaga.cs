using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabaYaga : MonoBehaviour {

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
            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[17, 1] == 2)
                OpenDialog(21, 18);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[18, 1] == 2)
                OpenDialog(22, 19);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[19, 1] == 2)
                OpenDialog(23, 20);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[20, 1] == 2)
                OpenDialog(24, 21);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[21, 1] == 2)
                OpenDialog(25, 22);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[22, 1] == 2)
                OpenDialog(26, 23);

            if (GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[23, 1] == 2 ||
                GameObject.Find("Quest Manager").GetComponent<QuestManager>().allQuests[23, 1] == 1)
                freeDialog = true;

            if (freeDialog)
                OpenDialog(27, -1);
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
