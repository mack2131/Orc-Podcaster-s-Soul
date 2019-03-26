using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour {


    private Button button;
    bool pressed;

	// Use this for initialization
	void Start () 
    {
        button = this.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (File.Exists(Application.persistentDataPath + "/SavedGame.orc"))
            button.interactable = true;
        else button.interactable = false;

        button.onClick.AddListener(Pressed);

        if (Input.GetMouseButtonUp(0) && pressed == true)
        {
            SaveLoad.LoadAsJson();
            SceneManager.LoadScene(SaveLoad.savedGame.CURRENTSCENE);
            GameObject.Find("LoadingScreenUI").GetComponent<Canvas>().enabled = true;
            pressed = false;
        }
	}

    void Pressed()
    {
        pressed = true;
    }
}
