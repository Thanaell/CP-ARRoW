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
        audioSource.loop = false;
        audioSource.volume = 0.55f;
        audioSource.PlayOneShot(audioTreasureClosed);
    }
	
	void OpenTreasure()
    {
        audioSource.Stop();
        audioSource.clip = audioTreasureOpened;
        audioSource.volume = 0.55f;
        audioSource.PlayOneShot(audioTreasureOpened);
    }

    void CloseTreasure()
    {
        audioSource.Stop();
        audioSource.clip = audioTreasureClosed;
        audioSource.volume = 0.55f;
        audioSource.PlayOneShot(audioTreasureClosed);
    }
}
