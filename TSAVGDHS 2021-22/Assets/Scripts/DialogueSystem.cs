using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public bool paused = true;
    
    public Text dialogueText;
    public GameObject dialogueBox;
    // Start is called before the first frame update
    void Start()
    {
        dialogueText = GetComponentInChildren<Text>();
        dialogueText.text = "Hola it worked";
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            dialogueBox.SetActive(true);
        }
        else
        {
            dialogueBox.SetActive(false);
        }
    }
}
