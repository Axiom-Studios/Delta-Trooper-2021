using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    InputMaster controls;
    public bool dialoguePaused = true;
    public bool menuPaused = false;
    
    public Text dialogueText;
    public GameObject dialogueBox;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        controls = new InputMaster();
		controls.Enable();

        dialogueText = GetComponentInChildren<Text>();
        dialogueText.text = "Hola it worked";
    }

    // Update is called once per frame
    void Update()
    {
        if (dialoguePaused)
        {
            dialogueBox.SetActive(true);
        }
        else
        {
            dialogueBox.SetActive(false);
        }

        if (menuPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }

        if (Keyboard.current.escapeKey.wasReleasedThisFrame)
        {
            menuPaused = !menuPaused;
        }
    }

    public void Resume ()
    {
        if (menuPaused)
        {
            menuPaused = false;
        }
    }
}
