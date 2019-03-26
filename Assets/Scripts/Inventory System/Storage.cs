using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class Storage : MonoBehaviour {

    public Canvas storageUI;
    public GameObject inventorySlot;//слот для вещи, Image
    public GameObject inventoryItem;//сама вещь, Image
    public GameObject slotPanel;
    public ItemDatabase database;//список всех вещей

    public static int slotsAmount = 56;//количество слотов в банке

    public static List<Item> items = new List<Item>();//список всех вещей
    public static List<GameObject> slots = new List<GameObject>();//список слотов

	// Use this for initialization
	void Start () 
    {
        CloseStorageUi();
        database = GameObject.Find("Inventory System Manager").GetComponent<Inventory>().database; // new ItemDatabase();
        slots.Clear();

        for (int i = 0; i < slotsAmount; i++)//цикл по количеству слотов
        {
            items.Add(new Item());//добавляем в список вещей вещь
            slots.Add(Instantiate(inventorySlot));//создаем объект слот
            if(slotPanel.gameObject != null)
                slots[i].transform.SetParent(slotPanel.transform);//ставим ему родителя объект GridLayotGroup, сюда будут ставиться слоты
            slots[i].transform.localScale = new Vector3(0.9999977f, 0.9999977f, 0.9999977f);//подгоняем размер
            slots[i].name = "Storage Slot " + i.ToString();//приписываем объекту имя
        }

        //если загружаем игру
        if (SaveLoad.wasLoaded == true)
        {
            LoadStorage();//загружаем банк
            SaveLoad.storageLoaded = false;//говорим, что загрузили
        }

        UpdateStorage();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!IsNear())//если далеко или нажали В, закрываем юи банка
            CloseStorageUi();//функция закрытия ию банка

        SentStorageData();
	}

    void OnMouseOver()//когда наведена мышь
    {
        if (IsNear() && Input.GetMouseButtonUp(1))//если близко к банку и нажали ПКМ
            OpenStorageUi();//открываем юи банка

        //подсвечивае объект при наведении
        GetComponentInChildren<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
    }

    void OnMouseExit()//когда мышь ушла с объекта
    {
        //возвращаем здоровый шейдер
        GetComponentInChildren<Renderer>().material.shader = Shader.Find("Diffuse");
    }

    bool IsNear()//проверяем, близко ли мы к банку
    {   
        //если расстояние между игроком и банком меньше заданного числа
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, GameObject.Find("Open Bank Position").transform.position) < 7)
            return true;//то да, мы близко
        else return false;//если нет, то нет
    }

    void OpenStorageUi()//открываем юи банка
    {
        storageUI.enabled = true;//разрешаем канву
        Inventory.isStoraging = true;//говорим, что работаем с банком
        GameObject.Find("InventoryUI").GetComponent<Canvas>().enabled = true;//открываем сумку игрока
    }

    void CloseStorageUi()//закрываем юи банка
    {
        storageUI.enabled = false;//закрываем канву банка
        Inventory.isStoraging = false;//говорим, что не работаем с инвентарм
    }

    public void StorageAddItem(int id)
    {
        for (int i = 0; i < slotsAmount; i++)//цикл по количетсву слотов
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

    void SentStorageData()//функция отправляет состояние банка в инвентарь, ктоторый всегда с игроком
    {
        for (int i = 0; i < slotsAmount; i++)//по всем слотам банка
        {
            if (slots[i].transform.childCount == 0)//если нет ребенок-преддет
                Inventory.storageData[i] = -1;//то в массив состояния банка отправляем -1, значит пусто
            else Inventory.storageData[i] = slots[i].GetComponentInChildren<ItemData>().item.id;//если есть дети, отправляем в массив айди предмета в слоте
        }
    }

    void UpdateStorage()//обновляем банк
    {
        for (int i = 0; i < slotsAmount; i++)//по всем ячейкам банка
        {
            if (Inventory.storageData[i] != -1)//если в массиве элемент не равен -1, это значит, что есть айди предмета
                StorageAddItem(Inventory.storageData[i]);//добавляем этот предмет в банк
        }
    }

    //отправляем состояние банка для сохранения
    public static void SendStorageDataToSave()
    {
        for (int i = 0; i < slotsAmount; i++)//идем по всем слотам банка
        {
            if (slots[i].transform.childCount == 0)//если нет детей, значит нет предметов
                PlayerData.data.STORAGE[i] = -1;//посылаем в массив состояний -1 - пусто
            else PlayerData.data.STORAGE[i] = slots[i].GetComponentInChildren<ItemData>().item.id;//иначе отправляем айди предмета
        }
    }

    //загружааем банк
    void LoadStorage()
    {
        for (int i = 0; i < slotsAmount; i++)//по всем ячейкам банка
        {
            if (SaveLoad.savedGame.STORAGE[i] != -1)//в загруженном массиве есть элемент, который не равен -1
                StorageAddItem(SaveLoad.savedGame.STORAGE[i]);//добавляем предмет, считая его айди как загруженный элемент массива
        }
    }
}
