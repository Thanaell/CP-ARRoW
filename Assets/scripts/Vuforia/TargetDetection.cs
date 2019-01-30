using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Classe permettant de savoir si la caméra détecte la cible Vuforia ou non
/// </summary>
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

/// <summary>
/// Méthode permettant de déterminer si la cible est détectée ou non par la caméra
/// </summary>
/// <param name="previousStatus">statut passé du tracking de la cible</param>
/// <param name="newStatus">statut présent du tracking de la cible</param>
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
        }

    }
}