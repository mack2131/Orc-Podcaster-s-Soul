using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PlayerData {

    public static PlayerData data = new PlayerData();
    /* ПОЗИЦИЯ ИГРОЕА, СЦЕНА, В КОТОРЫХ ПРОИЗВЕДЕНО    */
    /* СОХРАНЕНИЕ                                      */

    public  Vector3 PLAYERPOSITION;
    public string CURRENTSCENE;
    public int[] INVENTORY = new int[100];

    public int MONEY;

    public float HEALTH;
    public int PLAYERLEVEL;
    public int PLAYEREXP;

    public int HEADSLOT;
    public int WEAPONSLOT;

    public int[] STORAGE = new int[56];

    public int[] QUESTS = new int[30];

    public int[] LOOTEDCHESTS = new int[50];
    public int[] READEDBOOKS = new int[32];

    public int WELL;

    public int BOSSONEKILLED;
    public int BOSSTWOKILLED;
    public int BOSSTHREEKILLED;


    /* ПОЗИЦИИ НАЧАЛЬНЫХ ПОЗИЦИЙ ИГРОКА В КАЖДОЙ СЦЕНЕ */
    /* нужны для того чтобы игрок начинал сцену там,   */
    /* откуда ушел с нее, т.е. около портала           */

    public Vector3 HUBLEVELSTARTPOSITION;
    public Vector3 TUTORIALLEVELSTARTPOSITION;
    public Vector3 PROLOGUELEVELSTARTPOSITION;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	}
}
