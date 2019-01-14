using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource audioData;
    public AudioClip audio;


    // Use this for initialization
    void Start () {
        audioData.PlayOneShot(audio, 1);
;	}

}
