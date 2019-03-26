// sound[0]  -   death sound        //
// sound[1]  -   hint sound         //
// sound[2]  -   heal sound         //



using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fighter : MonoBehaviour {

    public float maxHealth;//максимальное здоровье
    public float health;//текущее здоровье
    public float damage;//текущий урон, который складывается из урона оружия + урон от скалирования уровня

    public GameObject opponent;//противник
    public AnimationClip attack;//анимация атаки
    public AnimationClip die;//анимация смерти
    public float range;//растояние до противника, в котором можно бить
    public float impactTime;//время, когда будет считаться, что игрок ударил, зависит от времени анимации
                            //в какой момент времени анимации подходит для вычета урона - процент проигрывания анимации
    public Image hitImage;
    private bool getHit;
    private float seconds;

    public  float levelDamage;//урон, увеличивается с увеличением уровня
    private float weaponDamage;//урон от оружия
    private int levelHealth;
    private int helmVitality;
    private bool impacted;//ударяем?
    private Animation anim;//анимауия для сбора компонент анимации
    private float impactLength;//длина удара, зависит от времени удара impactTime
    private float weaponPowerCoef;//сила удара оружия

    public Well resWell;

    public AudioSource[] sounds;

    bool started;
    bool ended;

    public float combetEscapeTime;
    public float countDown;

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this);
        health = maxHealth;//прираваем текущему здоровью максимальный показатель
        anim = GetComponent<Animation>();//собираем всю анимацию
        impactLength = (anim[attack.name].length * impactTime);//длительность удара это процент проигрывания анимации
        sounds = GetComponents<AudioSource>();
        opponent = null;
        levelHealth = 100;
	}
	
	// Update is called once per frame
	void Update () 
    {
        maxHealth = levelHealth + helmVitality;
        damage = levelDamage + weaponDamage;
        DeniedOverHeal();//функция запрещающая выползать здоровью за рамки максимального

        //если есть цель, но мы хотим её сбросить по нажатию ПКМ
        if (Input.GetMouseButtonDown(1) && opponent != null)//нажата правая кнопка мыши и есть цель
        {
            opponent = null;//сбрасываем цель
        }

        if (Input.GetKey(KeyCode.Space) //если нажата кнопка удара - ПРОБЕЛ
            && InRange() //если находимся на расстоянии удара
            && !IsDead() //и не мертвы
            && GameObject.Find("Place for Weapon").transform.childCount == 1)// и в руке есть оружие
        {                                                         
            anim.Play(attack.name);//играем анимацию удара
            ClickToMove.attack = true;//передаем в скрипт движения, что мы бьем
            if (opponent != null)//если есть выбранный враг
            {
                transform.LookAt(opponent.transform.position);//смотрим на него
            }
        }
        if (anim[attack.name].time > 0.9 * anim[attack.name].length)//если текущее время проигрывания анимации 
        {                                                           //больше 90 процентов, атака закончилась
            ClickToMove.attack = false;//передаем в скрипт движения, что не атакуем
            impacted = false;//удар закончился
        }
        Impact();//функция отвечающая за удар
        Die();//функция, отвечающая за смерть
        DrawGetHitImage();
	}

    void Impact()//УДАР
    {
        //  если есть цель и идет анимация атаки и удар закончился(или его не было)
        if (opponent != null && GetComponent<Animation>().IsPlaying(attack.name) && !impacted)
        {       //если время проигрывания анимации превышает процент, когда прошел удар и общее время анимации меньше 90 процентов, т.е. анимация играется, еще не закончилась, а визуально уже ударили
            if (anim[attack.name].time > impactLength && (anim[attack.name].time < 0.9 * anim[attack.name].length ))
            {
                sounds[1].Play();//играем музыку удара
                countDown = combetEscapeTime + 2;
                CancelInvoke("CombatEscapeCountDown");
                InvokeRepeating("CombatEscapeCountDown", 0, 1);
                if (opponent.tag != "Boss Dark Wood" && opponent.tag != "Library Boss" && opponent.tag != "CoyoteBoss")
                    opponent.GetComponent<Mob>().GetHit(damage);//бьем цель с уроном damage

                if (opponent.tag == "Boss Dark Wood")
                    opponent.GetComponent<DogBoss>().GetHit(damage);

                if (opponent.tag == "Library Boss")
                    opponent.GetComponent<KBoss>().GetHit(damage);

                if (opponent.tag == "CoyoteBoss")
                    opponent.GetComponent<CoyoteBoss>().GetHit(damage);

                impacted = true;//удар есть
            }
        }
    }

    bool InRange()//находится ли игрок на расстоянии удара
    {
        if (opponent != null)//если цель не пустая
        {       //если расстояние между целью и игроком меньше расстояния удара
            if (Vector3.Distance(opponent.transform.position, transform.position) < range)
            {
                return true;//то мы в радиусе удара
            }
            else return false;//уиначе нет
        }
        else return false;//нет врага - нет расстояния
    }

    public void GetHit(float damage)//получаем урон damagе
    {
        getHit = true;
        health = health - damage;//вычитаем из текущего здоровья урон удара
        if(health < 0)//если здоровье упало за 0
        {
            health = 0;//равняем 0
        }
    }

    void DrawGetHitImage()
    {
        if(getHit == true)
        {
            hitImage.enabled = true;
            getHit = false;
            seconds = 0;
            hitImage.color = new Color(hitImage.color.r, hitImage.color.g, hitImage.color.b, 1);
        } 
        else seconds += Time.deltaTime;
        if (seconds >= 3.0f)
        {
            hitImage.enabled = false;
            seconds = 0;
            hitImage.color = new Color(hitImage.color.r, hitImage.color.g, hitImage.color.b, 1);
        }
        else
        {
            hitImage.color = new Color(hitImage.color.r, hitImage.color.g, hitImage.color.b, hitImage.color.a - seconds / 100 );
        }
    }

    public bool IsDead()//мемртвы ли?
    {
        if (health == 0)//если текущее здоровье равно 0
        {
            return true;//возвращаем правды, мертвы
        }
        else return false;//иначе живы-здоровы
    }

    public void Die()//СМЕРТЬ
    {
        if(IsDead() && !ended)//если умерли
        {
            if (!started)//если еще не начали умирать
            {
                ClickToMove.die = true;//посылаем в скрипт движения, что умерли
                anim.CrossFade(die.name);//играем анимацию смерти
                started = true;//начали умирать
            }

            if (started && anim.IsPlaying(die.name))//если начали умирать и проигрывается анимация смерти
            {
                //делаем, что захотим
                Debug.Log("You DEAD!");
                sounds[0].Play();
                ended = true;//закончили
            }
        }
        if (started && !anim.IsPlaying(die.name))
        {
            GameObject.Find("Press any key to revive text").GetComponent<Text>().enabled = true;
            if (Input.anyKey)
                Revive();
        }
    }

    void Revive()
    {
        started = false;
        ended = false;
        health = maxHealth;
        ClickToMove.die = false;
        transform.position = resWell.resPosition.position;
        GameObject.Find("Press any key to revive text").GetComponent<Text>().enabled = false;
    }

    void CombatEscapeCountDown()
    {
        countDown--;
        if(countDown == 0)
        {
            CancelInvoke("CombatEscapeCountDown");
        }
    }

    public void LevelDamage(float levelCoef)//увеличиваем урон с прибавлением уровня персонажа на коэффициент levelCoef
    {
        levelDamage += levelCoef;//увеличиваем текущий урон
    }

    public void LevelHealth(int levelCoef)//увеличиваем здоровье с прибавлением уровня на коэффициент levelCoef
    {
        //maxHealth += levelCoef;//увеличиваем здоровье
        levelHealth += levelCoef;
    }

    public void AddWeaponPower(float weaponPower)//функция добавления силы оружия
    {
        weaponDamage = weaponPower;//сила урона = силе оружия
    }

    public void AddHeadPower(int vitality)
    {
        helmVitality = vitality;
    }

    public void Healing(int heal)//функция лечения
    {
        health += heal;//лечимся
        sounds[2].Play();//играем звук лечения
    }

    void DeniedOverHeal()//функция запрещающая выползать здоровью за рамки максимального
    {
        if (health > maxHealth)//если текущее здоровье больше макс
            health = maxHealth;//текщее равно максимальному
    }
 
}
