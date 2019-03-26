using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class HealHint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image spellHint;

    private Image spellDamageIcon;

    // Use this for initialization
    void Start()
    {
        spellHint = GameObject.Find("Spell Hint").GetComponent<Image>();
        spellDamageIcon = spellHint.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region IPointerEnterHandler Members

    public void OnPointerEnter(PointerEventData eventData)
    {
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
        spellHint.transform.GetChild(0).GetComponent<Text>().text = "Лечебный пончик";
        //описание заклинания
        spellHint.transform.GetChild(2).GetComponent<Text>().text = "Нет ничего лечебнее вкусного пончика от WhiteCoyote Donuts.";
    }

    void HideHint()
    {
        //отключаем изображение подсказки
        spellHint.enabled = false;
        //имя в подсказке
        spellHint.transform.GetChild(0).GetComponent<Text>().text = "";
        //описание заклинания
        spellHint.transform.GetChild(2).GetComponent<Text>().text = "";
    }
}