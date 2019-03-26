using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DogBossSpears : MonoBehaviour {

    public Transform target;//цель фаербола
    public float speed;//скорость полета
    public float damage;//урон
    public Transform hitPos;

    private bool hited;
    private float timer;
    public AudioSource[] sounds;

	// Use this for initialization
	void Start () 
    {
		hited = false;
        timer = 0;
        sounds = GetComponents<AudioSource>();
        sounds[0].Play();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (transform.position.y > 7.6)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            if (Vector3.Distance(hitPos.position, target.position) < 4.2)
            {
                if (!hited)
                {
                    target.GetComponent<Fighter>().GetHit(damage);
                    hited = true;
                }
            }
        }
        else
        {
            hited = true;
            timer += Time.deltaTime;
            if (timer > 10.0f)
                Destroy(gameObject);
        }
	}
}
