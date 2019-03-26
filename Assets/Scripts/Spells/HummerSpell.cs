using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class HummerSpell : MonoBehaviour {

    public float spinSpeed;
    public int damage;
    public AudioSource[] sounds;
    public float lifetimeSeconds;
    private float lifetimeTimer;
    private float attackCounter;

	// Use this for initialization
	void Start () 
    {
        sounds = GetComponents<AudioSource>();
        sounds[0].Play();
	}
	
	// Update is called once per frame
	void Update () 
    {
        OverLifeTime();
        Spinning();
        if (InRange())
            Attack();
	}

    void OverLifeTime()
    {
        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= lifetimeSeconds)
            Destroy(gameObject);
    }

    void Spinning()
    {
        transform.Rotate(Vector3.right, 90 * Time.deltaTime * spinSpeed);
    }

    bool InRange()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 7)
            return true;
        else return false;
    }

    void Attack()
    {
        attackCounter += Time.deltaTime;
        if (attackCounter > 0.7)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().GetHit(damage);
            attackCounter = 0;
        }
    }
}
