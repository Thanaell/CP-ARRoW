﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObjectScript : MonoBehaviour {


    public AudioClip audio;

    public void addComponents(GameObject go, int objectId)
    {
        go.AddComponent<InteractionScript>();
        go.GetComponent<InteractionScript>().id = objectId;

        go.AddComponent<AudioSource>();
        go.AddComponent<SoundManager>();
        go.GetComponent<AudioSource>().maxDistance = 2;
        go.GetComponent<AudioSource>().minDistance = 0;
        go.GetComponent<SoundManager>().audioData = go.GetComponent<AudioSource>();
        go.GetComponent<SoundManager>().audio = audio;
    }
}
