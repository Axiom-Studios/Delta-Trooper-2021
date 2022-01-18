using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    InputMaster controls;
    public bool dialoguePaused = false;
    public bool menuPaused = false;
    
    public Text dialogueText;
    public GameObject dialogueBox;
    public GameObject pauseMenu;
    public float messageDuration = 5f;
    public static List<string> sentencesQueue = new List<string>();
    float t1 = 0;
    // Start is called before the first frame update
    void Start()
    {
        controls = new InputMaster();
		controls.Enable();

        dialogueText = GetComponentInChildren<Text>();
        dialogueText.text = "[Text component loaded]";
        sentencesQueue.Add("Woah woah woah");
    }

    // Update is called once per frame
    void Update()
    {
        // Pause

        if (dialoguePaused)
        {
            Time.timeScale = 0;
            dialogueBox.SetActive(true);
        }
        else
        {
            if (!menuPaused)
            {
                Time.timeScale = 1;
            }
            dialogueBox.SetActive(false);
        }

        if (menuPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            if (!dialoguePaused)
            {
                Time.timeScale = 1;
            }
            pauseMenu.SetActive(false);
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StartCoroutine(Pause());
        }

        // Dialogue

        if (sentencesQueue.Count == 0)
        {
            dialoguePaused = false;
        }
        else
        {
            dialoguePaused = true;
            if (dialogueText.text != sentencesQueue[0])
            {
                dialogueText.text = sentencesQueue[0];
                t1 = Time.unscaledTime;
            }
            if (menuPaused)
            {
                t1 = Time.unscaledTime;
            }

            if (Time.unscaledTime - t1 > messageDuration)
            {
                sentencesQueue.RemoveAt(0);
                if (sentencesQueue.Count == 0)
                {
                    dialoguePaused = false;
                }
            }
        }
    }

    IEnumerator Pause()
    {
        yield return new WaitUntil(() => !Keyboard.current.escapeKey.IsPressed());
        menuPaused = !menuPaused;
    }

    public void Resume()
    {
        if (menuPaused)
        {
            menuPaused = false;
        }
    }
}
