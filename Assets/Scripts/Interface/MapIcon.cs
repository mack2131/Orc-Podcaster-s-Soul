using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class MapIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private AudioSource sound;//звук открытия инвентаря
    public Image hint;

	// Use this for initialization
	void Start () 
    {
        hint = GameObject.Find("Icon Hint").GetComponent<Image>();//ищем подсказку
        sound = GetComponent<AudioSource>();//берем звук
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    #region IPointerEnterHandler Members

    public void OnPointerEnter(PointerEventData eventData)//если мышь зашла на значок
    {
        ShowHint();//показываем подсказку
    }

    #endregion

    #region IPointerExitHandler Members

    public void OnPointerExit(PointerEventData eventData)//если мышь увели со значка
    {
        HideHint();//скрываем подсказку
    }

    #endregion

    #region IPointerClickHandler Members

    public void OnPointerClick(PointerEventData eventData)//если кликнули
    {
        sound.Play();//проигрываем музыку открытия инветоря
        GameObject.Find("InventoryUI").GetComponent<Canvas>().enabled = true;//открываем сумку
        Inventory.bPressCount = 1;//ставим счетчик нажатий кнопки В, что равен 1, как бы нажали её для открытия инветоря
    }

    #endregion


    void ShowHint()//функция, показывающая подсказу
    {
        hint.GetComponent<Image>().enabled = true;//позволяем отображаться изображению подсказки
        hint.GetComponentInChildren<Text>().text = "Карта";//пишем, что это сумка
    }

    void HideHint()//функция, скрывающая подсказку
    {
        hint.GetComponent<Image>().enabled = false;//скрываем изображение
        hint.GetComponentInChildren<Text>().text = "";//убираем текст
    }
}
