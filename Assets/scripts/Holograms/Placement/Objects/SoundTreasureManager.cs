using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ce script est associé aux prefabs de Tresors.
 * Il est utilisé pour gèrer le son du son
 * (les fonctions sont appelées par TreasureInteractionScript
 */

public class SoundTreasureManager : MonoBehaviour {

    /* le son émis lorsque le coffre est fermé et on le fixe*/
    public AudioClip audioTreasureClosed;
    /* le son émis lorsque le coffre est ouvert et on le fixe*/
    public AudioClip audioTreasureOpened;
    /* le son émis lorsqu'on a collecté toutes les clés (le coffre appele le joueur)*/
    public AudioClip audioCluesCollected;

    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource.clip = audioTreasureClosed;
        audioSource.loop = false;
        audioSource.volume = 0.40f;
        audioSource.PlayOneShot(audioTreasureClosed);
    }

    /*appelé lorsque le coffre fixé par le cursor est ouvert*/
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

    /*appelé lorsque le coffre fixé par le cursor est fermé*/
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

    /*cette variable permet de lancer qu'une seule fois le son audioCluesCollected*/
    bool isAudioCluesCollectedPlayed = false;

    /*appelé lorsque toutes les clés sont collectées*/
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
