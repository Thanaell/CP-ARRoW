﻿using System;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

public class SpatialUnderstandingState : Singleton<SpatialUnderstandingState>, IInputClickHandler, ISourceStateHandler
{
    public float MinAreaForStats = 5.0f;
    public float MinAreaForComplete = 15.0f;
    public float MinHorizAreaForComplete = 2.0f;
    public float MinWallAreaForComplete = 10.0f;

    private uint trackedHandsCount = 0;

    public TextMesh DebugDisplay;
    public TextMesh DebugSubDisplay;

    private bool _triggered;
    public bool HideText = false;

    private bool ready = false;

    private bool drawSpacialMaping = true;

    private string _spaceQueryDescription;

    public TargetDetection targetDetection;

    bool clickDetected = false;

    public string SpaceQueryDescription
    {
        get
        {
            return _spaceQueryDescription;
        }
        set
        {
            _spaceQueryDescription = value;
        }
    }



    public bool DoesScanMeetMinBarForCompletion
    {
        get
        {
            // Only allow this when we are actually scanning
            if ((SpatialUnderstanding.Instance.ScanState != SpatialUnderstanding.ScanStates.Scanning) ||
                (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
            {
                return false;
            }

            // Query the current playspace stats
            IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
            if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
            {
                return false;
            }
            SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();

            // Check our preset requirements
            if ((stats.TotalSurfaceArea > MinAreaForComplete) &&
                (stats.HorizSurfaceArea > MinHorizAreaForComplete) &&
                (stats.WallSurfaceArea > MinWallAreaForComplete) )
            {
                return true;
            }
            return false;
        }
    }

    public string PrimaryText
    {
        get
        {
            if (HideText)
                return string.Empty;

            // Display the space and object query results (has priority)
            if (!string.IsNullOrEmpty(SpaceQueryDescription))
            {
                return SpaceQueryDescription;
            }

            // Scan state
            if (SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
            {
                switch (SpatialUnderstanding.Instance.ScanState)
                {
                    case SpatialUnderstanding.ScanStates.Scanning:
                        // Get the scan stats
                        IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
                        if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
                        {
                            return "playspace stats query failed";
                        }

                        // The stats tell us if we could potentially finish
                        if (DoesScanMeetMinBarForCompletion)
                        {
                            return "When ready,scan a target to finalize your playspace";
                        }
                        return "Walk around and scan in your playspace";
                    case SpatialUnderstanding.ScanStates.Finishing:
                        return "Finalizing scan (please wait)";
                    case SpatialUnderstanding.ScanStates.Done:
                        return "Scan complete";
                    default:
                        return "ScanState = " + SpatialUnderstanding.Instance.ScanState;
                }
            }
            return string.Empty;
        }
    }

    public Color PrimaryColor
    {
        get
        {
            ready = DoesScanMeetMinBarForCompletion;
            if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning)
            {
               // if (trackedHandsCount > 0)
                //{
                    return ready ? Color.green : Color.red;
                //}
                //return ready ? Color.yellow : Color.white;
            }

            // If we're looking at the menu, fade it out
            float alpha = 1.0f;

            // Special case processing & 
            return (!string.IsNullOrEmpty(SpaceQueryDescription)) ?
                (PrimaryText.Contains("processing") ? new Color(1.0f, 0.0f, 0.0f, 1.0f) : new Color(1.0f, 0.7f, 0.1f, alpha)) :
                new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }

    public string DetailsText
    {
        get
        {
            if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.None)
            {
                return "";
            }

            // Scanning stats get second priority
            if ((SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
                (SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
            {
                IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
                if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
                {
                    return "Playspace stats query failed";
                }
                SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();

                // Start showing the stats when they are no longer zero
                if (stats.TotalSurfaceArea > MinAreaForStats)
                {
                    SpatialMappingManager.Instance.DrawVisualMeshes = false;
        

                    string subDisplayText = string.Format("totalArea : {0:0.0} / {1},  ", stats.TotalSurfaceArea,MinAreaForComplete);
                    subDisplayText += string.Format("horizArea : {0:0.0} / {1},  ", stats.HorizSurfaceArea, MinHorizAreaForComplete);
                    subDisplayText += string.Format("wallArea : {0:0.0} / {1}", stats.WallSurfaceArea, MinWallAreaForComplete);

                    return subDisplayText;
                }
                return "";
            }
            return "";
        }
    }

    private void Update_DebugDisplay()
    {
        // Basic checks
        if (DebugDisplay == null)
        {
            return;
        }

        // Update display text
        DebugDisplay.text = PrimaryText;
        DebugDisplay.color = PrimaryColor;
        DebugSubDisplay.text = DetailsText;
    }

    private void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    // Update is called once per frame
    public ObjectPlacer Placer;

    private void Update()
    {
        if (targetDetection.isTargetDetected || clickDetected) { 
            if (ready &&
                (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
                !SpatialUnderstanding.Instance.ScanStatsReportStillWorking)
            {
                SpatialUnderstanding.Instance.RequestFinishScan();
            }
         }

        // Updates
        Update_DebugDisplay();

        if (!_triggered && SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Done)
        {
            drawSpacialMaping = false;
            _triggered = Placer.CreateScene();
        }
        else
        {
            // hide mesh
            var customMesh = SpatialUnderstanding.Instance.GetComponent<SpatialUnderstandingCustomMesh>();
            customMesh.DrawProcessedMesh = false;
        }

        SpatialMappingManager.Instance.DrawVisualMeshes = drawSpacialMaping;

    }
    

    /*
     * ces trois events est appelée lorsque AirTap est detectée
     * On va le supprimer par la suite si y aurai aucune utilité
     */ 
    public void OnInputClicked(InputClickedEventData eventData)
    {
        /* if (ready &&
             (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
             !SpatialUnderstanding.Instance.ScanStatsReportStillWorking)
         {
             SpatialUnderstanding.Instance.RequestFinishScan();
         }*/
        clickDetected = true;
    }
    

    void ISourceStateHandler.OnSourceDetected(SourceStateEventData eventData)
    {
        trackedHandsCount++;
    }

    void ISourceStateHandler.OnSourceLost(SourceStateEventData eventData)
    {
        trackedHandsCount--;
    }
}