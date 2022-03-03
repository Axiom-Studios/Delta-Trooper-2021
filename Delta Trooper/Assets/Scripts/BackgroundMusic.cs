using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    List<AudioClip> levelMusic;

    int level;
    int currentMusic;


    void Start()
    {
        level = SpawnController.level;
        currentMusic = 0;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = levelMusic[0];
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        level = SpawnController.level;
        if (level != currentMusic && PlayerMovement.lives > 0)
        {
            audioSource.clip = levelMusic[level];
            audioSource.Play();
            currentMusic = level;
            Debug.Log("Now playing: " + currentMusic + "\nLevel: " + level);
        }
    }
}
