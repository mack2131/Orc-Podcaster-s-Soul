using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DogBoss : MonoBehaviour {

    public float health;//текущее здоровье моба
    public float maxHealth;//максимаоьное здоровье моба
    public float mana;
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
    public AnimationClip cast;
    public AnimationClip stun;

    private Animation anim;//сборник всех компонентов анимации
    public Vector3 startPosition;//стартовая позиция, куда вернемся, если игрок сбежит
    public float startPositionRange;//расстояние до стартовой позиции

    public int[] itemsToDropId;
    public GameObject droppedItem;
    public ItemDatabase database;

    public GameObject spears;
    public GameObject stoneEnter;
    public GameObject startBossFightTrigger;

    private bool startFight;
    private bool isArenaClose;
    private bool enableCast;
    private float spearCooldown;
    private bool resetPhase;

    public AudioSource[] sounds;

	// Use this for initialization
	void Start () 
    {
        health = maxHealth;//приравниваем здоровье текущее к максимальному
        anim = GetComponent<Animation>();//собираем всю анимацию клипы
        //opponent = player.GetComponent<Fighter>();//ставим игрока целью
        startPosition = transform.position;//запоминаем стартовую позицию

        database = GameObject.Find("Inventory System Manager").GetComponent<ItemDatabase>();

        isArenaClose = false;
        startFight = false;
        spearCooldown = 0;
        enableCast = true;
        mana = 100;
        resetPhase = false;
        //cameraSounds = GameObject.Find("Main Camera").GetComponents<AudioSource>();
        sounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        StartFight();        
        if (player == null)//если нет игрока
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;//ищем его
            opponent = player.GetComponent<Fighter>();//ставим игрока целью
        }

        if (startFight)
        {
            if (!isArenaClose)
            {
                //cameraSounds[0].Stop();
                //cameraSounds[1].Play();
                GameObject.Find("Main Camera").GetComponent<LevelSounds>().DogBoss();//sounds for dog boss
                Instantiate(stoneEnter);
                GameObject.Find("Start Boss Fight Trigger").GetComponent<Collider>().enabled = false;
                isArenaClose = true;
            }

            if (!IsDead())//если живы
            {
                if(health > 25000)
                    PhaseOne();

                if (health <= 25000 && health > 10000)
                    PhaseTwo();

                if (health <= 10000 && health > 5000)
                    PhaseOne();

                if (health <= 5000)
                {
                    if (!resetPhase)
                    {
                        mana = 100;
                        spearCooldown = 0;
                        resetPhase = true;
                    }
                    PhaseTwo();
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
        else anim.CrossFade(idle.name);

        PlayerDead();
        StopBossFight();
	}

    void StartFight()
    {
        if (startBossFightTrigger.GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
        {
            startFight = true;
            Pause.isBossFight = true;
        }
    }

    void PlayerDead()
    {
        if (player.GetComponent<Fighter>().health <= 0)
        {
            startFight = false;
            Pause.isBossFight = false;
            //cameraSounds[1].Stop();
            //cameraSounds[0].Play();
            GameObject.Find("Main Camera").GetComponent<LevelSounds>().Reset();//sounds for dog boss
        }
    }

    void StopBossFight()
    {
        if (!startFight)
        {

            health = maxHealth;
            Pause.isBossFight = false;

            if (GameObject.Find("Boss Stone Enter(Clone)") != null)
                Destroy(GameObject.Find("Boss Stone Enter(Clone)").gameObject);

            GameObject.Find("Start Boss Fight Trigger").GetComponent<Collider>().enabled = true;
            isArenaClose = false;
            if (Vector3.Distance(transform.position, startPosition) < startPositionRange)
                anim.CrossFade(idle.name);//анимацию спокойстви и стоим на месте
            else //если расстояние до начальной позиции больше заданного
                WalkToStartPosition();//то идем к начальной позиции
        }
    }

    void PhaseOne()
    {
        if (!InRange())//если не в радиусе удара
            Chase();//функиця приследования игрока
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

    void PhaseTwo()
    {
        if (Vector3.Distance(transform.position, startPosition) < startPositionRange)
        {
            if (mana >= 0 && enableCast)
                anim.CrossFade(cast.name);//анимацию каста и стоим на месте
            else anim.CrossFade(stun.name);

            spearCooldown += Time.deltaTime;

            if (spearCooldown > 3.0f && enableCast)
            {
                GameObject SpearSpell = Instantiate(spears);
                SpearSpell.transform.position = new Vector3(player.transform.position.x, 30, player.transform.position.z);
                SpearSpell.GetComponent<DogBossSpears>().target = player;
                spearCooldown = 0;
                mana -= 14;
                if (mana <= 0)
                    enableCast = false;
            }

            if (!enableCast)
            {
                mana += Time.deltaTime * 8;
                if (mana >= 100)
                {
                    mana = 100;
                    enableCast = true;
                    spearCooldown = 0;
                }
            }
            
        }
        else //если расстояние до начальной позиции больше заданного
            WalkToStartPosition();//то идем к начальной позиции
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
        if (Vector3.Distance(transform.position, player.transform.position) < rangeRadius)
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
            Pause.isBossFight = false;
            player.GetComponent<LevelingSystem>().GetExp(givenExp);//даем игроку опыт
            DropLoot();
            LevelingSystem.dogBossKill = true;
            Destroy(GameObject.Find("Boss Stone Exit").gameObject);
            Destroy(GameObject.Find("Boss Stone Enter(Clone)").gameObject);
            //cameraSounds[1].Stop();
            //cameraSounds[0].Play();
            GameObject.Find("Main Camera").GetComponent<LevelSounds>().Reset();//sounds for dog boss
            Destroy(gameObject);//уничтожаем моба
        }
    }

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
        int roll = Random.Range(0, 100);//бросок игрока

        for (int i = 0; i < itemsToDropId.Length; i++)//по всем вещам, которые может уронить моб
        {
            items[i] = database.FetchItemById(itemsToDropId[i]);//записываем эти вещи по айди из database
        }

        for (int i = 0; i < itemsToDropId.Length; i++)//по всем вещам, которые может выкинуть моб
        {
            if (items[i].dropRate >= roll)//если шанс выпадения вещи больше, чем выкинул игрок, то...
            {
                GameObject drop = Instantiate(droppedItem);//выкидывае вещь в мир
                //позициб вещи ставим вверх +5 единиц, чтобы не разлетались
                drop.transform.position = new Vector3(transform.position.x, transform.position.y + 7 + i, transform.position.z);
                drop.GetComponentInChildren<PressTheTextItemTitle>().item = items[i];//присваиваем какую вещь выкинули объекту, который отвечает за визуализацию вещи в мире
            }
        }
    }
}
