using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FPSCamera : MonoBehaviour {

    public GameObject player;

	public float mouseSentivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

	// Use this for initialization
	void Start () 
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotY = rotation.y;
        rotX = rotation.x;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Camera rotation
        float mouseX = Input.GetAxis("Mouse Y");
        float mouseY = Input.GetAxis("Mouse X");
        rotX += -mouseX * mouseSentivity * Time.deltaTime;
        rotY += mouseY * mouseSentivity * Time.deltaTime;
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        player.transform.rotation = localRotation;
    }
}
