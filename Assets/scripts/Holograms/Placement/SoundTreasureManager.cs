using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTreasureManager : MonoBehaviour {

    public AudioClip audioTreasureClosed;
    public AudioClip audioTreasureOpened;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource.clip = audioTreasureClosed;
        audioSource.loop = true;
        audioSource.volume = 0.18f;
        audioSource.Play();
	}
	
	void OpenTreasure()
    {
        audioSource.Stop();
        audioSource.clip = audioTreasureOpened;
        audioSource.loop = false;
        audioSource.volume = 0.55f;
        audioSource.PlayOneShot(audioTreasureOpened);
    }
}
