using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WlgArena : MonoBehaviour {

    public AnimationClip idle;
    private Animation anim;

    private GameObject player;
    private Canvas dialogCanvas;

    private bool wasOpen;

    // Use this for initialization
    void Start()
    {
        wasOpen = false;
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
            OpenDialog(37, -1);
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
