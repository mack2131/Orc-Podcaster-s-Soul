// у пердметов есть редкость:           //
// обычные     - белые        - 0       //
// необычные   - зеленые      - 1       //
// редкие      - синие        - 2       //
// эпические   - фиолетовые   - 3       //
// легендарные - оранжевые    - 4       //

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class PressTheTextItemTitle : MonoBehaviour {

    public int itemId;//айди выбошенной вещи
    public Item item = null;//данные о вещи
    public ItemDatabase database;

    private int lifeTimeCount;//текущие секунды жизни предмета, когда игрок выкинул из инвентаря

    private int fullSlot = 0;//счетчик заполненных слотов в инвентаре

	// Use this for initialization
	void Start () 
    {
		InvokeRepeating("LifeCycle", 1, 1);//запускаем отсчет счетчика каждую секунду на жизнь предмета
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.LookAt(Camera.main.transform);//чтобы текст смотрел в камеру изменить Scale X на -1 !!!!!!!! иначе будет перевернутым
        if (item == null)//если вещь выброшена из инспектора, а не из персонажа, то ей задается айди
        {   //мы берем этот айди
            item = GameObject.Find("Inventory System Manager").GetComponent<ItemDatabase>().FetchItemById(itemId);
            itemId = item.id;//и присваиваем вещи
        }
        else//если уже есть айди
        {
            this.transform.parent.transform.parent.name = item.slug;//присваиваем имя слаг имени объекту
            gameObject.GetComponent<TextMesh>().text = item.title;//меняем имя
            itemId = item.id;//меняем айди на выброшенную вещь
        }
        ParticleColor(item.rarity);

        if (lifeTimeCount > 300)//если счетчик больше 300 с, то объект должен исчезнуть
            Destroy(this.transform.parent.transform.parent.gameObject);//удаляем весь объект... см.иерархию родителей и детей, чтобы удалился весь объектDebug.Log("item is dead");
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1) && InRange())//если нажали пкм и находимся в радиусе сбора
        {   //проверяем, есть ли в инвентаре свободное место
            for (int i = 0; i < 100; i++)//для этого, по всем 16 слотам пробежим
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
                GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(itemId);
                Destroy(this.transform.parent.transform.parent.gameObject);//удаляем весь объект... см.иерархию родителей и детей, чтобы удалился весь объект
                fullSlot = 0;//обнуляем счетчик для дальнейшего счета
            }
        }
    }

    void ParticleColor(int itemRarity)//функция, меняющая цвет частиц и текста имени в соответсвии с редкость. предмета
    {
        switch (itemRarity)//смотрим редкость, в зависимости от редкости меняем цвет частиц и цвет букв, которыми
        {                  //                                               написано имя предмета                 
            case 0:// обычные     - белые 
                {
                    transform.parent.transform.parent.GetComponentInChildren<ParticleSystem>().startColor = Color.white;
                    gameObject.GetComponent<TextMesh>().color = Color.white;
                    break;
                }
            case 1:// необычные   - зеленые
                {
                    transform.parent.transform.parent.GetComponentInChildren<ParticleSystem>().startColor = Color.green;
                    gameObject.GetComponent<TextMesh>().color = Color.green;
                    break;
                }
            case 2:// редкие      - синие 
                {
                    transform.parent.transform.parent.GetComponentInChildren<ParticleSystem>().startColor = Color.blue;
                    gameObject.GetComponent<TextMesh>().color = Color.blue;
                    break;
                }
            case 3:// эпические   - фиолетовые
                {
                    transform.parent.transform.parent.GetComponentInChildren<ParticleSystem>().startColor = new Color(75, 0, 130);
                    gameObject.GetComponent<TextMesh>().color = new Color(75, 0, 130);
                    break;
                }
            case 4:// легендарные - оранжевые  
                {
                    transform.parent.transform.parent.GetComponentInChildren<ParticleSystem>().startColor = new Color(255, 165, 0);
                    gameObject.GetComponent<TextMesh>().color = new Color(255, 165, 0);
                    break;
                }
        }
    }

    void LifeCycle()//функция, которая увеличивает счетчик жизни премета
    {
        lifeTimeCount++;//увеличиваем счетчик
    }

    private bool InRange()//находимся ли мы в радиусе подбора вещи
    {   //если расстояние между игроком и вещью меньге числа, которое подбиралось опытным путем
        if (Vector3.Distance(this.transform.position, GameObject.Find("PlayerOrkpod").transform.localPosition) < 6.5)
            return true;//возвращаем правду
        else return false;//если нет, то фальш

    }
}
