using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip level0;
    public AudioClip level1;
    public AudioClip level2;
    public AudioClip level3;
    List<AudioClip> levelMusic = new List<AudioClip>();


    int level = SpawnController.level;
    int currentMusic = 0;


    void Start()
    {
        levelMusic.Add(level0);
        levelMusic.Add(level1);
        levelMusic.Add(level2);
        levelMusic.Add(level3);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = level0;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (level != currentMusic)
        {
            audioSource.clip = levelMusic[level];
            audioSource.Play();
        }
    }
}
