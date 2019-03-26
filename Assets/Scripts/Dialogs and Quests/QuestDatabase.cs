using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class QuestDatabase : MonoBehaviour {

    [SerializeField]
    public List<Quest> database = new List<Quest>();//лист всех вещей
    public JsonData questData;//файл json с праметрами вещей

	// Use this for initialization
	void Start () 
    {
        //открываем и читаем файл с параметрами всех вещей в папке /StreamingAssests/Items.json
        questData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "//StreamingAssets/Quests.orc"));
        ConstructQuestDatabse();//фукнция построения базы объектов
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void ConstructQuestDatabse()//функция построения базы объектов
    {
        for (int i = 0; i < questData.Count; i++)//цикл по количеству всех вещей
        {
            //добавляем в лист всех вещей новую вещь с параметрами, которые прочитали в json файле
            database.Add(new Quest((int)questData[i]["quest_id"],
                                  questData[i]["quest_name"].ToString(),
                                  questData[i]["description"].ToString()));
        }
    }

    public Quest FetchQuestById(int id)//получаем вещь по ее айди
    {
        for (int i = 0; i < questData.Count; i++)//идем по всем вещам
        {
            if (database[i].questId == id)//если в списке веще есть вещь с айди
            {
                return database[i];//возвращаем эту вещь
            }
        }
        return null;//если нет сопадения по айди, то ничего не взвращаем
    }

    public class Quest //собственно, класс диалога
    {
        /*====все атрибуты вещи======*/
        public int questId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        /*=============================*/

        //Конструктор вещи
        public Quest(int id, string name, string description)
        {
            //выставляем параметры
            this.questId = id;
            this.name = name;
            this.description = description;
        }

        //если создаем вещь без параметро, то она как бы есть, но пустая и имеет айди -1...как бы есть, но ее как бы нет
        public Quest()
        {
            this.questId = -1;
        }
    }
}
