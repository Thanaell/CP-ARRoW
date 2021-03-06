﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ce script est associé au prefab : CluePrefabWithParticles.
 * Il est utilisé pour gèrer le son de la clé
 */

public class SoundClueManager : MonoBehaviour {

    /*le son émis lorsque la clé existe*/
    public AudioClip audioCall;
    /* le son émis lorsqu'on recupere la clé*/
    public AudioClip audioClueCollected;

    public AudioSource audioSource;

    
    float timeToStart;

    
    void Start()
    {
        timeToStart = 0.0f;
    }

    /*appelé lorsqu'on collecte la clé (appelée par ClueInteractionScript)*/
    void ClueCollected()
    {
        audioSource.Stop();
        audioSource.clip = audioClueCollected;
        audioSource.volume = 0.50f;
        audioSource.PlayOneShot(audioSource.clip);
    }

   
    void Update()
    {
        /* l'émission de son est décalé de 1s par rapport la création de l'objet*/
        if (!audioSource.isPlaying && timeToStart > 1)
        {
            audioSource.clip = audioCall;
            audioSource.loop = true;
            audioSource.volume = 0.60f;
            audioSource.Play();
        }
        timeToStart += Time.deltaTime;
    }
}
