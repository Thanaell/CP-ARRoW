using System;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

public class SpatialUnderstandingUniqueState : Singleton<SpatialUnderstandingUniqueState>, IInputClickHandler, ISourceStateHandler
{
    [SerializeField]
    private float minAreaForStats = .5f;
    public float MinAreaForStats
    {
        get
        {
            return minAreaForStats;
        }
    }

    [SerializeField]
    private float minAreaForComplete = 5.0f;
    public float MinAreaForComplete
    {
        get
        {
            return minAreaForComplete;
        }
    }

    [SerializeField]
    private float minHorizAreaForComplete = 1.0f;
    public float MinHorizAreaForComplete
    {
        get
        {
            return minHorizAreaForComplete;
        }
    }

    [SerializeField]
    private float minWallAreaForComplete = 8.0f;
    public float MinWallAreaForComplete
    {
        get
        {
            return minWallAreaForComplete;
        }
    }

    /*
     * Dans la scéne de jeu Click n'est pas autorisé
     * Ici le click est utilisé dans le cas de debug et lors de LivePreview car Vuforia n'est pas compatible avec LivePreview
     */ 
    public bool clickIsAllowed = false;
    private uint trackedHandsCount = 0;
    bool clickDetected = false;

    private bool _triggered = false;

    [SerializeField]
    private bool drawSpacialMaping = false;

    /*
     * detection de la cible vuforia pour créer la scéne
     */ 
    [SerializeField]
    private TargetDetection targetDetection;



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
            if ((stats.TotalSurfaceArea > minAreaForComplete) &&
                (stats.HorizSurfaceArea > minHorizAreaForComplete) &&
                (stats.WallSurfaceArea > minWallAreaForComplete) )
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

    public ObjectPlacer Placer;

    private void Update()
    {
        /*
         * lancement de la finalisation du scan de la scéne que si la cible est détectée ou
         * si le click est detecté (dans le cas où le click est autorisé)
         */ 
        if (targetDetection.isTargetDetected || clickDetected) { 
            if (DoesScanMeetMinBarForCompletion &&
                (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
                !SpatialUnderstanding.Instance.ScanStatsReportStillWorking)
            {
                SpatialUnderstanding.Instance.RequestFinishScan();
            }
         }
        
        /*
         * triggered permet de lancer une seule fois la création de la scene
         */ 
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
        if (clickIsAllowed)
        {
            clickDetected = true;
        }
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