using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Magic : MonoBehaviour {

    public AnimationClip cast;//клип анимации каста заклинания
    public Image castProgressBar;
    public Image castProgressBarFrame;
    public GameObject target;

    [Header("Fireball magic")]
    public Image fireballIcon;//значок заклинания
    public Image fireballCooldown;//изображение, отвечабщее за перезарядку
    public GameObject fireballPrefab;//префаб самого фаэрбола в мире
    public float fireballCooldownTime;//время перезарядки заклинания
    public float fireballCastRadius;//радиус, на котором можно кастовать заклинание
    public float fireballDamage;//урон от заклинания
    public static bool enableFireball;//РАЗРЕШЕН ЛИ ФАЕРБОЛ, В ЗАВИСИМОСТИ ОТ ПРОГРЕССА ИГРОКА
    private float fireballCounter;//счетчик для перезарядки
    private bool fireballCasting;//кастуем ли заклинание
    private bool fireballRestored;//откатилось ли заклинание
    private bool fireballRestoring;//заклинание откатывается

    [Header("Icewave magic")]
    public Image iceIcon;
    public Image iceCooldown;
    public GameObject iceWavePrefab;
    public float iceCooldownTime;
    public float iceCastRadius;
    public float iceDamage;
    private float iceCounter;
    private bool iceCasting;
    private bool iceRestored;
    private bool iceRestoring;
    public static bool enableIceWave;//РАЗРЕШЕН ЛИ ЛЕДЯНАЯ ВОЛНА, В ЗАВИСИМОСТИ ОТ ПРОГРЕССА ИГРОКА.

    [Header("Sprint magic")]
    public Image sprintIcon;
    public Image sprintCooldown;
    public float sprintCooldownTime;
    private float sprintCounter;
    private bool sprintCasting;
    private bool sprintRestored;
    private bool sprintRestoring;

    private Animation anim;//анимация для сбора всех анимаций
    public static bool casting;

	// Use this for initialization
	void Start () 
    {
        casting = false;

        //enableFireball = true;
        //enableIceWave = true;

        anim = GetComponent<Animation>();//собираем всю анимацию

        fireballRestored = true;//перезарядки нет
        iceRestored = true;//перезарядки нет      
        sprintRestored = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (casting)
            ShowCastProgress();
        else HideCastProgress();

        if (!GetComponent<Fighter>().IsDead())
        {
            /*=====================ОГНЕННЫЙ ШАР=====================================*/
            if (enableFireball)//если открыт фаербол
                fireballIcon.enabled = true;//рисуем значок фаербола
            else fireballIcon.enabled = false;//иначе не рисуем фаербол

            if (fireballRestored == true)//если пересаражено заклинание
                fireballCooldown.fillAmount = 0;//убираем картинку перезарядки
            else FireballRestoration();//функция, отвечающая за перезарядку заклинания

            if (!InFireballCastRadius() && fireballRestored)//если мы не в зоне действия заклинания
                fireballCooldown.fillAmount = 1;//то визуально говорим, что нельзя кастовать

            if (Input.GetKeyDown(KeyCode.Alpha1) && //если нажали 1
                !casting &&                          //не кастуем в данный момент
                fireballRestored &&                 //и заклинание перезарядилось
                enableFireball &&                   //и открыт фаербол в связи с прогрессом
                InFireballCastRadius() &&           //и игрок в радиусе каста
                !GetComponent<Fighter>().IsDead())  //и игрок жив
            {
                target = GetComponent<Fighter>().opponent.gameObject;
                fireballCasting = true;//говорим, что кастуем
                fireballRestored = false;//заклинание не перезагружено
                fireballRestoring = true;//заклинание восстанавливается
            }

            if (fireballCasting && !GetComponent<Fighter>().IsDead())//если кастуем
            {
                ClickToMove.attack = true;//говорим в скрипт движения, что кастуем(атакуем)
                transform.LookAt(target.transform.position);//смотрим на цель
                anim.Play(cast.name);//проигрываем анимацию заклинания
                casting = true;//кастуем заклинание
                //если анимация проигралась на 80%, т.е. визуально, произошло сотворение заклинания
                if (anim[cast.name].time > 0.80 * anim[cast.name].length)
                {
                    ClickToMove.attack = false;//говорим, что можем ходить
                    fireballCasting = false;//больше не кастуем
                    Fireball();//фнукция спавна фаербола
                }
            }

            /*========================================================================================*/

            /*=======ЛЕДЯНАЯ ВОЛНА====================================================================*/

            if (enableIceWave)//если открыт фаербол
                iceIcon.enabled = true;//рисуем значок фаербола
            else iceIcon.enabled = false;//иначе не рисуем фаербол

            if (iceRestored == true)//если пересаражено заклинание
                iceCooldown.fillAmount = 0;//убираем картинку перезарядки
            else IcewaveRestore();//функция, отвечающая за перезарядку заклинания

            if (!InIceRadius() && iceRestored)//если мы не в зоне действия заклинания
                iceCooldown.fillAmount = 1;//то визуально говорим, что нельзя кастовать

            if (Input.GetKeyDown(KeyCode.Alpha2) && //если нажали 1
                !casting &&                         //не кастуем в данный момент
                iceRestored &&                     //и заклинание перезарядилось
                enableIceWave &&                   //и открыт фаербол в связи с прогрессом
                InIceRadius() &&           //и игрок в радиусе каста
                !GetComponent<Fighter>().IsDead())  //и игрок жив
            {
                target = GetComponent<Fighter>().opponent.gameObject;
                iceCasting = true;//говорим, что кастуем
                iceRestored = false;//заклинание не перезагружено
                iceRestoring = true;//заклинание восстанавливается
            }

            if (iceCasting && !GetComponent<Fighter>().IsDead())//если кастуем
            {
                ClickToMove.attack = true;//говорим в скрипт движения, что кастуем(атакуем)
                transform.LookAt(target.transform.position);//смотрим на цель
                casting = true;//кастуем заклинание
                anim.Play(cast.name);//проигрываем анимацию заклинания
                //если анимация проигралась на 80%, т.е. визуально, произошло сотворение заклинания
                if (anim[cast.name].time > 0.80 * anim[cast.name].length)
                {
                    ClickToMove.attack = false;//говорим, что можем ходить
                    iceCasting = false;//больше не кастуем
                    Icewave();//фнукция спавна фаербола
                }
            }

            /*=======СПРИНТ====================================================================*/

            if (sprintRestored == true)//если пересаражено заклинание
                sprintCooldown.fillAmount = 0;//убираем картинку перезарядки
            else SprintRestore();//функция, отвечающая за перезарядку заклинания

            if (Input.GetKeyDown(KeyCode.Alpha3) && //если нажали 3
                    !casting &&                         //не кастуем в данный момент
                    sprintRestored &&                     //и заклинание перезарядилось
                    !GetComponent<Fighter>().IsDead())  //и игрок жив
            {
                sprintCasting = true;//говорим, что кастуем
                sprintRestored = false;//заклинание не перезагружено
                sprintRestoring = true;//заклинание восстанавливается
            }

            if (sprintCasting && !GetComponent<Fighter>().IsDead())//если кастуем
            {
             
                Sprint();//фнукция спавна фаербол
                sprintCasting = false;
            }
            /*========================================================================================*/
        }
        else
        {
            ClickToMove.attack = false;
            casting = false;
            fireballCasting = false;
            iceCasting = false;
            HideCastProgress();
        }
        /*========================================================================================*/

    }


    bool InFireballCastRadius()//находится ли игрок в радиусе каста заклинания
    {
        if (GetComponent<Fighter>().opponent != null)//если есть цель, которую берем из Fighter
        {
            //есди позиция между игроком и целью меньше радиуса каста
            if (Vector3.Distance(transform.position, GetComponent<Fighter>().opponent.transform.position) < fireballCastRadius)
                return true;//то верно, мы в зоне каста
            else return false;//иначе, не в зоне каста
        }
        else return false;//если нет цели, то и подавно в не зоны действия
    }

    void Fireball()//функция спавна фаербола
    {
        GameObject fireball = Instantiate(fireballPrefab);//создаем объект
        fireball.GetComponent<Fireball>().target = target;//даем компоненту Fireball перемнной цель цель игрока
        fireball.GetComponent<Fireball>().damage = fireballDamage;//передаем урон
        //ставим начальную позицию объекта, чтобы фаербол заспавнился в центре игрока, а не из под ног
        fireball.transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z + 3);
    }

    void FireballRestoration()//функция отката заклинания
    {
        if (fireballRestoring == true)//если фаерболл перезаряжается
        {
            fireballCooldown.fillAmount = 1;//заполняем картинку кулдаун
            fireballCounter = 0;//обнуляем счетчик
            fireballRestoring = false;//говорим, что не восстанавливаемся
        }
        fireballCounter += Time.deltaTime;//считаем счетчик
        //если счетчик меньше времени восстановления и заклинание не откатилось
        if (fireballCounter <= fireballCooldownTime && fireballRestored == false)
            fireballCooldown.fillAmount -= 1 / fireballCooldownTime * Time.deltaTime;//уменьшаем картинку отката
        else fireballRestored = true;//если счетчик уже превысил, то говорим, что заклинание восстановлено

    }

    bool InIceRadius()
    {
        if (GetComponent<Fighter>().opponent != null)//если есть цель, которую берем из Fighter
        {
            //есди позиция между игроком и целью меньше радиуса каста
            if (Vector3.Distance(transform.position, GetComponent<Fighter>().opponent.transform.position) < iceCastRadius)
                return true;//то верно, мы в зоне каста
            else return false;//иначе, не в зоне каста
        }
        else return false;//если нет цели, то и подавно в не зоны действия
    }
    void Icewave()
    {
        GameObject ice = Instantiate(iceWavePrefab);//создаем объект
        ice.GetComponent<Icewave>().target = target;//даем компоненту Icewave перемнной цель цель игрока
        ice.GetComponent<Icewave>().damage = fireballDamage;//передаем урон
    }

    void IcewaveRestore()
    {
        if (iceRestoring == true)//если ледяная волна перезаряжается
        {
            iceCooldown.fillAmount = 1;//заполняем картинку кулдаун
            iceCounter = 0;//обнуляем счетчик
            iceRestoring = false;//говорим, что не восстанавливаемся
        }
        iceCounter += Time.deltaTime;//считаем счетчик
        //если счетчик меньше времени восстановления и заклинание не откатилось
        if (iceCounter <= iceCooldownTime && iceRestored == false)
            iceCooldown.fillAmount -= 1 / iceCooldownTime * Time.deltaTime;//уменьшаем картинку отката
        else iceRestored = true;//если счетчик уже превысил, то говорим, что заклинание восстановлено
    }

    void SprintRestore()
    {
        if (sprintRestoring == true)//если ледяная волна перезаряжается
        {
            sprintCooldown.fillAmount = 1;//заполняем картинку кулдаун
            sprintCounter = 0;//обнуляем счетчик
            sprintRestoring = false;//говорим, что не восстанавливаемся
        }
        sprintCounter += Time.deltaTime;//считаем счетчик
        //если счетчик меньше времени восстановления и заклинание не откатилось

        if (sprintCounter > 7)
            GetComponent<ClickToMove>().speed = 10;

        if (sprintCounter <= sprintCooldownTime && sprintRestored == false)
            sprintCooldown.fillAmount -= 1 / sprintCooldownTime * Time.deltaTime;//уменьшаем картинку отката
        else sprintRestored = true;//если счетчик уже превысил, то говорим, что заклинание восстановлено
    }

    void Sprint()
    {
        GetComponent<ClickToMove>().speed = 15;
    }

    void ShowCastProgress()
    {
        castProgressBar.enabled = true;
        castProgressBarFrame.enabled = true;
        castProgressBar.fillAmount += 1f / 80;
    }

    void HideCastProgress()
    {
        castProgressBar.enabled = false;
        castProgressBarFrame.enabled = false;
        castProgressBar.fillAmount = 0;
    }
}