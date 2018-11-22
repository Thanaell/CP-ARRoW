using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ChangeColorCube : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    public GameObject myCube;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED
            //|| newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED
            )
        {
            myCube.GetComponent<Renderer>().material.color = Color.green;
            Debug.Log("Image found, changed color of cube");
        }
        else
        {
            myCube.GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("Image lost, changed color of cube");
        }
    }
}