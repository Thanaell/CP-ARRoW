using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObjectScript : MonoBehaviour {


    public AudioClip audio;

    public void addComponents(GameObject go)
    {
        go.AddComponent<InteractionScript>();
        go.AddComponent<AudioSource>();
        go.AddComponent<AudioManager>();
        go.GetComponent<AudioSource>().maxDistance = 2;
        go.GetComponent<AudioSource>().minDistance = 0;
        go.GetComponent<AudioManager>().audioData = go.GetComponent<AudioSource>();
        go.GetComponent<AudioManager>().audio = audio;
    }
}
