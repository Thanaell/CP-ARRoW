using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VuforiaCommand : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnStop()
    {
        VuforiaBehaviour.Instance.enabled = false;
        GetComponent<DefaultInitializationErrorHandler>().enabled = false;
    }

    void OnResume()
    {
        VuforiaBehaviour.Instance.enabled = true;
        GetComponent<DefaultInitializationErrorHandler>().enabled = true;
    }
}
