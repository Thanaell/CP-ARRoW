using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PopBoxes : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    public GameObject Cubes;

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
            Cubes.SetActive(true);
            Debug.Log("Image found, changed color of cube");
        }
        else
        {
            Cubes.SetActive(false);
            Debug.Log("Image lost, changed color of cube");
        }
    }
}