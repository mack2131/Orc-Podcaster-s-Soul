using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayOrkMenuAnimation : MonoBehaviour {

    public AnimationClip idleOrkMenu;

	// Use this for initialization
	void Start () 
    {
        //Destroy(GameObject.Find("PlayerOrkpod").gameObject);
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        gameObject.GetComponent<Animation>().CrossFade(idleOrkMenu.name);
	}
}
