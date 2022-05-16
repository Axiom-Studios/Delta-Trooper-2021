using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
	AudioSource audioSource;
	public AudioClip loopAudio;
	void Start() {
		audioSource = gameObject.GetComponent<AudioSource>();
	}

    void Update()
    {
		if (!audioSource.isPlaying) {
			audioSource.clip = loopAudio;
			audioSource.loop = true;
			audioSource.Play();
		}
    }
}
