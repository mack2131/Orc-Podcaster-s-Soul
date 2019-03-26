using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class StoryBooksDatabase : MonoBehaviour {
    [SerializeField]
    public List<Story> storyDatabase = new List<Story>();//лист всех вещей
    public JsonData storyData;//файл json с праметрами вещей

    // Use this for initialization
    void Start()
    {
        //открываем и читаем файл с параметрами всех вещей в папке /StreamingAssests/Story.orc
        storyData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "//StreamingAssets/Story.orc"));
        ConstructStoryDatabse();//фукнция построения базы объектов
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ConstructStoryDatabse()//функция построения базы объектов
    {
        for (int i = 0; i < storyData.Count; i++)//цикл по количеству всех вещей
        {
            //добавляем в лист всех вещей новую вещь с параметрами, которые прочитали в json файле
            storyDatabase.Add(new Story((int)storyData[i]["id"], storyData[i]["story"].ToString()));
        }
    }

    public Story FetchStoryById(int id)//получаем вещь по ее айди
    {
        for (int i = 0; i < storyData.Count; i++)//идем по всем вещам
        {
            if (storyDatabase[i].id == id)//если в списке веще есть вещь с айди
            {
                return storyDatabase[i];//возвращаем эту вещь
            }
        }
        return null;//если нет сопадения по айди, то ничего не взвращаем
    }
}

public class Story //собственно, класс вещи
{
    /*====все атрибуты книги======*/
    public int id { get; set; }
    public string story { get; set; }
    /*=============================*/

    //Конструктор вещи
    public Story(int id, string story)
    {
        //выставляем параметры
        this.id = id;
        this.story = story;
    }

    //если создаем вещь без параметро, то она как бы есть, но пустая и имеет айди -1...как бы есть, но ее как бы нет
    public Story()
    {
        this.id = -1;
    }
}
