using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour {

    public Transform position;
    public int itemId;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1) && IsNear())
        {
            //GameObject.Find("Quest Manager").GetComponent<QuestManager>().isHaveWoodenLog = true;
            GameObject.Find("Inventory System Manager").GetComponent<Inventory>().PlayerAddItem(itemId);
            Destroy(gameObject);
        }
    }

    bool IsNear()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, position.transform.position) < 5)
            return true;
        else return false;
    }
}
