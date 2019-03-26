using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeResButton : MonoBehaviour {

    public int resId;
    public Toggle fullScreenToggle;
    private bool full;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Button>().onClick.AddListener(SetScreenRes);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void SetScreenRes()
    {
        if (fullScreenToggle.isOn)
            full = true;
        else full = false;

        switch (resId)
        {
            case 0:
                {
                    Screen.SetResolution(800, 600, full);
                    PlayerSettings.settings.SCREENRES = 0;
                    break;
                }
            case 1:
                {
                    Screen.SetResolution(1024, 768, full);
                    PlayerSettings.settings.SCREENRES = 1;
                    break;
                }
            case 2:
                {
                    Screen.SetResolution(1280, 1024, full);
                    PlayerSettings.settings.SCREENRES = 2;
                    break;
                }
            case 3:
                {
                    Screen.SetResolution(1920, 1080, full);
                    PlayerSettings.settings.SCREENRES = 3;
                    break;
                }
        }
    }
}
