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
        CameraDevice.Instance.Stop();
        CameraDevice.Instance.Deinit();
        //GetComponent<VuforiaBehaviour>().enabled = false;
        //GetComponent<DefaultInitializationErrorHandler>().enabled = false;
    }

    void OnResume()
    {
        CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_DEFAULT);
        CameraDevice.Instance.Start();
        //GetComponent<VuforiaBehaviour>().enabled = true;
        //GetComponent<DefaultInitializationErrorHandler>().enabled = true;
    }
}
