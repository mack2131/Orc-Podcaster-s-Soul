using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class Volume : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {	
	}
	
	// Update is called once per frame
	void Update () 
    {
        OnValueChanged();	
	}

    public void OnValueChanged()
    {
        AudioListener.volume = GetComponent<Slider>().value;
    }
}
