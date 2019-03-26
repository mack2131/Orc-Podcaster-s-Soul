using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class FireballHint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image spellHint;

	// Use this for initialization
	void Start () 
    {
        spellHint = GameObject.Find("Spell Hint").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    #region IPointerEnterHandler Members

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Magic.enableFireball)
            ShowHint();
    }

    #endregion

    #region IPointerExitHandler Members

    public void OnPointerExit(PointerEventData eventData)
    {
        HideHint();
    }

    #endregion

    void ShowHint()
    {
        //включаем изображение подсказки
        spellHint.enabled = true;
        //имя в подсказке
        spellHint.transform.GetChild(0).GetComponent<Text>().text = "Огненный шар";
        //количество урона
        spellHint.transform.GetChild(1).GetComponent<Text>().text = GameObject.FindGameObjectWithTag("Player").GetComponent<Magic>().fireballDamage.ToString();
        //описание заклинания
        spellHint.transform.GetChild(2).GetComponent<Text>().text = "Мощное заклинание, которое фокусирует силу горящего пукана в шар.";
        //картинка силы заклинаний
        spellHint.transform.GetChild(3).GetComponent<Image>().enabled = true;
    }

    void HideHint()
    {
        //отключаем изображение подсказки
        spellHint.enabled = false;
        //имя в подсказке
        spellHint.transform.GetChild(0).GetComponent<Text>().text = "";
        //количество урона
        spellHint.transform.GetChild(1).GetComponent<Text>().text = "";
        //описание заклинания
        spellHint.transform.GetChild(2).GetComponent<Text>().text = "";
        //картинка силы заклинаний
        spellHint.transform.GetChild(3).GetComponent<Image>().enabled = false;
    }
}
