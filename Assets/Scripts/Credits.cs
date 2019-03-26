using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class Credits : MonoBehaviour {

    public Text textField;
    public float scrollSpeed;
    private string story;
    private int delayTime = 0;
    private float counter = 0;

    // Use this for initialization
    void Start()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
        /*story = "\n" + "\n" + "\n" + "\n" + "\n" +
                "Спасибо за игру!" + "\n" +
                "Надеюсь, вам понравилось" + "\n" +
                "Почти все модели были взяты с сайта scetchfab." + "\n" +
                "Большое количество музыкальных, графических ресурсов были взяты с сайта opengameart" + "\n" +
                "Огромную благодарность выражаю Орку-Подкастеру за его творчество," + "\n" +
                "Надеюсь, он не  разгневается за использование его образа в игре." + "\n" +
                "Если у Вас есть замечания, конструктивная критика, то пишите сюда:" + "\n" +
                "email: whitecoyotegames@gmail.com" + "\n" +
                "vk: vk.com/whitecoyotegames" + "\n"
                ;*/
        //StartCoroutine(StoryType());
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 10.0f)
        {
            if (Input.anyKey)
            {
                StopIntro();
            }
        }
    }

    IEnumerator StoryType()
    {
        for (int i = 0; i < story.Length; i++)
        {
            delayTime++;
            textField.text += story[i];
            Vector3 moveUp = new Vector3(textField.transform.position.x, textField.transform.position.y + scrollSpeed, textField.transform.position.z);
            textField.transform.position = moveUp;
            yield return 0;
        }
    }

    void StopIntro()
    {
        GameObject.Find("LoadingScreenUI").GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene("MainMenu");
    }
}
