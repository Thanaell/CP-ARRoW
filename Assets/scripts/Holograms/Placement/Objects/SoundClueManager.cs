using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClueManager : MonoBehaviour {


    public AudioClip audioCall;
    public AudioClip audioClueCollected;
    public AudioClip audioClueSeen;
    public AudioSource audioSource;

    float timeToStart;

    
    void Start()
    {
        timeToStart = 0.0f;
        //audioSource.clip = audioCall;
        //audioSource.loop = true;
        //audioSource.volume = 0.40f;
        //audioSource.Play();
    }

    void ClueCollected()
    {

        audioSource.Stop();
        audioSource.clip = audioClueCollected;
        audioSource.volume = 0.50f;
        audioSource.PlayOneShot(audioSource.clip);
    }

    void ClueSeen()
    {
        if (audioSource.clip != audioClueSeen || !audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = audioClueSeen;
            audioSource.volume = 0.60f;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying && timeToStart > 1)
        {
            audioSource.clip = audioCall;
            audioSource.loop = true;
            audioSource.volume = 0.40f;
            audioSource.Play();
        }
        timeToStart += Time.deltaTime;
    }
}
