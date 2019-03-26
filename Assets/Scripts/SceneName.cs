using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class SceneName : MonoBehaviour {

    public Text sceneName;
    public Image backgroundImage;
    private string name;
    private float counter;
    public AudioSource[] sounds;

	// Use this for initialization
	void Start () 
    {
        name = SceneManager.GetActiveScene().name;
        sounds = GetComponents<AudioSource>();
        sounds[0].Play();
	}
	
	// Update is called once per frame
	void Update () 
    {
        ShowSceneName();
	}

    void ShowSceneName()
    {
        counter += Time.deltaTime;
        switch (name)
        {
            case "Hub":
                {
                    sceneName.text = "Деревня у замка";
                    break;
                }
            case "Dark Wood":
                {
                    sceneName.text = "Тёмный лес";
                    break;
                }
            case "Kudykina Mountain":
                {
                    sceneName.text = "Кудыкина гора";
                    break;
                }
            case "The Way to Uganda":
                {
                    sceneName.text = "Библиотека";
                    break;
                }
            case "Tutorial":
                {
                    sceneName.text = "Пещера";
                    break;
                }
            case "Coyote Castle":
                {
                    sceneName.text = "Замок Белого Койота";
                    break;
                }
            case "Castle Arena":
                {
                    sceneName.text = "Край мира";
                    break;
                }
            case "Easter Egg":
                {
                    sceneName.text = "Квартира Орка";
                    break;
                }
            default: break;
        }
        if (counter >= 4)
        {
            GetComponent<Canvas>().enabled = false;
        }
        else
        {
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, backgroundImage.color.a - counter / 400);
            sceneName.color = new Color(sceneName.color.r, sceneName.color.g, sceneName.color.b, sceneName.color.a - counter / 400);
        }
    }
}
