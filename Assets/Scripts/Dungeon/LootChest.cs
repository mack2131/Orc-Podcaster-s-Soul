using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class LootChest : MonoBehaviour {

    public int ChestId;
    public int itemId;
    public GameObject droppedItem;
    public float rangeRadius;

	// Use this for initialization
	void Start () 
    {
        if (Inventory.lootedLootChests[ChestId] == 1)
            Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    bool InRange()
    {
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < rangeRadius)
         return true;
        else return false;
    }

    void OnMouseOver()//если навдена мышка на моба
    {
        if (Input.GetMouseButtonUp(1))//нажата правая кнопка мыши
        {
            Inventory.lootedLootChests[ChestId] = 1;
            LootItem(itemId);
            Destroy(gameObject);
        }
    }

    void LootItem(int id)
    {
        GameObject newItem = Instantiate(droppedItem);//создаем объект из префаба, который отвечает за визуализацию вещи в открытом мире
        //ставим позицию спавна вещи 
        newItem.transform.position = transform.position;
        newItem.GetComponentInChildren<MeshFilter>().GetComponentInChildren<PressTheTextItemTitle>().itemId = itemId;//говорим выброшенной вещи, какя она
    }

}
