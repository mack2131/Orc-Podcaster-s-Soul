using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class PlayerUI : MonoBehaviour {

    public Image healthBar;//полоса здоровья
    public Image targetHealthBar;
    public Image targetHealthBarFrame;
    public Canvas playerInterfaceUI;
    public Canvas inventoryUI;
    public Image helpImage;
    public Image map;
    public Sprite[] maps = new Sprite[6];

    private GameObject target;
    private int qCounter;
    private string name;//имя сцены

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        //редактируем заполнение полосы здоровья
        healthBar.fillAmount = GetComponent<Fighter>().health / GetComponent<Fighter>().maxHealth;
        if (inventoryUI.enabled == true)//если открыт инвентарь сумка
            playerInterfaceUI.enabled = false;//убираем иконку и полосу здоровья
        else playerInterfaceUI.enabled = true;//иначе полоса здоровья и иконка доступны

        //полоса здоровья для цели
        TargetHealthBar();
            
	}

    void TargetHealthBar()
    {
        if (GetComponent<Fighter>().opponent != null)
        {
            targetHealthBar.enabled = true;
            targetHealthBarFrame.enabled = true;
            if (GetComponent<Fighter>().opponent.tag != "Boss Dark Wood" && GetComponent<Fighter>().opponent.tag != "Library Boss" && GetComponent<Fighter>().opponent.tag != "CoyoteBoss")
                targetHealthBar.fillAmount = GetComponent<Fighter>().opponent.GetComponent<Mob>().health / GetComponent<Fighter>().opponent.GetComponent<Mob>().maxHealth;

            if(GetComponent<Fighter>().opponent.tag == "Boss Dark Wood")
                targetHealthBar.fillAmount = GetComponent<Fighter>().opponent.GetComponent<DogBoss>().health / GetComponent<Fighter>().opponent.GetComponent<DogBoss>().maxHealth;

            if(GetComponent<Fighter>().opponent.tag == "Library Boss")
                targetHealthBar.fillAmount = GetComponent<Fighter>().opponent.GetComponent<KBoss>().health / GetComponent<Fighter>().opponent.GetComponent<KBoss>().maxHealth;
            
            if (GetComponent<Fighter>().opponent.tag == "CoyoteBoss")
                targetHealthBar.fillAmount = GetComponent<Fighter>().opponent.GetComponent<CoyoteBoss>().health / GetComponent<Fighter>().opponent.GetComponent<CoyoteBoss>().maxHealth;
        }
        else
        {
            targetHealthBar.enabled = false;
            targetHealthBarFrame.enabled = false;
        }

        OnOffPlayerHelp();
        OnOffMap();
    }

    void OnOffPlayerHelp()
    {
        if (Input.GetKey(KeyCode.F1) && !Pause.isPause)//если нажали кнопку
            helpImage.enabled = true;
        else helpImage.enabled = false;
    }

    void OnOffMap()
    {
        if (Input.GetKey(KeyCode.M) && !Pause.isPause)//если нажали кнопку
        {
            name = SceneManager.GetActiveScene().name;
            map.enabled = true;

            switch (name)
            {
                case "Hub":
                    {
                        map.GetComponent<Image>().sprite = maps[1];
                        break;
                    }
                case "Dark Wood":
                    {
                        map.GetComponent<Image>().sprite = maps[2];
                        break;
                    }
                case "Kudykina Mountain":
                    {
                        map.GetComponent<Image>().sprite = maps[3];
                        break;
                    }
                case "The Way to Uganda":
                    {
                        map.GetComponent<Image>().sprite = maps[4];
                        break;
                    }
                case "Tutorial":
                    {
                        map.GetComponent<Image>().sprite = maps[0];
                        break;
                    }
                default:
                    {
                        map.GetComponent<Image>().sprite = maps[5];
                        break;
                    }
            }
        }
        else map.enabled = false;
    }
}

