/* sounds[0] - click music                                                                */





using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using LitJson;

public class ItemData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerClickHandler {

    public Item item;//объект вещь, которая ассоциируется с картинкой вещи Image
    public int amount;//количество вещей в стаке
    public int slotNumber;//номер слота, где вещь
    public string slotName;//имя слота, где вещь
    public GameObject hint;//объект, куда выводится описание вещи

    public GameObject itemDropToTheWorld;//префаб ставится в префаб самой вещи-картинки, куда надет этот скрипт

    private bool isDragging;//вещь перемещается в данный момент
    private bool isDraged;//перемщен ли был?

    public AudioSource[] sounds;//звуки 

	// Use this for initialization
	void Start () 
    {
        hint = GameObject.Find("Item Hint");//ищем объект, куда выводтся краткая инфа об объекте
        hint.GetComponent<Image>().enabled = false;//прячем его, чтобы глаза не мазолил
        sounds = GetComponents<AudioSource>();//инициализируем звуки
        this.name = item.slug;
        DeleteHint();
	}
	
	// Update is called once per frame
	void Update () 
    {
        slotName = "Inventory Slot " + slotNumber;//задаем имя слота, где находится вещь
    }

    //СОБЫТИЕ, КОГДА КУРСОР ВХОДИТ ВНУТРЬ ИЗОБРАЖЕНИЯ
    #region IPointerEnterHandler Members

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowHint();//вызываем функцию показа подсказки
    }

    #endregion

    //СОБЫТИЕ, КОГДА КУРСОР ВЫХОДИТ ИЗ ИЗОБРАЖЕНИЯ
    #region IPointerExitHandler Members

    public void OnPointerExit(PointerEventData eventData)
    {
        DeleteHint();//вызываем функцию скрыть подсказку
    }

    #endregion

    void ShowHint()//рисуем подсказку
    {
        hint.GetComponent<Image>().enabled = true;//разрешаем видимость подсказки к объекту
        Text itemTitle;//объект для того, чтобы можно было поменять цвет текста имени в соотвествии с редкостью вещи
        itemTitle = hint.transform.GetChild(0).GetComponent<Text>();//берем объект, отвечающий за имя вещи
        itemTitle.color = ChangeTitleColor(item.rarity);//присваиваем цвет текста имени в соотвествии с редкостью вещи
        hint.transform.GetChild(0).GetComponent<Text>().text = item.title;//имя предмета
        hint.transform.GetChild(1).GetComponent<Text>().text = item.description;//описание предмета
        hint.transform.GetChild(2).GetComponent<Text>().text = item.power.ToString(); //сила предмета
        hint.transform.GetChild(3).GetComponent<Text>().text = item.vitality.ToString();//жизни от предмета
        hint.transform.GetChild(4).GetComponent<Text>().text = item.value.ToString();
        hint.transform.GetChild(5).GetComponent<Image>().enabled = true;//включаем отображение значка жизни от предмета
        hint.transform.GetChild(6).GetComponent<Image>().enabled = true;//включаем отображение значка урона от предмета
        hint.transform.GetChild(7).GetComponent<Image>().enabled = true;
    }

    public void DeleteHint()//удаляем подсказку
    {
        hint.GetComponent<Image>().enabled = false;//прячем подсказку
        hint.transform.GetChild(0).GetComponent<Text>().text = "";//имя предмета
        hint.transform.GetChild(1).GetComponent<Text>().text = "";//описание предмета
        hint.transform.GetChild(2).GetComponent<Text>().text = "";//сила предмета
        hint.transform.GetChild(3).GetComponent<Text>().text = "";//жизни от предмета
        hint.transform.GetChild(4).GetComponent<Text>().text = "";
        hint.transform.GetChild(5).GetComponent<Image>().enabled = false;//включаем отображение значка жизни от предмета
        hint.transform.GetChild(6).GetComponent<Image>().enabled = false;//включаем отображение значка урона от предмета
        hint.transform.GetChild(7).GetComponent<Image>().enabled = false;
    }

    Color ChangeTitleColor(int itemRarity)//функция возврата цвета в зависимости от редкости предмета
    {
        switch (itemRarity)//смотрим редкость, в зависимости от редкости отправляем цвет , в который окрасятся буквы
        {                                  
            case 0:// обычные     - белые 
                {
                    Color32 grey = new Color32(176, 196, 222, 255);
                    return grey;
                }
            case 1:// необычные   - зеленые
                {
                    Color32 green = new Color32(154, 205, 50, 255);
                    return green;
                }
            case 2:// редкие      - синие 
                {
                    Color32 blue = new Color32(70, 130, 180, 255);
                    return blue;
                }
            case 3:// эпические   - фиолетовые
                {
                    Color32 viol = new Color32(75, 0, 130, 255);
                    return viol;
                }
            case 4:// легендарные - оранжевые  
                {
                    Color32 orange = new Color32(255, 169, 69, 255);
                    return orange;
                }
            case 5://квествоые - голубые
                {
                    Color32 lightBlue = new Color32(0, 255, 255, 255);
                    return lightBlue;
                }
            default://если вдруг не совпадает ни одна редкость, то вернем черный цвет и тестировщики, скажут, на каком предмете все сломалось
                {
                    return Color.black;
                }
        }
    }

    //СОБЫТИЕ, КОГДА ПЕРЕМЕЩАЕМ ИЗОБРАЩЕНИЕ С ЗАЖАТОЙ ЛЕВОЙ КНОПКОЙ МЫШИ
    #region IDragHandler Members

    public void OnDrag(PointerEventData eventData)
    {
        if (!Inventory.isTrading && !Inventory.isStoraging)//если не открыт магазин(торговец) или банк
        {
            DeleteHint();//удаляем подсказку
            isDragging = true;//сейчас перемежается вещь
            //transform.SetParent(GameObject.Find("Slot Panel").transform);//ставим родителя панель инветаря
            transform.SetParent(GameObject.Find("Inventory Panel").transform);//ставим родителя панель инветаря
            transform.position = Input.mousePosition;//перемещаем за мышкой
            Inventory.items[slotNumber].id = -1;//какбудто нет вещи в слоте
        }
    }

    #endregion

    //СОБЫТИЕ, КОГДА КЛИКАЕМ НА ИЗОБРАЖЕНИЕ
    #region IPointerClickHandler Members

    public void OnPointerClick(PointerEventData eventData)
    {
        sounds[0].Play();//проигрываем звук клика

        if (Input.GetMouseButtonUp(0))//если подняли левую кнопку мыши, т.е. переместили вещь и в конце отпустили лкм
        {   //если перемещаемая вещь близко к слоту, который служит выбросить вещь в мир
            /*if (Vector2.Distance(this.transform.position, Inventory.slots[19].transform.position) < 30)
            {
                DropItemToTheFuckingWorld();//выкидываем вещь нахрен
                Destroy(gameObject);//ломает объект в ивенторе 
            }*/

            for (int i = 0; i < 100; i++)//идем по слотам сумки
            {   //если расстояние между изображением вещи и ближайшем из слотов меньше заданного и 
                if (Vector2.Distance(this.transform.position, Inventory.slots[i].transform.position) < 30 &&
                    Inventory.slots[i].transform.childCount == 0)//у слота нет детей(т.е. слот свободный)
                {
                    DropToSlot(i);//фузываем функцию бросить вещь в слот с параметром i, который отвечает за номер свободного слота
                    break;//прерываем цикл, как нашли подходящую ячейку
                }
                else isDraged = false;//после того как отпустили лкм и не нашли нужный слот
            }
            if (isDraged == false)//если нет нужного слота после перетаскивания
                ReturnItem();//возвращаем вещь
        }

        if (Input.GetMouseButtonUp(1) && !Inventory.isTrading && !Inventory.isStoraging)//если нажата правая кнопка мыши и не включен режим торговца, срабатывает, когда подимаем кнопку после клика
        {
            UseItem();//используем вещь
        }
        else if (Input.GetMouseButtonUp(1) && Inventory.isTrading && item.type != "quest")//если жмем ПКМ и включен магазин
        {
            //если вещь находится в инвентаре игрока, то при ПКМ - продаем вещь
            if(gameObject.transform.parent.name != "Vendor Slot " + slotNumber.ToString()) 
                SellItem();

            //если вещь находится в инвенторе вендора, то покупаем вещь
            if (gameObject.transform.parent.name == "Vendor Slot " + slotNumber.ToString())
                BuyItem();
        }
        else if (Input.GetMouseButtonUp(1) && Inventory.isStoraging)//если ПКМ и работаем с банком
        {
            //если вещь находится в инвентаре игрока, то при ПКМ - продаем вещь
            if (gameObject.transform.parent.name != "Storage Slot " + slotNumber.ToString())
                SaveToStorage();//помещаем вещь в банк

            //если вещь находится в инвенторе вендора, то покупаем вещь
            if (gameObject.transform.parent.name == "Storage Slot " + slotNumber.ToString())
                TakeFromStorage();//берем вещь из банка
        }


    }

    #endregion

    public void DropToSlot(int slotNum)//функция бросить вещь в слот после перемещения
    {
        if (Inventory.slots[slotNum].transform.childCount == 0)//если у слота нет детей, а значит он пустой
        {
            transform.SetParent(Inventory.slots[slotNum].transform);//ставим этот слот родителем
            transform.position = transform.parent.position;//меняем позицию
            slotNumber = slotNum;//передаем номер слота
            isDraged = true;//??? я так и не помню, что эта за переменная, но удалять страшно
        }
    }

    void ReturnItem()//функция возвращения вещи
    {
        if (item.type == "weapon" && slotNumber == 101)//если есть вещь в слоте оружия и тип перемещаемого предмета - оружие
        {
            transform.SetParent(Inventory.slots[101].transform);//возвращаем в слот оружия
            transform.position = GameObject.Find("Weapon Slot Position").transform.position;//ставим позицию слота оружия
        }
        if (item.type == "helmet" && slotNumber == 100)//если есть вещь в слоте оружия и тип перемещаемого предмета - оружие
        {
            transform.SetParent(Inventory.slots[100].transform);//возвращаем в слот оружия
            transform.position = GameObject.Find("Head Slot Position").transform.position;//ставим позицию слота оружия
        }
        else//если это просто предмет из инвентаря
        {
            transform.SetParent(Inventory.slots[slotNumber].transform);//возвращаем его в слот, откуда взяли
            transform.position = transform.parent.position;            //и присваеваем позицию слота
        }
    }

    public void UseItem()//использование вещи
    {
        switch (item.type)//смотрим, какой у вещи тип
        {
            case "weapon"://если оружие
                {
                    if (Inventory.slots[101].transform.childCount == 0)//нет ли в слоте оружия уже что-то
                    {//если нет в слоте оружия
                        transform.SetParent(Inventory.slots[101].transform);//ставим родителем слот оружия
                        slotNumber = 101;//номер слота равен номеру слота оружия
                        transform.position = Inventory.slots[101].transform.position;
                    }
                    else
                    {
                        if (Inventory.slots[101].transform.GetChild(0).name == this.name)
                        {
                            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(item.id);
                            Destroy(this.gameObject);
                        }
                        else
                        {
                            int newId = Inventory.slots[101].transform.GetChild(0).GetComponent<ItemData>().item.id;
                            Destroy(Inventory.slots[101].transform.GetChild(0).gameObject);

                            transform.SetParent(Inventory.slots[101].transform);//ставим родителем слот оружия
                            slotNumber = 101;//номер слота равен номеру слота оружия
                            transform.position = Inventory.slots[101].transform.position;

                            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(newId);
                        }
                    }

                    break;
                }
            case "helmet"://если голова
                {
                    if (Inventory.slots[100].transform.childCount == 0)//нет ли в слоте головы уже что-то
                    {//если нет в слоте головы
                        transform.SetParent(Inventory.slots[100].transform);//ставим родителем слот головы
                        slotNumber = 100;//номер слота равен номеру слота головы
                        transform.position = Inventory.slots[100].transform.position;
                    }
                    else
                    {
                        if (Inventory.slots[100].transform.GetChild(0).name == this.name)
                        {
                            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(item.id);
                            Destroy(this.gameObject);
                        }
                        else
                        {
                            int newId = Inventory.slots[100].transform.GetChild(0).GetComponent<ItemData>().item.id;
                            Destroy(Inventory.slots[100].transform.GetChild(0).gameObject);

                            transform.SetParent(Inventory.slots[100].transform);//ставим родителем слот голова
                            slotNumber = 100;//номер слота равен номеру слота голова
                            transform.position = Inventory.slots[100].transform.position;

                            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(newId);
                        }
                    }

                    break;
                }
            case "food":
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().Healing(item.vitality);
                    DeleteHint();
                    Destroy(gameObject);
                    break;
                }
        }
    }

    public void DropItemToTheFuckingWorld()//функция выкидывающая вещь из персонажа
    {
        GameObject newItem = Instantiate(itemDropToTheWorld);//создаем объект из префаба, который отвечает за визуализацию вещи в открытом мире
        //ставим позицию спавна вещи выше, чем сам игрок, дабы не сталкивались и не разлетались
        newItem.transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x,
                                                 GameObject.FindGameObjectWithTag("Player").transform.position.y + 7,
                                                 GameObject.FindGameObjectWithTag("Player").transform.position.z);
        newItem.GetComponentInChildren<MeshFilter>().GetComponentInChildren<PressTheTextItemTitle>().item = item;//говорим выброшенной вещи, какя она
    }

    void SellItem()//функция продажи вещи
    {
        Inventory.money += item.value;//прибавляем цену вещи
        Destroy(gameObject);//уничтожаем объект
    }

    void BuyItem()//функция покупки вещи (=добавляем вещь в инвентарь игрока=)
    {
        int fullSlot = 0;//счетчик занятых ячеек
        for (int i = 0; i < 100; i++)//для этого, по всем 16 слотам инвенторя игрока пробежим
        {
            if (Inventory.slots[i].transform.childCount == 1)//и посмотрим, есть ли дети у ячеек слотов
                fullSlot++;//если есть, то увеличиваем счетчик заполненных слотов
        }
        if (fullSlot == 100)//если счетчик полных слотов равен количеству всех слотов
        {
            fullSlot = 0;//значит инвентарь полон и ничего не делаем, обнуляем счетчик для дальнейшего счета
        }
        else//если есть места
        {   //вызываем функцию добавления вещи в инвентарь персонажа
            if (Inventory.money - item.value >= 0)//если деньг хватает на покупку
            {   //добавляем игроку эту вещь в инвентарь
                GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(item.id);
                Inventory.money -= item.value;//вычитаем цену из денег игрока
            } 
            fullSlot = 0;//обнуляем счетчик для дальнейшего счета
        }
    }


    void SaveToStorage()//функция, помещабщая вещь в банк
    {
        int fullSlot = 0;//счетчик занятых ячеек
        for (int i = 0; i < Storage.slotsAmount; i++)//для этого, по всем 16 слотам инвенторя игрока пробежим
        {
            if (Storage.slots[i].transform.childCount == 1)//и посмотрим, есть ли дети у ячеек слотов
                fullSlot++;//если есть, то увеличиваем счетчик заполненных слотов
        }
        if (fullSlot == Storage.slotsAmount)//если счетчик полных слотов равен количеству всех слотов
        {
            fullSlot = 0;//значит инвентарь полон и ничего не делаем, обнуляем счетчик для дальнейшего счета
        }
        else
        {
            GameObject.Find("Player Bank").GetComponent<Storage>().StorageAddItem(item.id);//если есть места
            Destroy(gameObject);
        }
    }

    void TakeFromStorage()//функция, дает игроку вещь из банка
    {
        int fullSlot = 0;//счетчик занятых ячеек
        for (int i = 0; i < 100; i++)//для этого, по всем 16 слотам инвенторя игрока пробежим
        {
            if (Inventory.slots[i].transform.childCount == 1)//и посмотрим, есть ли дети у ячеек слотов
                fullSlot++;//если есть, то увеличиваем счетчик заполненных слотов
        }
        if (fullSlot == 100)//если счетчик полных слотов равен количеству всех слотов
        {
            fullSlot = 0;//значит инвентарь полон и ничего не делаем, обнуляем счетчик для дальнейшего счета
        }
        else//если есть места
        {   //добавяем вещь в инвентарь персонажа
            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(item.id);
            Destroy(gameObject);
        }
    }
}
