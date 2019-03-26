using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class LevelingSystem : MonoBehaviour {
    [Header("Levels")]
    public GameObject player;//объект игрок
    public int level;//текущий уровень
    public Text levelText;//куда пишем текущий уровень

    [Header("Stats")]
    public Text powerText;//куда пишем силу удара
    public Text vitalityText;//куда пишем жизни
    public Text expText;//куда пишем опыт

    private int nextLevelExp = 1000;//сколько опыта на следующий уровень
    public int currentExp;//текущее количество опыта
    private int maxLevel = 100;//максимальный уровень

    public AudioSource[] sounds;//звуки инвенторя

    public static bool dogBossKill;
    public static bool kBossKill;
    public static bool coyoteBossKill;

	// Use this for initialization
	void Start () 
    {
        level = 0;//при начале уровень обращаем в 0
        currentExp = 0;//и количество опыта тоже в 0
        sounds = GetComponents<AudioSource>();//инициализируем звуки
	}
	
	// Update is called once per frame
	void Update () 
    {

        ShowStats();//выводим харатеристики персонажа
        if(level != maxLevel)//если текущий уровень не равен максимальному
            LevelUp();//поднимаем уровень
	}

    void ShowStats()//выод характеристик персонажа
    {
        levelText.text = level.ToString();//печатаем текущий уровень в поле вывода
        expText.text = currentExp.ToString() + " / " + nextLevelExp.ToString();//печатаем текущий опыт в поле вывода
        //печатаем текущий жизни в поле вывода
        vitalityText.text = player.GetComponent<Fighter>().health.ToString() + " / " + player.GetComponent<Fighter>().maxHealth.ToString();
        powerText.text = player.GetComponent<Fighter>().damage.ToString();//печатаем текущий сила в поле вывода
    }

    public void GetExp(int exp)//функция получения опыта после убийства монстров, вызывается в другом скрипте
    {
        currentExp += exp;//прибавляем опыт за убийство к текущему опыту
    }

    void LevelUp()//функция левел апа
    {
        if (currentExp >= nextLevelExp)//если количетво опыта равно или больше количеству опыта, необходмому для
        {                              //поднятия уровня
            sounds[3].Play();
            level++;//увеличиваем уровень
            currentExp -= nextLevelExp;//вычитаем из текщего опыта опыт на уровень
            nextLevelExp += 50;//увеличиваем порог на следующий уровень
            player.GetComponent<Fighter>().LevelDamage(0.02f);//увеличиваем урон с уровнем
            player.GetComponent<Fighter>().LevelHealth(4);//увеличиваем жизни с уровнем
        }
    }

    //загружаем уровень
     public void LoadLevelExp()
    {
        level = SaveLoad.savedGame.PLAYERLEVEL;//берем уровень из зугруженного файла
        currentExp = SaveLoad.savedGame.PLAYEREXP;//берем текущий опыт из загруженного файла
        GetComponent<Fighter>().health = SaveLoad.savedGame.HEALTH;//берем здоровье игрока

        for (int i = level; i > 0; i--)//за все уровни, что загрузили, мы долэжны скалировать атаку и количество опыта
        {
            nextLevelExp += 50;//увеличиваем порог на следующий уровень
            player.GetComponent<Fighter>().LevelDamage(0.02f);//увеличиваем урон с уровнем
            player.GetComponent<Fighter>().LevelHealth(4);//увеличиваем жизни с уровнем
        }
        if (SaveLoad.savedGame.BOSSONEKILLED == 1)
            LevelingSystem.dogBossKill = true;
        if (SaveLoad.savedGame.BOSSTWOKILLED == 1)
            LevelingSystem.kBossKill = true;
        if (SaveLoad.savedGame.BOSSTHREEKILLED == 1)
            LevelingSystem.coyoteBossKill = true;
    }
}
