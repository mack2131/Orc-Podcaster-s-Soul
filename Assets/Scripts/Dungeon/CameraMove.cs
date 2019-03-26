using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, 0);
    public float rotationSpeed;

    private float currentYRotation;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(target);  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            canMove = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void LateUpdate()
    {
        if(canMove)
            MoveCamera();

    }

    void MoveCamera()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = -Input.GetAxis("Mouse Y");

        if (vertical != 0)
        {
            float temp = Mathf.Clamp(currentYRotation + vertical * rotationSpeed * Time.deltaTime, -20, 35);
            if (temp != currentYRotation)
            {
                float rot = temp - currentYRotation;
                transform.RotateAround(target.position, transform.right, rot);
                currentYRotation = temp;
            }
        }
        if (horizontal != 0)
            transform.RotateAround(target.position, Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
        transform.LookAt(target.position + offset);
    }
}
