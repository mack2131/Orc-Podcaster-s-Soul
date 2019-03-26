/* В инвентаре инициализирутся и ставятся все слоты в void Start() посредством добавления */
/* в объект список List<GameObject> slots = new List<GameObject>() методом Instansiate.   */
/* Т.к. в игре должен присутствовать банк и вендор, а в игре 16 слотов у сумки игрока, то */
/* номера слотов с 0 по 15 (16 штук - это сумка), 16 17 18 - это ячейка головы, оружия и  */
/* кольца, то дальше будет идти тако разделение:                                          */
/* 0 - 15 - сумка игрока                                                                  */
/* 16 - ячейка головы                                                                     */
/* 17 - ячейка оружия                                                                     */
/* 18 - ячейка аксессуара                                                                 */
/* 19 - ячейка для выброса предмета в мир                                                 */
/*                                                                                        */
/* ПОСЛЕ 20.03.2018                                                                       */
/* 0 - 100 - сумка игрока                                                                 */
/* 100 - ячейка головы                                                                    */
/* 101 - ячейка оружия                                                                    */
/* 102 - ячейка аксессуара                                                                */

/*                                                                                        */
/*                                                                                        */
/* sounds[0] - open inventory music                                                       */
/* sounds[1] - click music                                                                */
 



using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class Inventory : MonoBehaviour {

    public Canvas inventoryUI;//канва интерфейса инвенторая и игрока
    public ItemDatabase database;//список всех вещей
    //public GameObject inventoryPanel;//панель инвенторя
    public GameObject slotPanel;//панель, где стоит GridLayotGroup, сюда будут ставиться слоты
    public GameObject inventorySlot;//слот для вещи, Image
    public GameObject inventoryItem;//сама вещь, Image

    public static int slotAmount;//количество слотов, для моей игры будет 16
    public static List<Item> items = new List<Item>();//список всех вещей
    public static List<GameObject> slots = new List<GameObject>();//список слотов

    public static bool isTrading;//мы ооргуем сейчас?
    public static bool isStoraging;//мы работаем с банком сейчас?

    public int startMoney;
    public static int money;

    public Text moneyText;

    public static int[] storageData = new int[Storage.slotsAmount];//массив длиною в размер банка игрока, в котором хранится айди предметов, которые есть в банке, если элемент равен -1, то нет предмета

    public AudioSource[] sounds;//звуки инвенторя

    public static int bPressCount = 0;//счетчик нажатий на кнопку В - открыть сумку, инвентарь

    public static int[] lootedLootChests = new int[50];
    public static int[] readedBooks = new int[32];

    public Button closeInventory;


	// Use this for initialization
	void Start () 
    {
        database = GetComponent<ItemDatabase>();//берем с объекта компонент ItemDataBase, этот скрипт и скрипт ItemDataBase должен быть на одном объекте
        inventoryUI.GetComponent<Canvas>().enabled = false;//скрываем интерфейс инвенторя
        slotAmount = 102;//количество слотов 19, потому что 16 - это сумка, остальные 3 - это эквип персонажа и т.д по нарастанию потенциала
        for (int i = 0; i < slotAmount; i++)//цикл по количеству слотов
        {
            if (i < 100)
            {
                items.Add(new Item());//добавляем в список вещей вещь
                slots.Add(Instantiate(inventorySlot));//создаем объект слот
                slots[i].transform.SetParent(slotPanel.transform);//ставим ему родителя объект GridLayotGroup, сюда будут ставиться слоты
                slots[i].transform.localScale = new Vector3(0.9999977f, 0.9999977f, 0.9999977f);//подгоняем размер
                slots[i].name = "Inventory Slot " + i.ToString();//приписываем объекту имя
            }
            if (i == 100) //голова
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));
                slots[i].transform.SetParent(GameObject.Find("Head Slot Position").transform);
                slots[i].transform.position = slots[i].transform.parent.position;
                slots[i].name = "Inventory Slot " + i.ToString();
            }
            if (i == 101) //оружие
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));
                slots[i].transform.SetParent(GameObject.Find("Weapon Slot Position").transform);
                slots[i].transform.position = slots[i].transform.parent.position;
                slots[i].name = "Inventory Slot " + i.ToString();
            }
            /*if (i == 102) //кольцо
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));
                slots[i].transform.SetParent(GameObject.Find("Accessory Slot Position").transform);
                slots[i].transform.position = slots[i].transform.parent.position;
                slots[i].name = "Inventory Slot " + i.ToString();
            }*/
            /*if (i == 19) //выбрасываем предмет в мир
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventorySlot));
                slots[i].transform.SetParent(GameObject.Find("Drop Item Slot").transform);
                slots[i].transform.position = slots[i].transform.parent.position;
                slots[i].name = "Inventory Slot " + i.ToString();
            }*/
        }

        sounds = GetComponents<AudioSource>();//инициализируем звуки

        
        CleanStorageData();

        closeInventory.onClick.AddListener(ClickCancel);
        if(!IsItemInInventory(53))
            PlayerAddItem(53);/* добавляем золотой пончик */
        //если была загрузка
        if (SaveLoad.wasLoaded == true)
        {
            LoadInventory();//загружаем инвентарь
            SaveLoad.inventoryLoaded = false;//говорим, что загрузили инвентарь
        }
        else money = startMoney;

        //isTrading = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        OnOffInventoryUi();//функция показа-скрыть интерфейс инвенторя
        moneyText.text = money.ToString();

        if (money <= 0)//деньги не могут быть меньше 0
            money = 0;
	}

    void OnOffInventoryUi()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isStoraging && !isTrading && !Pause.isPause)//если нажали кнопку
        {
            bPressCount++;//увеличиваем счетчик
            if (bPressCount == 1)//если нажали раз
            {
                sounds[0].Play();//звук открывания инвенторя
                inventoryUI.GetComponent<Canvas>().enabled = true;//показываем инвентарь
            }
            if (bPressCount == 2)//если нажали второй раз
            {
                sounds[0].Play();//звук открывания инвенторя
                inventoryUI.GetComponent<Canvas>().enabled = false;//скрываем инвентарь
                bPressCount = 0;//обнуляем счетчик
            }
        }
    }

    void ClickCancel()
    {
        sounds[0].Play();//звук открывания инвенторя
        inventoryUI.GetComponent<Canvas>().enabled = false;//скрываем инвентарь
        bPressCount = 0;//обнуляем счетчик
    }

    public bool IsItemInInventory(int id)//функция проверки, находится ли вещь в инветоре
    {
        for (int i = 0; i < 102; i++)//по списку всех веще в сумке
        {
            if (slots[i].transform.childCount !=0 && slots[i].GetComponentInChildren<ItemData>().item.id == id)//если айди совпадает
                return true;//значит такая вещь есть, правда
        }
        return false;//значит, нет такой вещи
    }

    public void PlayerAddItem(int id)
    {
        for (int i = 0; i < 100; i++)//цикл по количетсву слотов
        {
            if (slots[i].transform.childCount == 0)//если нет детей, т.е. нет вещей в слоте, т.е. слот пустой
            {
                Item addItem = database.FetchItemById(id);//берем вещь из списка вещей
                GameObject newItem = Instantiate(inventoryItem);//создаем объект вещь Image 
                newItem.GetComponent<ItemData>().item = addItem;//передаем параметры
                newItem.transform.SetParent(slots[i].transform, false);//ставим родителя
                newItem.transform.position = newItem.transform.parent.transform.position;//меняем позицию
                newItem.GetComponent<Image>().sprite = addItem.icon;//ставим иконку
                newItem.GetComponent<ItemData>().slotNumber = i;//передаем номер слота
                break;
            }
        }
    }

    public void PlayerAddEquipment(int id, int slotNumber)//функция добавления предметов в экипировку, вызывается при загрузке в скрипте PlayerEquipment.cs
    {
        Item addItem = database.FetchItemById(id);//берем вещь из списка вещей
        GameObject newItem = Instantiate(inventoryItem);//создаем объект вещь Image 
        newItem.GetComponent<ItemData>().item = addItem;//передаем параметры
        newItem.transform.SetParent(slots[slotNumber].transform, false);//ставим родителя
        newItem.transform.position = newItem.transform.parent.transform.position;//меняем позицию
        newItem.GetComponent<Image>().sprite = addItem.icon;//ставим иконку
        newItem.GetComponent<ItemData>().slotNumber = slotNumber;//передаем номер слота
    }

    void CleanStorageData()//очищаем банк при старте
    {
        for (int i = 0; i < Storage.slotsAmount; i++)
            storageData[i] = -1;//минус 1 значит, что нет вещей
    }

    //функция отправляет состояние инвенторя для сохранения
    public static void SendInventoryState()
    {
        for (int i = 0; i < 100; i++)//идем по всем ячейкам
        {
            if (slots[i].transform.childCount == 0)//если нет предметов в слоте
                PlayerData.data.INVENTORY[i] = -1;//отправляем -1, значит пусто
            else PlayerData.data.INVENTORY[i] = slots[i].GetComponentInChildren<ItemData>().item.id;//если есть предмет, то отправляем его айди
        }

        PlayerData.data.LOOTEDCHESTS = lootedLootChests;
        PlayerData.data.READEDBOOKS = readedBooks;
    }

    //функция загрузки инветоря
    void LoadInventory()
    {
        for (int i = 0; i < 100; i++)//по всем ячейкам
        {
            if (SaveLoad.savedGame.INVENTORY[i] != -1)//если не пусто,значит есть айди предмета
                PlayerAddItem(SaveLoad.savedGame.INVENTORY[i]);//добавляем его в инвентарь
        }
        money = SaveLoad.savedGame.MONEY;

        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().LoadLevelExp();

        lootedLootChests = SaveLoad.savedGame.LOOTEDCHESTS;
        readedBooks = SaveLoad.savedGame.READEDBOOKS;
    }

    public void TakeMoney(int amount)
    {
        money += amount;
    }
}
