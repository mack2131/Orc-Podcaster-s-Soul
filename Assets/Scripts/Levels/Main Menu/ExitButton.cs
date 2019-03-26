using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu{

    public class ExitButton : MonoBehaviour
    {

        private Button exitButton;
        bool pressed;

        // Use this for initialization
        void Start()
        {
            exitButton = this.GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {
            exitButton.onClick.AddListener(ButtonPressed);
            if (Input.GetMouseButtonUp(0) && pressed == true)
            {
                Application.Quit();
                pressed = false;
            }
        }

        void ButtonPressed()
        {
            pressed = true;
        }
    }
}