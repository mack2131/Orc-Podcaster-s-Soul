using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class Funfetti : MonoBehaviour {
    
    [SerializeField]
    public GameObject player;//цель для кофетти - игрок
    public ParticleSystem paricle;//сами частицы
    public float speed;//скорость движения частиц
    public float mainSpeed;//скорость двжиения самого объекта
    public int funMoney;//количество денег, ктоторое получит игрок
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];//хранилище частиц

	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");//ищем игрока
        paricle.GetComponent<ParticleSystem>();//берем частицы в хранилище
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (paricle.isPlaying)//если проигрываются система частиц
        {
            /*двигаем все частицы к игроку*/
            int length = paricle.GetParticles(particles);
            Vector3 playerPos = player.transform.position;
            for (int i = 0; i < length; i++)
            {
                particles[i].position = particles[i].position + (playerPos - particles[i].position) / (particles[i].remainingLifetime) * Time.deltaTime * speed;
            }
            paricle.SetParticles(particles, length);
        }
        /*двигаем всю систему к игроку, пока она не будет очень близка к нему*/
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * mainSpeed);
        if (Vector3.Distance(transform.position, player.transform.position) < 0.5)
        {

            Destroy(gameObject);
        }
	}
}
