using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour {

    public DialogueNode[] node;
    public int currentNode;
    public bool showDialogueEnd = true;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnGUI()
    {
        if (showDialogueEnd != false)
        {
            GUI.Box(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 300, 600, 250), "");
            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 90), node[currentNode].NpcText);
            for (int i = 0; i < node[currentNode].playerAnswer.Length; i++)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 200 + 25 * i, 500, 25), node[currentNode].playerAnswer[i].answerText))
                {
                    if (node[currentNode].playerAnswer[i].speakEnd)
                    {
                        showDialogueEnd = false;
                    }
                    currentNode = node[currentNode].playerAnswer[i].nextNode;
                }
            }
        }
    }

}

[System.Serializable]
public class DialogueNode
{
    public string NpcText;
    public Answer[] playerAnswer;
}
[System.Serializable]
public class Answer
{
    public string answerText;
    public int nextNode;
    public bool speakEnd;
}
