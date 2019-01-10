using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TargetDetection : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    public bool isTargetDetected;

    void Start()
    {
        isTargetDetected = false;
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
            isTargetDetected = true;
            Debug.Log("la cible a ete vue");
        }

    }
}