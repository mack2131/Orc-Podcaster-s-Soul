using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRocks : MonoBehaviour {

    public float amplitude;

    private Vector3 posOffset = new Vector3();
    private Vector3 temPos = new Vector3();

	// Use this for initialization
	void Start () 
    {
        posOffset = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Flying();
	}

    void Flying()/*движение вверх-вниз объекта*/
    {
        temPos = posOffset;
        temPos.y += Mathf.Sin((Time.fixedTime * Mathf.PI * 1f) * amplitude);
        transform.position = temPos;
    }
}
