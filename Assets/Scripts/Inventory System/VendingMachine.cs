using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class VendingMachine : MonoBehaviour {

    public Canvas vedingMachineUI;//интерфейс магазина
    public GameObject inventorySlot;//слот для вещи, Image
    public GameObject inventoryItem;//сама вещь, Image
    public GameObject slotPanel;//панель для слотов вещей
    public ItemDatabase database;//список всех вещей

    public static List<Item> items = new List<Item>();//список всех вещей
    public static List<GameObject> slots = new List<GameObject>();//список слотов

	// Use this for initialization
	void Start () 
    {
        CloseTradingUi();//закрываем окно магазин
        database = GameObject.Find("Inventory System Manager").GetComponent<Inventory>().database; // new ItemDatabase();
        slots.Clear();//очищаем слоты, если не очищать, то при повторном заходе на сцену, будут появляться баги и ошибки

        for (int i = 0; i < 88; i++)//цикл по количеству слотов
        {
            items.Add(new Item());//добавляем в список вещей вещь
            slots.Add(Instantiate(inventorySlot));//создаем объект слот
            slots[i].transform.SetParent(slotPanel.transform);//ставим ему родителя объект GridLayotGroup, сюда будут ставиться слоты
            slots[i].transform.localScale = new Vector3(0.9999977f, 0.9999977f, 0.9999977f);//подгоняем размер
            slots[i].name = "Vendor Slot " + i.ToString();//приписываем объекту имя
        }

        /* добавляем еду в магазин */
        VendorAddItem(2); /* Сладкий пончик */
        VendorAddItem(26);/* Пончик с лесными орехами */
        VendorAddItem(31);/* Основа для пончика */
        VendorAddItem(32);/* Шоколодный пончик */
        VendorAddItem(48);/* Пончик с голубой глазурью */
        VendorAddItem(49);/* Пончик с сахарной пудрой */
        /* добавляем шлем и контроллер пс вр */
        VendorAddItem(5);
        VendorAddItem(6);
        /* добавляем простые вещи */
        VendorAddItem(34);/* Каска оперативника */
        VendorAddItem(35);/* Простой шлем */
        VendorAddItem(36);/* Костяная корона */
        VendorAddItem(38);/* Магнитный шлем */
        VendorAddItem(41);/* Рубиновый меч */
        VendorAddItem(43);/* Дробилка */
        VendorAddItem(45);/* Меч искателя */
        VendorAddItem(46);/* Банка на палке */
        VendorAddItem(47);/* Коса */
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!IsNear())//если далеко от магазина
            CloseTradingUi();//закрываем интерфейс магазина
	}

    void OnMouseOver()//когда навели мышку
    {
        if (IsNear() && Input.GetMouseButtonUp(1))//если очень близко и нажали ПКМ
            OpenTradingUi();//открываем магазин
        //подсвечиваем, когда навели мышку
         GetComponentInChildren<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
    }

    void OnMouseExit()//как только увели мышку
    {   
        //убрали подсветку
        GetComponentInChildren<Renderer>().material.shader = Shader.Find("Diffuse");
    }

    bool IsNear()//рядом ли с магазином
    {
        //если дистанция между магазином и гроком меньше числа, 
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 7)
            return true;//то верно, близко
        else return false;//иначе далеко
    }

    void OpenTradingUi()//открываем магазин
    {
        vedingMachineUI.enabled = true;//открываем юи магазина
        Inventory.isTrading = true;//говорим, что мы сейчас торгуем
        GameObject.Find("InventoryUI").GetComponent<Canvas>().enabled = true;//открывем сумку игрока
    }

    void CloseTradingUi()//закрываем магазин
    {
        vedingMachineUI.enabled = false;//закрываем интерфейс магазина
        Inventory.isTrading = false;//говорим, что больше не торгуем
    }

    public void VendorAddItem(int id)//добавление вещи в магазин
    {
        for (int i = 0; i < 88; i++)//цикл по количетсву слотов
        {
            if (slots[i].transform.childCount == 0)//если нет детей, т.е. нет вещей в слоте, т.е. слот пустой
            {
                Item addItem = database.FetchItemById(id);//берем вещь из списка вещей
                GameObject newItem = Instantiate(inventoryItem);//создаем объект вещь Image 
                newItem.GetComponent<ItemData>().item = addItem;//передаем параметры
                newItem.transform.SetParent(slots[i].transform);//ставим родителя
                newItem.transform.position = newItem.transform.parent.transform.position;//меняем позицию
                newItem.GetComponent<Image>().sprite = addItem.icon;//ставим иконку
                newItem.GetComponent<ItemData>().slotNumber = i;//передаем номер слота
                break;
            }
        }
    }
}
