using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickToMove : MonoBehaviour {
   
    private Vector3 position;//позиция
    public float speed;//скорость
    public float rotationSpeed;//скорость вращения
    public CharacterController controller;//контроллер игрока
    public AnimationClip run;//анимация бега
    public AnimationClip idle;//анимация покоя

    public static bool attack;//атакует ли
    public static bool die;//умер ли

    private Animation anim;//анимация для сбора всех компонентов анимации

	// Use this for initialization
	void Start () 
    {
        //position = transform.position;//запоминаем позицию
        anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!attack && !die)//если живы и не атакуем
        {


            /*if (Input.GetMouseButton(0))
            {
                определяем, куда кликнули
                LocatePosition();
            }*/
            Move();//двигаемся
        }
        else ;//иначе мы сдохли или атакуем

	}

    /*void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
    }*/

    void Move()//двигаемся
    {

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.S))//если кнопка нажата долго и была опущена - НАЗАД
        {
            transform.Rotate(Vector3.up, 180);//разворачиваем игрока на 180 
        }
        if (Input.GetKey(KeyCode.D))//если долго нажата кнопка - ВПРАВО 
        {
            transform.Rotate(Vector3.up, 90 * Time.deltaTime * rotationSpeed);//поворачивем персонажи вправо
        }
        if (Input.GetKey(KeyCode.A))//если долго нажата кнопка - ВЛЕВО
        {
            transform.Rotate(Vector3.up, -90 * Time.deltaTime * rotationSpeed);//поворачиваем персонажа влево
        }

        if (Input.GetKey(KeyCode.W))//если долго нажата кнопка ВПЕРЕД
        {
            controller.SimpleMove(transform.forward * speed);//перемщаем игрока вперед
            anim.CrossFade(run.name);//проигрываем анимацию бега
        }
        else//если не двигаем
        {
            anim.CrossFade(idle.name);//проигрываем анимацию спокойствия
        }

        /*if (transform.rotation.x != 0 || transform.rotation.z != 0)
        {
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }*/
    }

    public void Sprint(float newSpeed)
    {
        speed = newSpeed;
    }
}
