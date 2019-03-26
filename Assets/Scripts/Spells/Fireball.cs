using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Fireball : MonoBehaviour {

    public GameObject target;//цель фаербола
    public float speed;//скорость полета
    public float damage;//урон

	// Use this for initialization
	void Start () 
    {
        Magic.casting = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.LookAt(target.transform);//смотрим на цель
        transform.Translate(Vector3.forward * Time.deltaTime * speed);//движемся к цели
        //если расстояние между целью и фаерболом маленькое
        if (Vector3.Distance(transform.position, target.transform.position) < 2)
        {
            //target.GetComponent<Mob>().GetHit(damage);//вызываем у цели функцию GetHit с параметром урона

            if (target.tag != "Boss Dark Wood" && target.tag != "Library Boss" && target.tag != "CoyoteBoss")
                target.GetComponent<Mob>().GetHit(damage);//бьем цель с уроном damage

            if (target.tag == "Boss Dark Wood")
                target.GetComponent<DogBoss>().GetHit(damage);

            if (target.tag == "Library Boss")
                target.GetComponent<KBoss>().GetHit(damage);

            if (target.tag == "CoyoteBoss")
                target.GetComponent<KBoss>().GetHit(damage);

            Destroy(gameObject);//уничтожаем фаербол
        }
	}
}
