using System;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

public class SpatialUnderstandingUniqueState : Singleton<SpatialUnderstandingUniqueState>, IInputClickHandler, ISourceStateHandler
{
    public float MinAreaForStats = 5.0f;
    public float MinAreaForComplete = 15.0f;
    public float MinHorizAreaForComplete = 2.0f;
    public float MinWallAreaForComplete = 10.0f;

    private uint trackedHandsCount = 0;

    private bool _triggered = false;

    private bool drawSpacialMaping = true;

    public TargetDetection targetDetection;

    bool clickDetected = false;

    public bool HideText = false;


    private string _spaceQueryDescription;

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


    private void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    // Update is called once per frame
    public ObjectPlacer Placer;

    private void Update()
    {
        if (targetDetection.isTargetDetected || clickDetected) { 
            if (DoesScanMeetMinBarForCompletion &&
                (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
                !SpatialUnderstanding.Instance.ScanStatsReportStillWorking)
            {
                SpatialUnderstanding.Instance.RequestFinishScan();
            }
         }
        
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