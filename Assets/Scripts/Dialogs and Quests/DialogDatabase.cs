using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class DialogDatabase : MonoBehaviour {

    [SerializeField]
    public List<Dialog> database = new List<Dialog>();//лист всех вещей
    public JsonData dialogData;//файл json с праметрами вещей

	// Use this for initialization
	void Start () 
    {
        //открываем и читаем файл с параметрами всех вещей в папке /StreamingAssests/Items.json
        dialogData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "//StreamingAssets/Dialogs.orc"));
        ConstructDialogDatabse();//фукнция построения базы объектов
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void ConstructDialogDatabse()//функция построения базы объектов
    {
        for (int i = 0; i < dialogData.Count; i++)//цикл по количеству всех вещей
        {
            //добавляем в лист всех вещей новую вещь с параметрами, которые прочитали в json файле
            database.Add(new Dialog((int)dialogData[i]["id"],
                                  dialogData[i]["dialog"].ToString()));
        }
    }

    public Dialog FetchDialogById(int id)//получаем вещь по ее айди
    {
        for (int i = 0; i < dialogData.Count; i++)//идем по всем вещам
        {
            if (database[i].id == id)//если в списке веще есть вещь с айди
            {
                return database[i];//возвращаем эту вещь
            }
        }
        return null;//если нет сопадения по айди, то ничего не взвращаем
    }

    public class Dialog //собственно, класс диалога
    {
        /*====все атрибуты вещи======*/
        public int id { get; set; }
        public string dialog { get; set; }
        /*=============================*/

        //Конструктор вещи
        public Dialog(int id, string dialog)
        {
            //выставляем параметры
            this.id = id;
            this.dialog = dialog;
        }

        //если создаем вещь без параметро, то она как бы есть, но пустая и имеет айди -1...как бы есть, но ее как бы нет
        public Dialog()
        {
            this.id = -1;
        }
    }

}
