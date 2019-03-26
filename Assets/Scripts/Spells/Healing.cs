using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Healing : MonoBehaviour {

    public GameObject healingDonutPrefab;//префаб для пончика
    public Transform healingDonutStartPos;//стартовая позиция пончика, где он поялвяется
    private GameObject[] donuts;//массив объектов лечащих пончиков
    public Text outputDonutsAmount;//текст, куда выводится количество доступных пончиков
    public Image cooldown;//изображение, отвечающее за откат
    public int currentDonuts;//текущее кол-во пончиков
    public int maxDonuts;//максимальное кол-во пончиков
    public float cooldownTime;//время отката
    public static int heal;//сколько хилим
    private float counter;//счетчик текущего времени
    private bool isRestoring;//идет ли восстановление пончика

	// Use this for initialization
	void Start () 
    {
        InstantiateDonut();//спавним пончик
        maxDonuts = 5;
        cooldownTime = 30;
        heal = 30;
	}
	
	// Update is called once per frame
	void Update () 
    {
        ShowHealingDonutsAmount();//выводим количество пончиков в птекстовое поле
        Restore();//функция восстановления

		if(Input.GetKeyDown(KeyCode.Q) && DonutsAmount() != 0)//если нажали кнопку лечиться и есть пончики
        {
            GetComponent<Fighter>().Healing(heal);//вызвали функцию лечения из Fighter
            Destroy(GameObject.FindGameObjectWithTag("Healing Donut"));//удаляем один пончик
        }
	}

    void InstantiateDonut()//функция спавна пончика
    {
        GameObject healDonut = Instantiate(healingDonutPrefab);//новый объект из префаба
        healDonut.transform.SetParent(GameObject.Find("Healing Donut Storage").transform, false);//ставим ему родителем объект стекляную бочку для пончиков
        healDonut.transform.position = healingDonutStartPos.transform.position;//позицией ставим начальную позицию спавна пончика
        healDonut.transform.localScale = new Vector3(0.6899018f, 0.7291662f, 0.7747734f);//меняем размер (подбирался теоритически)
        healDonut.transform.localRotation = new Quaternion(0, 0, 90, 90);//крутим пончик лицом к игроку

    }

    void ShowHealingDonutsAmount()//функция выожа количества доступных пончиков
    {
        //выводдим инфу в поле текстовое
        outputDonutsAmount.text = GameObject.FindGameObjectsWithTag("Healing Donut").Length.ToString();
    }

    int DonutsAmount()//функция возвращает текущее количество заспавленых пончиков
    {
        donuts = GameObject.FindGameObjectsWithTag("Healing Donut");//пытаемся заполнить элементы массива
        currentDonuts = donuts.Length;//текущее количество пончиков - это длина массива
        return currentDonuts;//возвращае тек кол-во пончиков
    }

    void Restore()//функция восстановления
    {
        if (DonutsAmount() < maxDonuts)//если текущее количество пончиков меньше максимального
        {
            if (isRestoring == false)//если не идет восстановление
            {
                cooldown.fillAmount = 1;//кулдаун на макс
                counter = 0;//счетчик секунд в ноль
                isRestoring = true;//начинаем восстановление
            }
            counter += Time.deltaTime;//увеличиваем счетчик
            if (counter <= cooldownTime && isRestoring == true)//если счетчик меньше времени восстановления пончика и идет восстановление
                cooldown.fillAmount -= 1 / cooldownTime * Time.deltaTime;//уменьшаем визуально кулдаун
            else//иначе
            {
                InstantiateDonut();//спавним пончик
                isRestoring = false;//говорим, что не восстанавливаем
            }
        }

    }
}
