using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class PlayerEquipment : MonoBehaviour {

    public GameObject player;//объект игрока

    private bool isHeadEquip;//одета ли голова
    private bool newHeadEquip;//менялось ли голова, переменная нужна для того, чтобы менять ??? только один раз при изменении головы
    private bool isWeaponEquip;//одета ли оружие
    private bool newWeaponEquip;//менялось ли оружие, переменная нужна для того, чтобы менять урон только один раз при изменении оружия

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        //при загрузке игры
        if (SaveLoad.wasLoaded == true)
        {
            LoadEquipment();//загружаем экипировку
            SaveLoad.equipmentLoaded = false;//говорим, что загрузились
        }

        CheckEquipment();//проверяем что одето

        if(isHeadEquip == true && newHeadEquip == true)//если есть голова и только поместили голову
            GetHeadEquipment();//функция головы

        if (isHeadEquip == false)//если нет в голове ничего, в т.ч. и мозгов
        {   //смотрим, есть ли у объекта слота для головы дети
            player.GetComponent<Fighter>().AddHeadPower(0);
            if (GameObject.Find("Place for Helmet").transform.childCount == 1)//если есть,значит есть голов
                Destroy(GameObject.Find("Place for Helmet").transform.GetChild(0).gameObject);//убираем голову
        }

        if (isWeaponEquip == true && newWeaponEquip == true)//если есть голова и только поместили оружие
            GetWeaponEquipment();//функция оружия

        if (isWeaponEquip == false) //если нет оружия
        {
            player.GetComponent<Fighter>().AddWeaponPower(0);//урон от оружия 0
            if (GameObject.Find("Place for Weapon").transform.childCount == 1)
                Destroy(GameObject.Find("Place for Weapon").transform.GetChild(0).gameObject);
        }
	}

    //функция, которая определяет есть ли в слотах экипировки предметы и были ли они перемещены или изменены
    void CheckEquipment()
    {
        if (Inventory.slots[100].transform.childCount == 1)
            isHeadEquip = true;
        else
        {
            isHeadEquip = false;
            newHeadEquip = true;
        }

        if (Inventory.slots[101].transform.childCount == 1)
            isWeaponEquip = true;
        else
        {
            isWeaponEquip = false;
            newWeaponEquip = true;
        }
    }

    public void GetHeadEquipment()//функция головы
    {
        Item helm = Inventory.slots[100].GetComponentInChildren<ItemData>().item;
        player.GetComponent<Fighter>().AddHeadPower(helm.vitality);
        //player.GetComponent<Fighter>().AddWeaponPower(weapon.power);
        GameObject helmet = Instantiate(helm.model);
        //helmet.transform.parent = GameObject.Find("Place for Helmet").transform;
        helmet.transform.SetParent(GameObject.Find("Place for Helmet").transform, false);
        helmet.transform.position = GameObject.Find("Place for Helmet").transform.position;
        helmet.transform.rotation = GameObject.Find("Place for Helmet").transform.rotation;
        helmet.transform.localPosition = new Vector3(0 + helm.model.transform.position.x,
                                                0 + helm.model.transform.position.y,
                                                0 + helm.model.transform.position.z);
        //helmet.transform.localScale = helm.model.transform.localScale;
        newHeadEquip = false;
    }

    public void GetWeaponEquipment()//функция оружия
    {
        Item weapon = Inventory.slots[101].GetComponentInChildren<ItemData>().item;
        player.GetComponent<Fighter>().AddWeaponPower(weapon.power);
        GameObject handedWeapon = Instantiate(weapon.model);
        //handedWeapon.transform.parent = GameObject.Find("Place for Weapon").transform;
        handedWeapon.transform.SetParent(GameObject.Find("Place for Weapon").transform, false);
        handedWeapon.transform.position = GameObject.Find("Place for Weapon").transform.position;
        handedWeapon.transform.rotation = GameObject.Find("Place for Weapon").transform.rotation;
        //handedWeapon.transform.localScale = weapon.model.transform.localScale;
        newWeaponEquip = false;
    }

    //функция отправляет данные о слотах экипировки для сохранения
    public static void SendEquipmentState()
    {
        if (Inventory.slots[100].transform.childCount == 1)//если есть голова, то передаем айди предмета в голове
            PlayerData.data.HEADSLOT = Inventory.slots[100].GetComponentInChildren<ItemData>().item.id;
        else PlayerData.data.HEADSLOT = -1;//если нет, то передаем -1,значит пусто

        if (Inventory.slots[101].transform.childCount == 1)//если есть оружие, передаем его айди
            PlayerData.data.WEAPONSLOT = Inventory.slots[101].GetComponentInChildren<ItemData>().item.id;
        else PlayerData.data.WEAPONSLOT = -1;//если нет, то передаем -1,значит пусто
    }

    //функция загрузки инвенторя
    void LoadEquipment()
    {
        if (SaveLoad.savedGame.HEADSLOT != -1)//если во время сохранения юыл предмет
        {
            GetComponent<Inventory>().PlayerAddEquipment(SaveLoad.savedGame.HEADSLOT, 100);//ставим его в соответсвующий слот
            GetHeadEquipment();//экипируем голову
        }
        if (SaveLoad.savedGame.WEAPONSLOT != -1)
        {
            GetComponent<Inventory>().PlayerAddEquipment(SaveLoad.savedGame.WEAPONSLOT, 101);//ставим его в соответсвующий слот
            GetWeaponEquipment();//экипируем оружие
        }
    }
}
