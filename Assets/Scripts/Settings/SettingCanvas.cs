using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using LitJson;

public class SettingCanvas : MonoBehaviour {

    public Button okButton;
    public Slider volumeSlider;
    public Toggle fullsreenToggle;
    public Dropdown screenResDropdown;

    PlayerSettings playerSettings;
    
	// Use this for initialization
	void Start () 
    {
        okButton.onClick.AddListener(CloseSettings);
        //if (SceneManager.GetActiveScene().name == "Main Menu")
        //    LoadMenuSettings();
        //else LoadSettings();
        LoadSettings();
        GetComponent<Canvas>().enabled = false;

    }

    void CloseSettings()
    {
        SaveSettings();
        GetComponent<Canvas>().enabled = false;
    }

    void LoadSettings()
    {
        bool full;

        string jsonString = File.ReadAllText(Application.dataPath + "//StreamingAssets/Settings.orc");
        playerSettings = JsonUtility.FromJson<PlayerSettings>(jsonString);

        AudioListener.volume = playerSettings.VOLUME;
        volumeSlider.value = AudioListener.volume;

        if (playerSettings.FULLSCREEN == 1)
        {
            //full = true;
            full = false;
            fullsreenToggle.isOn = true;
        }
        else
        {
            //full = false;
            full = true;
            fullsreenToggle.isOn = true;
        }

        if (playerSettings.SCREENRES == 0)
        {
            Screen.SetResolution(800, 600, full);
            //screenResDropdown.value = 0;
        }
        if (playerSettings.SCREENRES == 1)
        {
            Screen.SetResolution(1024, 768, full);
            //screenResDropdown.value = 1;
        }
        if (playerSettings.SCREENRES == 2)
        {
            Screen.SetResolution(1280, 1024, full);
            //screenResDropdown.value = 2;
        }
        if (playerSettings.SCREENRES == 3)
        {
            Screen.SetResolution(1920, 1080, full);
            //screenResDropdown.value = 3;
        }
    }

    void SaveSettings()
    {
        PlayerSettings.settings.VOLUME = AudioListener.volume;

        if(Screen.fullScreen)
            PlayerSettings.settings.FULLSCREEN = 1;
        else PlayerSettings.settings.FULLSCREEN = 0;

        /*if (Screen.currentResolution.width == 800)
            PlayerSettings.settings.SCREENRES = 0;
        if (Screen.currentResolution.width == 1024)
            PlayerSettings.settings.SCREENRES = 1;
        if (Screen.currentResolution.width == 1280)
            PlayerSettings.settings.SCREENRES = 2;
        if (Screen.currentResolution.width == 1920)
            PlayerSettings.settings.SCREENRES = 3;*/

        string json = JsonUtility.ToJson(PlayerSettings.settings);
        File.WriteAllText(Application.dataPath + "//StreamingAssets/Settings.orc", json);
    }
}

public class PlayerSettings
{
    public static PlayerSettings settings = new PlayerSettings();
    public int FULLSCREEN;
    public float VOLUME;
    public int SCREENRES;
}
