using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public Button exitButton;
    public Button continueButton;
    public Button saveButton;

    public Canvas pauseUI;

    public static bool isBossFight;
    public static bool isPause;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isBossFight)
            saveButton.gameObject.SetActive(true);
        else saveButton.gameObject.SetActive(false);

        if (Input.GetKey(KeyCode.Escape))
        {
            Pause.isPause = true;
            pauseUI.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;
            exitButton.onClick.AddListener(ExitPressed);
            continueButton.onClick.AddListener(ContinuePressed);
            saveButton.onClick.AddListener(SavePressed);
        }
	}

    void ExitPressed()
    {
        Application.Quit();
    }

    void ContinuePressed()
    {
        Pause.isPause = false;
        pauseUI.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
    }

    void SavePressed()
    {
        Debug.Log(Application.persistentDataPath);
        //SaveLoad.Save();
        SaveLoad.SaveAsJson();
    }
}
