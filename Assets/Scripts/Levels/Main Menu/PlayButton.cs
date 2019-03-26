using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MainMenu{

    public class PlayButton : MonoBehaviour
    {

        private Button button;
        bool pressed;

        // Use this for initialization
        void Start()
        {
            button = this.GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {
            button.onClick.AddListener(Pressed);
            if (Input.GetMouseButtonUp(0) && pressed == true)
            {
                //SceneManager.LoadScene("Boat");
                //Application.LoadLevel("Boat");
                SceneManager.LoadScene("Tutorial");
                //SceneManager.LoadScene("Prologue");
                //SceneManager.LoadScene("Hub");
                GameObject.Find("LoadingScreenUI").GetComponent<Canvas>().enabled = true;
                pressed = false;
            }
        }

        void Pressed()
        {
            pressed = true;
        }
    }
}
