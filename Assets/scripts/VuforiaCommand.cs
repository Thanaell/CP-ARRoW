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
        VuforiaRuntime.Instance.Deinit();
        GetComponent<VuforiaBehaviour>().enabled = false;
        GetComponent<DefaultInitializationErrorHandler>().enabled = false;
    }

    void OnResume()
    {
        VuforiaRuntime.Instance.InitVuforia();
        GetComponent<VuforiaBehaviour>().enabled = true;
        GetComponent<DefaultInitializationErrorHandler>().enabled = true;
    }
}
