using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class HelpIcon : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

    public Image hint;

	// Use this for initialization
	void Start () 
    {
        hint = GameObject.Find("Icon Hint").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    #region IPointerEnterHandler Members

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowHint();//показываем подсказку
    }

    #endregion

    #region IPointerExitHandler Members

    public void OnPointerExit(PointerEventData eventData)
    {
        HideHint();//скрываем подсказку
    }

    #endregion

    #region IPointerClickHandler Members

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    #endregion

    void ShowHint()//функция, показывающая подсказу
    {
        hint.GetComponent<Image>().enabled = true;//позволяем отображаться изображению подсказки
        hint.GetComponentInChildren<Text>().text = "Помощь в управлении (зажмите F1)";//пишем, что это сумка
    }

    void HideHint()//функция, скрывающая подсказку
    {
        hint.GetComponent<Image>().enabled = false;//скрываем изображение
        hint.GetComponentInChildren<Text>().text = "";//убираем текст
    }
}
