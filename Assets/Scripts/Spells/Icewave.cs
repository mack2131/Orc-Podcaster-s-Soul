using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class Icewave : MonoBehaviour {

    public GameObject target;//цель фаербола
    public float speed;//скорость полета
    public float damage;//урон

    private float counter;
    private bool firstHit;

	// Use this for initialization
	void Start () 
    {
        Magic.casting = false;
        transform.position = target.transform.position;
        counter = 0;
        firstHit = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (firstHit == true)
        {
            //target.GetComponent<Mob>().GetHit(damage);
            if (target.tag != "Boss Dark Wood" && target.tag != "Library Boss" && target.tag != "CoyoteBoss")
                target.GetComponent<Mob>().GetHit(damage);//бьем цель с уроном damage

            if (target.tag == "Boss Dark Wood")
                target.GetComponent<DogBoss>().GetHit(damage);

            if (target.tag == "Library Boss")
                target.GetComponent<KBoss>().GetHit(damage);

            if (target.tag == "CoyoteBoss")
                target.GetComponent<KBoss>().GetHit(damage);

            firstHit = false;
        }
        DestroyWave();
	}

    void DestroyWave()
    {
        counter += Time.deltaTime;
        if (counter > 5)
            Destroy(gameObject);
    }
}
