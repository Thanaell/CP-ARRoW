using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClueManager : MonoBehaviour {


    public AudioClip audioCall;
    public AudioClip audioClueCollected;
    public AudioSource audioSource;

    
    void Start()
    {
        audioSource.clip = audioCall;
        audioSource.loop = true;
        audioSource.volume = 0.40f;
        audioSource.Play();
    }

    void ClueCollected()
    {

        audioSource.Stop();
        audioSource.clip = audioClueCollected;
        audioSource.volume = 0.40f;
        audioSource.PlayOneShot(audioSource.clip);
    }
}
