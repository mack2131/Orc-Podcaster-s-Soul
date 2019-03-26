using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ScreenResolution : MonoBehaviour {

    private Dropdown dropdown;
    public Toggle fullsreen;
    private bool full;
    

	// Use this for initialization
	void Start () 
    {
        dropdown = GetComponent<Dropdown>();
	}
	
	// Update is called once per frame
    void Update()
    {
        OnValueChanged();
    }

    public void OnValueChanged()
    {
        if (fullsreen.isOn == true)
            full = true;
        else full = false;

        if (dropdown.value == 0)
            Screen.SetResolution(800, 600, full);
        if (dropdown.value == 1)
            Screen.SetResolution(1024, 768, full);
        if (dropdown.value == 2)
            Screen.SetResolution(1280, 1024, full);
        if (dropdown.value == 3)
            Screen.SetResolution(1920, 1080, full);
    }
}
