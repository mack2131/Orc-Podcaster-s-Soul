using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealthRing : MonoBehaviour {

    public Fighter player;
    public Mob target;
    public float healthPercentage;

    private Canvas canvas;
    private Image ring;
    public Mob previousTarget;
    public Mob currentTarget;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (player.opponent != null /*&& player.countDown > 0*/)
        {
            //DrawCircle();
        }
        else
        {
            target = null;
            healthPercentage = 0;
        }
	}

    void OnGUI()
    {
    }

    void DrawCircle()
    {
        target = player.opponent.GetComponent<Mob>();
        healthPercentage = target.health / target.maxHealth;
        canvas = player.opponent.GetComponentInChildren<Canvas>();
        ring = canvas.GetComponentInChildren<Image>();
        canvas.enabled = true;
        ring.fillAmount = healthPercentage;
        currentTarget = target;
    }

}
