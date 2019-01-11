using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCommands : MonoBehaviour {

    public GameObject cube;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPop()
    {
        cube.SetActive(true);
    }

    void OnKill()
    {
        cube.SetActive(false);
    }
}
