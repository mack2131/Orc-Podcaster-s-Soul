using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mob : MonoBehaviour {

    public float health;//текущее здоровье моба
    public float maxHealth;//максимаоьное здоровье моба
    public float damage;//урон
    public float speed;//скорость передвижения
    public float range;//расстояние, на котором доступен удар
    public float rangeRadius;//радиус, на котором моб сагрится на игрока
    public float escapeRadius;
    public int givenExp = 400;//опыт за убийство
    public Transform player;//игрок
    public CharacterController controller;//котроллер моба для движения
    public float impactTime;//вермя атаки
    private float impactLength;//длина атак, процент проигрывания анимации, когда визуально есть удар
    private bool impacted;//был ли удар
    private Fighter opponent;//цель - это игрок

    public GameObject Funfetti;
    public int funMoney;

    public AnimationClip run;//анимация бега
    public AnimationClip idle;//анимация спокойствия
    public AnimationClip die;//анимация смерти
    public AnimationClip attackAnim;//анимация атаки

    private Animation anim;//сборник всех компонентов анимации
    public Vector3 startPosition;//стартовая позиция, куда вернемся, если игрок сбежит
    public float startPositionRange;//расстояние до стартовой позиции

    public int[] itemsToDropId;
    public GameObject droppedItem;
    public ItemDatabase database;

    public AudioSource[] sounds;

	// Use this for initialization
	void Start () 
    {
        health = maxHealth;//приравниваем здоровье текущее к максимальному
        anim = GetComponent<Animation>();//собираем всю анимацию клипы
        //opponent = player.GetComponent<Fighter>();//ставим игрока целью
        startPosition = transform.position;//запоминаем стартовую позицию

        database = GameObject.Find("Inventory System Manager").GetComponent<ItemDatabase>();

        sounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Debug.Log(anim[attackAnim.name].time);
        if (player == null)//если нет игрока
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;//ищем его
            opponent = player.GetComponent<Fighter>();//ставим игрока целью
        }

        if (player.GetComponent<Fighter>().opponent != null)//если у игрока есть цель
        {   //совпадает ли имя цили с именем этого объекта
            if (player.GetComponent<Fighter>().opponent.name == gameObject.name)
                Highlight();//если да, то делаем подсветку
            else UnHighlight();//если нет убираем подсветку
        }
        else UnHighlight();//если цель сбросилась - убираем подсветку

        if (!IsDead())//если живы
        {
            if (InRangeRadius())//если не в радиусе агро
            {
                if (!InRange())//если не в радиусе удара
                {
                        Chase();//функиця приследования игрока
                }
                else//если в радиусе удара
                {
                    anim.CrossFade(attackAnim.name);//играем анимацию удара
                    Attack();//функиця атаки
                    //если если время проигрывания анимации атакаи больше 95 процентов от длины, т.е. почти закончилась
                    if (anim[attackAnim.name].time > 0.90 * anim[attackAnim.name].length)
                    {
                        impacted = false;//удар закончился
                    }
                }
            }
            else//если не в радиусе агро
            {   //если расстояние до начальной позиции моба меньше выбранного, чтобы моб не висел в воздух
                if (Vector3.Distance(transform.position, startPosition) < startPositionRange)
                    anim.CrossFade(idle.name);//анимацию спокойстви и стоим на месте
                else //если расстояние до начальной позиции больше заданного
                    WalkToStartPosition();//то идем к начальной позиции
            }
        }
        else//если мерты
        {
            anim.CrossFade(die.name);//играем анимацию смерти
            Died();//функция смерти
        }

        if (transform.rotation.x != 0 || transform.rotation.z != 0)
        {
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
	}

    bool InRange()//в радиусе атаки ли?
    {   //если расстояне между мобом и игроком меньше радиуса атаки
        if (Vector3.Distance(transform.position, player.transform.position) < range)
        {
            return true;//значит в радиусе атаке
        }
        else return false;//если нет, то нет
    }

    bool InRangeRadius()//если в радиусе агро
    {   //если расстояне между мобом и игроком меньше радиуса агро
        if(Vector3.Distance(transform.position, player.transform.position) < rangeRadius)
        {
            return true;//значит моб агрится
        }
        else return false;//нет, нет
    }

    void Chase()//преследование
    {
        transform.LookAt(player.position);//смотрим на игрока
        controller.SimpleMove(transform.forward * speed);//двигаемся посредством движения контроллера
        anim.CrossFade(run.name);//играем анимацию бега
    }

    void WalkToStartPosition()//возвращение на стартовую позицию
    {
        transform.LookAt(startPosition);//смотрим на стартовую позицию
        controller.SimpleMove(transform.forward * speed);//двигаемся посредством движения контроллера
        anim.CrossFade(run.name);//играем анимацию бега
        player.GetComponent<Fighter>().opponent = null;//снимаем цель игрока, у игрока теперь нет цели
    }

    void OnMouseOver()//если навдена мышка на моба
    {
        if (Input.GetMouseButtonUp(1) && !IsDead())//нажата правая кнопка мыши
        {
            player.GetComponent<Fighter>().opponent = gameObject;//этот моб становится целью игрока
        }
    }

    public void GetHit(float damage)//получаем урон
    {
        health = health - damage;//вычитаем урон из текущего здоровья
        if (health <= 0)//если здоровье опустилось ниже 0
        {
            health = 0;//поднимаем в 0
        }
    }

    bool IsDead()//умерли ли?
    {
        if (health <= 0)//если здоровье в 0
        {
            return true;//то да, умерли
        }
        else return false;//если нет, то живы
    }

    void Died()//функция смерти
    {
        anim.CrossFade(die.name);//играем анимацю смерт
        if (anim[die.name].time > anim[die.name].length * 0.80)//если текущее время проигрывания анимации больше 90 процентов ее длины
        {
            //player.GetComponent<Fighter>().opponent = null;//убираем у игрока цель
            player.GetComponent<LevelingSystem>().GetExp(givenExp);//даем игроку опыт
            DropLoot();
            Destroy(gameObject);//уничтожаем моба
        }
    }

    /*void OnDestroy()
    {
        DropLoot();
    }*/

    void Attack()//функция атаки
    {       //если время проигрывания анимации больше времени, когда визульно был удар
        if (anim[attackAnim.name].time > anim[attackAnim.name].length * impactTime && 
            !impacted && //и когда не было удара
            anim[attackAnim.name].time < anim[attackAnim.name].length * 0.85)//и анимация удара еще не закончилась
        {
            sounds[0].Play();
            opponent.GetHit(damage);//вызываем функцию урона у игрока
            impacted = true;//удар был
        }
    }

    void DropLoot()//функция сброса лута
    {
        GameObject funfetti = Instantiate(Funfetti);
        funfetti.transform.position = transform.position;
        funfetti.GetComponent<Funfetti>().funMoney = funMoney;
        GameObject.Find("Inventory System Manager").GetComponent<Inventory>().TakeMoney(funMoney); ;

        Item[] items = new Item[itemsToDropId.Length];//сами вещи
        //int[,] weights = new int[itemsToDropId.Length, 2];//весы вещей
        //int[,] search = new int[itemsToDropId.Length, 3];//массив для поиска
        int roll = Random.Range(0, 100);//бросок игрока

        for (int i = 0; i < itemsToDropId.Length; i++)//по всем вещам, которые может уронить моб
        {
            items[i] = database.FetchItemById(itemsToDropId[i]);//записываем эти вещи по айди из database
        }

        for(int i = 0; i < itemsToDropId.Length; i++)//по всем вещам, которые может выкинуть моб
        {
            if(items[i].dropRate >= roll)//если шанс выпадения вещи больше, чем выкинул игрок, то...
            {
                GameObject drop = Instantiate(droppedItem);//выкидывае вещь в мир
                //позициб вещи ставим вверх +5 единиц, чтобы не разлетались
                drop.transform.position = new Vector3(transform.position.x, transform.position.y + 7 + i, transform.position.z);
                drop.GetComponentInChildren<PressTheTextItemTitle>().item = items[i];//присваиваем какую вещь выкинули объекту, который отвечает за визуализацию вещи в мире
            }
        }

        /*дроп лута бинарным поиском - не реализован до конца, готовится ряд для половинной выборки, самой выборки нет*/
        /*
        itemsToDropId //массив вещей, которые могут упасть
        item.drop
        item dropeed [start number, end number, itemId]
        for (int i = 0; i < itemsToDropId.Length; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (j == 0)
                    weights[i, j] = items[i].dropRate;
                if (j == 1)
                    weights[i, j] = items[i].id;
            }
        }
        Debug.Log(weights[0, 0] + " " + weights[0, 1]);
        Debug.Log(weights[1, 0] + " " + weights[1, 1]);
        Debug.Log(weights[2, 0] + " " + weights[2, 1]);
        Debug.Log("\n");

 
        for (int i = itemsToDropId.Length - 1; i > 0; i--)
        {
            for (int j = i; j > 0; j--)
            {
                //weights[i, 0] += weights[j - 1, 0];
                maxWeight += weights[j - 1, 0];
            }
        }
        //0-30
        //30-90
        //90-180
        for (int i = 0; i < itemsToDropId.Length; i++)
        {

        }

        Debug.Log(search[0, 0] + " " + search[0, 1] + " " + search[0, 2]);
        Debug.Log(search[1, 0] + " " + search[1, 1] + " " + search[1, 2]);
        Debug.Log(search[2, 0] + " " + search[2, 1] + " " + search[2, 2]);
        */
    }

    void Highlight()//функция подсветки
    {   //ставим скачанны шейдер
        GetComponentInChildren<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
    }

    void UnHighlight()//функция отключения подсвтеки
    {   //возвращаем исходный
        GetComponentInChildren<Renderer>().material.shader = Shader.Find("Diffuse");
    }
        
}
