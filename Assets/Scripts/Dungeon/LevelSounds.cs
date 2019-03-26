using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSounds : MonoBehaviour
{
    public AudioClip[] levelSounds = new AudioClip[11];
    private string sceneName;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        SetMusic();
    }

    void Update()
    {
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            sceneName = SceneManager.GetActiveScene().name;
            SetMusic();
        }
    }

    void SetMusic()
    {
        switch(sceneName)
        {
            case "Tutorial":
                {
                    GetComponent<AudioSource>().clip = levelSounds[0];
                    break;
                }
            case "Hub":
                {
                    GetComponent<AudioSource>().clip = levelSounds[1];
                    break;
                }
            case "Dark Wood":
                {
                    GetComponent<AudioSource>().clip = levelSounds[2];
                    break;
                }
            case "Kudykina Mountain":
                {
                    GetComponent<AudioSource>().clip = levelSounds[4];
                    break;
                }
            case "The Way to Uganda":
                {
                    GetComponent<AudioSource>().clip = levelSounds[5];
                    break;
                }
            case "Coyote Castle":
                {
                    GetComponent<AudioSource>().clip = levelSounds[7];
                    break;
                }
            case "Castle Arena":
                {
                    GetComponent<AudioSource>().clip = levelSounds[8];
                    break;
                }
            case "Easter Egg":
                {
                    GetComponent<AudioSource>().clip = levelSounds[10];
                    break;
                }
        }
        GetComponent<AudioSource>().Play();
    }

    public void DogBoss()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = levelSounds[3];
        GetComponent<AudioSource>().Play();
    }

    public void KBoss()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = levelSounds[6];
        GetComponent<AudioSource>().Play();
    }

    public void CoyoteBoss()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = levelSounds[9];
        GetComponent<AudioSource>().Play();
    }

    public void Reset()
    {
        SetMusic();
    }
}
