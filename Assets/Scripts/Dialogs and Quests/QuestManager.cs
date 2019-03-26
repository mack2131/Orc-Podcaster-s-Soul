using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;


public class QuestManager : MonoBehaviour {

    [SerializeField]
    public int currentQuest;
    public int[,] allQuests;

    public Text questName;
    public Text questGoal;

    private QuestDatabase database;
    public AudioSource sounds;

    /*for quests*/
    public GameObject GlassesQuestMob;
    public GameObject KnucklesLake;
    public GameObject questWoodenLog;
    public GameObject honeyJar;
    public GameObject cottail;
    public GameObject blackM;
    public GameObject treeJuice;
    public GameObject mountainInv;
    public GameObject portalToLib;
    public GameObject donutPath;
    public bool isHaveWoodenLog;

    public bool questWoodenLogSpawn;
    public bool glassGooseSpawn;
    public bool KnucklesLakeSpwn;
    public bool honeyJarSpawn;
    public bool cottailSpawn;
    public bool blackMSpawn;
    public bool treeJuiceSpawn;
    public bool mountainInvSpawn;
    public bool portalToLibSpawn;
    public bool donutPathSpawn;

	// Use this for initialization
	void Start () 
    {
        database = GetComponent<QuestDatabase>();
        allQuests = new int[database.database.Count, 2];

        questWoodenLogSpawn = false;
        glassGooseSpawn = false;
        KnucklesLakeSpwn = false;
        honeyJarSpawn = false;
        cottailSpawn = false;
        blackMSpawn = false;
        treeJuiceSpawn = false;
        mountainInvSpawn = false;
        portalToLibSpawn = false;
        donutPathSpawn = false;

        if (SaveLoad.wasLoaded)
            LoadQuest();

        sounds = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        CurrentQuest(currentQuest);
	}

    public void AcceptQuest(int id)/*функция выводит информацию о текущем квесте в окно "ЗАДАНИЕ"*/
    {
        questName.text = database.FetchQuestById(id).name;
        questGoal.text = database.FetchQuestById(id).description;
        currentQuest = database.FetchQuestById(id).questId;
    }

    public void CleanQuest()/*очищаем текущий квест, задания у игрока нет*/
    {
        sounds.Play();
        questGoal.text = "";
        questName.text = "";
        currentQuest = -1;
    }

    /* Функция работает по айди квеста, выполняя различные условия и меняя состояние сцен в зависимости от прогресса */
    /* игрок. У квестов три состояния: 0 - квест еще не брался, 1 - квест в процессе выполнения, 2 - квест выполнен. */ 
    /* В зависимости от состояния квеста и проверки его условий меняется и мир. Массив состояния квеста сохраняется  */
    /* и передается в скрипт PlayeData.cs в массив QUESTS. В каждой сцене в скрипте, которые повешeн на менеджер     */ 
    /* сцены, выполняется проверка всех условий и состояний квеста и меняется состояние мира в зависимости от        */
    /* игрока.                                                                                                       */
    void CurrentQuest(int id)
    {
        switch (id)
        {
            case -1:
                {
                    break;
                }
            case 0:/*ПЕРВЫЙ КВЕСТ Учимся двигаться*/
                {
                    if (allQuests[id, 1] != 2)
                    {
                        allQuests[id, 1] = 1;
                        GameObject.Find("Spirit").transform.position = Vector3.MoveTowards(GameObject.Find("Spirit").transform.position, GameObject.Find("Spirit Position 2").transform.position, Time.deltaTime * 200);
                        if (GameObject.Find("Rocks 1") != null)
                            Destroy(GameObject.Find("Rocks 1").gameObject);
                        if (GameObject.Find("Spirit Position 2").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                        {
                            CleanQuest();
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            GameObject.Find("Spirit Position 2").GetComponent<Collider>().enabled = false;
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 1:/*ВТОРОЙ КВЕСТ Учимся открывать сундуки*/
                {
                    if (allQuests[id, 1] != 2)
                    {
                        allQuests[id, 1] = 1;
                        if (GameObject.Find("Rocks 2") != null)
                            Destroy(GameObject.Find("Rocks 2").gameObject);
                        if (Inventory.slots[100].transform.childCount != 0 && Inventory.slots[101].transform.childCount != 0)
                        {
                            CleanQuest();
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 2:/*Третий квест Убить гусей*/
                {
                    if (allQuests[id, 1] != 2)
                    {
                        allQuests[id, 1] = 1;
                        if (GameObject.Find("Rocks 3") != null)
                            Destroy(GameObject.Find("Rocks 3").gameObject);
                        GameObject.Find("Spirit").transform.position = Vector3.MoveTowards(GameObject.Find("Spirit").transform.position, GameObject.Find("Spirit Position 3").transform.position, Time.deltaTime * 200);
                        if (GameObject.Find("Goose Crystall Quest").transform.childCount == 0)
                        {
                            CleanQuest();
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 3:/*Четвертый квест Прочитать манускрипт*/
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Rocks 4") != null)
                        Destroy(GameObject.Find("Rocks 4").gameObject);
                    GameObject.Find("Spirit").transform.position = Vector3.MoveTowards(GameObject.Find("Spirit").transform.position, GameObject.Find("Spirit Position 4").transform.position, Time.deltaTime * 200);
                    if (Inventory.readedBooks[0] == 1)
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 4://Пятый квест Кристалльный путь
                {
                    allQuests[id, 1] = 1;
                    GameObject.Find("Spirit").transform.position = Vector3.MoveTowards(GameObject.Find("Spirit").transform.position, GameObject.Find("Spirit Position 5").transform.position, Time.deltaTime * 200);
                    if (GameObject.Find("Rocks 5") != null)
                        Destroy(GameObject.Find("Rocks 5").gameObject);
                    if (GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().resWell.id == 1)
                    {
                        if (GameObject.Find("Rocks 6") != null)
                            Destroy(GameObject.Find("Rocks 6").gameObject);
                    }
                    if (GameObject.Find("Spirit Position 5").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("Spirit Position 5").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 5://Шестой квест Знакомый в деревне
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Rocks 7") != null && SceneManager.GetActiveScene().name == "Tutorial")
                        Destroy(GameObject.Find("Rocks 7").gameObject);
                    if (GameObject.Find("Welovegames Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 6://Седьмой квест Поговорить с Белым Койотом
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 7://восьмой квест Принести дрова Койoту
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Quest Wooden Log(Clone)") == null && !questWoodenLogSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(10))
                    {
                        Instantiate(questWoodenLog);
                        questWoodenLogSpawn = true;
                    }
                    if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(10))
                    {
                        GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = true;
                        if (GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                        {
                            Magic.enableFireball = true;//сохранение магии будет происходить при загрузке игры в функции LoadQuest() 
                            CleanQuest();
                            Destroy(GameObject.Find("wooden log").gameObject);
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = false;
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 8://Квесть девятый Тряпка для очков
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Glasses Quest Mob(Clone)") == null && !glassGooseSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(11))
                    {
                        Instantiate(GlassesQuestMob);
                        glassGooseSpawn = true;
                    }
                    if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(11))
                        GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = true;
                    if (GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        Destroy(GameObject.Find("glass fabric").gameObject);
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 9:/*квест 10 - убить угандских войнов на молочном озере*/
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Knuckles Quest Lake(Clone)") == null && !KnucklesLakeSpwn)
                    {
                        Instantiate(KnucklesLake);
                        KnucklesLakeSpwn = true;
                    }
                    if (GameObject.Find("Knuckles Quest Lake(Clone)").transform.childCount == 0)
                    {
                        Destroy(GameObject.Find("Knuckles Quest Lake(Clone)").gameObject);
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 10: /* квест 11 - Путь в темный лес */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Bjorn Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("Bjorn Position").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 11: /* квест 12 - Космический путешественник */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Quest Pine") != null)
                        Destroy(GameObject.Find("Quest Pine").gameObject);
                    if (GameObject.Find("Ship Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("Ship Position").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 12:/* квест 13 - Найти мед */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Honey Jar(Clone)") == null && !honeyJarSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(25))
                    {
                        Instantiate(honeyJar);
                        honeyJarSpawn = true;
                    }
                    if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(25))
                    {
                        GameObject.Find("Bjorn Position").GetComponent<Collider>().enabled = true;
                        if (GameObject.Find("Bjorn Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                        {
                            CleanQuest();
                            Destroy(GameObject.Find("honey").gameObject);
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            GameObject.Find("Bjorn Position").GetComponent<Collider>().enabled = false;
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 13:/* квест 14 - Соусная сила */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Quest Pine 2") != null)
                        Destroy(GameObject.Find("Quest Pine 2").gameObject);

                    if (GameObject.Find("Boss examine position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("Boss examine position").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 14:/*квест 15 - В поисках Богини */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(7))
                    {
                        CleanQuest();
                        Destroy(GameObject.Find("sze chuan sauce").gameObject);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 15:/*квест 16 - Огромный монстр */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Boss Stone Enter") != null)
                        Destroy(GameObject.Find("Boss Stone Enter").gameObject);

                    if (GameObject.Find("Zhu-Zha Boss") == null)
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(50);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 16:/*квест 17 - Вернуться к Денису */
                {
                    allQuests[id, 1] = 1;
                    if(GameObject.Find("Welovegames Position") != null)
                        GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = true;
                    if (GameObject.Find("Welovegames Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position) &&
                        GameObject.Find("Welovegames Position") != null)
                    {
                        GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = false;
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 17:/*квест 18 - Найти Баб Ягу */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Baba Yaga Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = false;
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 18:/*квест 19 - Плюшевая шишка */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Quest Cottail(Clone)") == null && !cottailSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(28))
                    {
                        Instantiate(cottail);
                        cottailSpawn = true;
                    }
                    if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(28))
                    {
                        GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = true;
                        if (GameObject.Find("Baba Yaga Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                        {
                            CleanQuest();
                            Destroy(GameObject.Find("cottail").gameObject);
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = false;
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 19:/*квест 20 - Черный гриб */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Quest Mushroom(Clone)") == null && !blackMSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(29))
                    {
                        Instantiate(blackM);
                        blackMSpawn = true;
                    }
                    if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(29))
                    {
                        GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = true;
                        if (GameObject.Find("Baba Yaga Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                        {
                            CleanQuest();
                            Destroy(GameObject.Find("black mushroom").gameObject);
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = false;
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 20:/*квест 21 - Сок из дерева */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Tree Juice(Clone)") == null && !treeJuiceSpawn && !GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(30))
                    {
                        Instantiate(treeJuice);
                        treeJuiceSpawn = true;
                    }
                    if (GameObject.Find("Inventory System Manager").GetComponent<Inventory>().IsItemInInventory(30))
                    {
                        GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = true;
                        if (GameObject.Find("Baba Yaga Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                        {
                            CleanQuest();
                            Destroy(GameObject.Find("tree juice").gameObject);
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            GameObject.Find("Baba Yaga Position").GetComponent<Collider>().enabled = false;
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 21:/*квест 22 - Карлики */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Mountain Invansion(Clone)") == null && !mountainInvSpawn)
                    {
                        Instantiate(mountainInv);
                        mountainInvSpawn = true;
                    }
                    if (GameObject.Find("Mountain Invansion(Clone)").transform.childCount == 0)
                    {
                        Destroy(GameObject.Find("Mountain Invansion(Clone)").gameObject);
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 22:/*квест 23 - Второй ключ */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Mountain Quest Rocks") != null)
                        Destroy(GameObject.Find("Mountain Quest Rocks").gameObject);

                    if (GameObject.Find("Mountain Examine Trigger").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        GameObject.Find("Mountain Examine Trigger").GetComponent<Collider>().enabled = false;
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        Magic.enableIceWave = true;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 23:/*квест 24 - Узнать про библиотеку */
                {
                    allQuests[id, 1] = 1;
                    GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = true;

                    if (GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("WhiteCoyote Position").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 24:/*квест 25 - Путь в библиотеку */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Portal to Library(Clone)") == null && !portalToLibSpawn)
                    {
                        Instantiate(portalToLib);
                        portalToLibSpawn = true;
                    }
                    if (GameObject.Find("Library Spirit Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position))
                    {
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        GameObject.Find("Library Spirit Position").GetComponent<Collider>().enabled = false;
                        FillQuestArray(id);
                    }
                    break;
                }
            case 25:/*квест 26 - Вторжение */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Library rocks") != null)
                        Destroy(GameObject.Find("Library rocks").gameObject);

                    if (GameObject.Find("Fat King") == null)
                    {
                        if (GameObject.Find("Library Exit Rock") != null)
                            Destroy(GameObject.Find("Library Exit Rock").gameObject);
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 26:/*квест 27 - WEHATETHISGAME */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("WhiteCoyote") != null)
                        Destroy(GameObject.Find("WhiteCoyote").gameObject);

                    if (GameObject.Find("Welovegames Position") != null)
                        GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = true;

                    if (GameObject.Find("Welovegames Position").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position) &&
                        GameObject.Find("Welovegames Position") != null)
                    {
                        GameObject.Find("Welovegames Position").GetComponent<Collider>().enabled = false;
                        CleanQuest();
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 27:/*квест 28 - Игра за трон */
                {
                    allQuests[id, 1] = 1;
                    if (GameObject.Find("Quest Doors") != null)
                        Destroy(GameObject.Find("Quest Doors").gameObject);

                    if (GameObject.Find("Coyote Arena Position (1)").GetComponent<Collider>().bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position) &&
                        GameObject.Find("Coyote Arena Position (1)") != null)
                    {
                        //GameObject.Find("Coyote Arena Position").GetComponent<Collider>().enabled = false;
                        CleanQuest();
                        GameObject.Find("Coyote Arena Position (1)").GetComponent<Collider>().enabled = false;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                        currentQuest = 28;
                        AcceptQuest(28);
                        FillQuestArray(id);
                    }
                    break;
                }
            case 28:/*квест 29 - Белый Койот */
                {
                    allQuests[id, 1] = 1;
                    if (SceneManager.GetActiveScene().name == "Castle Arena")
                    {
                        if (GameObject.Find("WhiteCoyote Boss") == null)
                        {
                            CleanQuest();
                            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelingSystem>().GetExp(350);
                            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(51);
                            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(52);
                            currentQuest = 29;
                            AcceptQuest(29);
                            FillQuestArray(id);
                        }
                    }
                    break;
                }
            case 29:/*квест 29 - Побег */
                {
                    allQuests[id, 1] = 1;
                    if (SceneManager.GetActiveScene().name == "Castle Arena")
                    {
                        if (GameObject.Find("Path to Portal(Clone)") == null && !donutPathSpawn)
                        {
                            Instantiate(donutPath);
                            donutPathSpawn = true;
                        }
                    }
                    break;
                }
            default:
                {
                    allQuests[id, 1] = 1;
                    break;
                }
        }
    }

    void FillQuestArray(int currentId)/*если квест завершился, то его и все предыдущие ставим 2, т.е. завершили*/
    {
        for (int i = 0; i <= currentId; i++)
            allQuests[i, 1] = 2;
    }

    public void SentQuestState()/*передаем состояние квестов в массив состояния для сохранения игры*/
    {
        for (int i = 0; i < allQuests.Length / 2; i++)
            PlayerData.data.QUESTS[i] = allQuests[i, 1];
    }

    void LoadQuest()/*фнкция загрузки квеста*/
    {
        for (int i = 0; i < allQuests.Length / 2; i++)/*идем по все квестам, если в каком-то квесте стоит 1, значит квест в процессе выполнения и мы берем его за текущий, дальше менеджер сцен загрузит необходимое состояние*/
        {
            allQuests[i, 1] = SaveLoad.savedGame.QUESTS[i];
            if (SaveLoad.savedGame.QUESTS[i] == 1)
            {
                AcceptQuest(i);
                break;
            }
        }

        if (allQuests[7, 1] == 2)
            Magic.enableFireball = true;
        if (allQuests[22, 1] == 2)
           Magic.enableIceWave = true;
        SaveLoad.playerQuestLoaded = false;
    }
}
