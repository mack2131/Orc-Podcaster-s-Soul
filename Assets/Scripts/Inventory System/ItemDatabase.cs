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
using LitJson;

public class ItemDatabase : MonoBehaviour {
    [SerializeField]
    public List<Item> database = new List<Item>();//лист всех вещей
    public JsonData itemData;//файл json с праметрами вещей

	// Use this for initialization
	void Start () 
    {
        //открываем и читаем файл с параметрами всех вещей в папке /StreamingAssests/Items.json
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "//StreamingAssets/Items.orc"));
        ConstructItemDatabse();//фукнция построения базы объектов
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void ConstructItemDatabse()//функция построения базы объектов
    {
        for (int i = 0; i < itemData.Count; i++)//цикл по количеству всех вещей
        {
            //добавляем в лист всех вещей новую вещь с параметрами, которые прочитали в json файле
            database.Add(new Item((int)itemData[i]["id"],
                                  itemData[i]["type"].ToString(), 
                                  itemData[i]["title"].ToString(), 
                                  (int)itemData[i]["value"],
                                  (int)itemData[i]["stats"]["power"],
                                  (int)itemData[i]["stats"]["vitality"],
                                  itemData[i]["description"].ToString(),
                                  (int)itemData[i]["rarity"],
                                  (int)itemData[i]["drop"],
                                  itemData[i]["slug"].ToString()));
        }
    }

    public Item FetchItemById(int id)//получаем вещь по ее айди
    {
        for (int i = 0; i < itemData.Count; i++)//идем по всем вещам
        {
            if (database[i].id == id)//если в списке веще есть вещь с айди
            {
                return database[i];//возвращаем эту вещь
            }
        }
        return null;//если нет сопадения по айди, то ничего не взвращаем
    }
}

public class Item //собственно, класс вещи
{
    /*====все атрибуты вещи======*/
    public int id { get; set; }
    public string type { get; set; }
    public string title { get; set; }
    public int value { get; set; }
    public int power { get; set; }
    public int vitality { get; set; }
    public string description { get; set; }
    public int rarity { get; set; }
    public int dropRate { get; set; }
    public string slug { get; set; }
    public Sprite icon { get; set; }
    public GameObject model { get; set; }
    /*=============================*/

    //Конструктор вещи
    public Item(int id, string type, string title, int value, int power, int vitality, string description, int rarity, int dropRate, string slug)
    {
        //выставляем параметры
        this.id = id;
        this.type = type;
        this.title = title;
        this.value = value;
        this.power = power;
        this.vitality = vitality;
        this.description = description;
        this.rarity = rarity;
        this.dropRate = dropRate;
        this.slug = slug;
        this.icon = Resources.Load<Sprite>("Items Icon/" + slug);
        this.model = Resources.Load<GameObject>("Mesh/" + slug);
    }

    //если создаем вещь без параметро, то она как бы есть, но пустая и имеет айди -1...как бы есть, но ее как бы нет
    public Item()
    {
        this.id = -1;
    }
}
