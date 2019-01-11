using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class WalkedDistance : MonoBehaviour {
    private float oldX;
    private float newX;
    private float oldZ;
    private float newZ;
    private float timer;
    public float walkedDistance;
    string path = Path.Combine(Application.persistentDataPath, "MyFile.txt");


    // Use this for initialization
    void Start () {
        oldX = transform.position.x;
        oldZ = transform.position.z;
        File.WriteAllText(path, "Walked distance every half second in last session : " + walkedDistance.ToString());
    }
	
	// Update is called once per frame
	void Update () {
        if( timer >= 0.5)
        {
            timer = 0;
            walkedDistance += Mathf.Sqrt((oldX - newX) * (oldX - newX) + (oldZ - newZ) * (oldZ - newZ));
            Debug.Log(walkedDistance);
            oldZ = newZ;
            oldX = newX;
            newZ = transform.position.x;
            newX = transform.position.z;

            //write walkedDistance to file in System/File Explorer/LocalAppData/CPARRoW/LocalState  
            File.AppendAllText(path, walkedDistance.ToString());
        }
        timer += Time.deltaTime;	
	}
}
