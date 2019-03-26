using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour {

    public Canvas settingCanvas;
    private Button button;

	// Use this for initialization
	void Start () 
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(Pressed);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (settingCanvas == null)
            settingCanvas = GameObject.FindObjectOfType<SettingCanvas>().gameObject.GetComponent<Canvas>();
	}

    void Pressed()
    {
        settingCanvas.enabled = true;
    }
}
