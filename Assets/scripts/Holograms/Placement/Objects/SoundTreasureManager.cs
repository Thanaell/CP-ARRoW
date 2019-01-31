using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTreasureManager : MonoBehaviour {

    public AudioClip audioTreasureClosed;
    public AudioClip audioTreasureOpened;
    public AudioClip audioCluesCollected;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource.clip = audioTreasureClosed;
        audioSource.loop = false;
        audioSource.volume = 0.40f;
        audioSource.PlayOneShot(audioTreasureClosed);
    }
	
	void OpenTreasure()
    {
        if ((!audioSource.isPlaying || audioSource.clip == audioTreasureClosed || audioSource.clip == audioCluesCollected) & audioSource.clip!= audioTreasureOpened)
        {
            audioSource.Stop();
            audioSource.clip = audioTreasureOpened;
            audioSource.volume = 0.70f;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    void CloseTreasure()
    {
        if (!audioSource.isPlaying & !isAudioCluesCollectedPlayed)
        {
            audioSource.Stop();
            // audioSource.clip = audioTreasureClosed;
            audioSource.volume = 0.40f;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    bool isAudioCluesCollectedPlayed = false;
    void CluesAreCollected()
    {
        if (!isAudioCluesCollectedPlayed)
        {
            audioSource.clip = audioCluesCollected;
            isAudioCluesCollectedPlayed = true;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
