using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollingText : MonoBehaviour {

    public Text textField;
    public Canvas storyTellingUI;
    public AudioClip audioStoryTelling;
    public float pause;
    public float scrollSpeed;
    private string story;
    private AudioSource audio;
    private int delayTime = 0;
    private bool storyMusicPlay;

	// Use this for initialization
	void Start () 
    {
        story = "2019 год:" + "\n" +
                "Население планеты 7.897.691.254 человека." + "\n" +
                "Всё больше людей стали заболевать раковыми заболеваниями." + "\n" +
                "По телевидению только и говорят, что очередная звезда умерла от рака..." + "\n" +
                "Ученые всего мира не могут найти лекарство" + "\n" +
                "\n" +
                "2020 год:" + "\n" +
                "От рака умирает все больше и больше людей. Численность населения планеты уменшилось до 5.342.436.117" + "\n" +
                "Ученые всего мира бьют тревогу. Рак не щадит ни детей ни взрослых." + "\n" +
                "Професора всего мира объединились в попытке найти лекарство." + "\n" +
                "\n" +
                "2021 год:" + "\n" +
                "Численность населения уменшилась до 2.748.617.952 человек." + "\n" +
                "По всему миру трупы свозят на огромные площадки." + "\n" +
                "Испытания новой вакцины ни к чему не привели." + "\n" +
                "Улицы городов пустеют, на работу уже никто не выходит, все стараюстся провести как можно больше времени с родными..." + "\n" +
                "\n" +
                "2022 год:" + "\n" +
                "Численность населения упала до 1.489.153.089 человек." + "\n" +
                "Над лекарством трудится около 10.000 ученых. Но безуспешно." + "\n" +
                "Государства пытаюстся поддерживаьт основые телевизеоные каналы." + "\n" +
                "Люди ждут конец света." + "\n" +
                "Происходят перебои с электричеством." + "\n" +
                "\n" +
                "2023 год, апрель:" + "\n" +
                "Численность населения упала до 1.149.321 человека." + "\n" +
                "Над вакцинной никто не трудится. Города совсем пустые. Лидеры держав уже мертвы. Все пришло в упадок." + "\n" +
                "\n" +
                "2023 год, июль:" + "\n" +
                "Численность населения упала до 569.123 человек." + "\n" +
                "\n" + 
                "2023 год, август" + "\n" +
                "Численность населения 000.000 человек...Хотя, вы все еще живы. Каким-то непонятным образом рак обошел вас стороной и вы..." + "\n" +
                "Последний человек, последний выживший..." + "\n" +
                "Вы нашли себе машину и стараетесь найти других людей, но безуспешно. Электричества нет, еда только консервы и то, что еще не пропало." + "\n" +
                "Электричества нет, вас спасает генератор и фонарики." + "\n" +
                "Насосы, перекрывающие доступ воды к тоннелям метро отключены, все затоплено." + "\n" +
                "\n" +
                "2023 год, сентябрь:" + "\n" +
                "Вы все еще живы и чувствуете себя королем Земли." + "\n" +
                "Зная, что в ближайшее время вода, охлаждающая атомные электростанции скоро испарится, и это приведет к серии ядерных катастроф, вы пытаетесь найти большой корабль. Ведь в морях и океанах безопасней." + "\n" +
                "\n" +
                "2023 год, 17 сентября:" + "\n" +
                "Вы прибыли в портовой город и нашли большой корабль, который позволит превозить огромное количество груза." + "\n" +
                "Этот корабль поможет в поисках таких же как вы, последних выживших...Если такие еще нет" + "\n" +
                "Численность Земли составляет 1 человек...";
        AudioSource.PlayClipAtPoint(audioStoryTelling, Camera.main.transform.position, 0.2f);
        storyMusicPlay = true;
        StartCoroutine(StoryType());
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(storyMusicPlay == true)
            GameObject.Find("One shot audio").transform.position = Camera.main.transform.position;
        if (Input.GetKey(KeyCode.Escape))
        {
            StopIntro();
        }
	}

    IEnumerator StoryType()
    {
        while (delayTime < 100)
        {
            delayTime++;
            yield return 0;
            yield return new WaitForSeconds(pause);
        }
        for (int i = 0; i < story.Length; i++)
        {
            delayTime++;
            textField.text += story[i];
            if (delayTime > 850)
            {
                Vector3 moveUp = new Vector3(textField.transform.position.x, textField.transform.position.y + scrollSpeed, textField.transform.position.z);
                textField.transform.position = moveUp;
            }
            yield return 0;
            yield return new WaitForSeconds(pause);
        }
        while (delayTime < 2500)
        {
            Vector3 moveUp = new Vector3(textField.transform.position.x, textField.transform.position.y + scrollSpeed, textField.transform.position.z);
            textField.transform.position = moveUp;
            if (Input.GetKey(KeyCode.Escape))
            {
                StopIntro();
            }
            yield return 0;
            yield return new WaitForSeconds(pause);
        }
    }

    void StopIntro()
    {
        storyTellingUI.GetComponent<Canvas>().enabled = false;
        Destroy(GameObject.Find("One shot audio"));
        storyMusicPlay = false;
        StopAllCoroutines();
    }
}
