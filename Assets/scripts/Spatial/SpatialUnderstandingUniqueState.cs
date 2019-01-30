using System;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

/*
 * SpatialUnderstandingUniqueState est un script permettant d'appeler la création du "monde" lorsque toutes les conditions nécessaires sont 
 * respectées.  (La création du "monde" se fait dans le script ObjectPlacer)
 */


public class SpatialUnderstandingUniqueState : Singleton<SpatialUnderstandingUniqueState>, IInputClickHandler, ISourceStateHandler
{
    [Tooltip("Minimum area needed to show the numbers needed to finish the scan")]
    [SerializeField]
    private float minAreaForStats = .5f;

    public float MinAreaForStats
    {
        get
        {
            return minAreaForStats;
        }
    }

    [Tooltip("Minimum area needed to finish the scan")]
    [SerializeField]
    private float minAreaForComplete = 5.0f;
    public float MinAreaForComplete
    {
        get
        {
            return minAreaForComplete;
        }
    }

    [Tooltip("Minimum horizontal area (floor) needed to finish the scan")]
    [SerializeField]
    private float minHorizAreaForComplete = 1.0f;
    public float MinHorizAreaForComplete
    {
        get
        {
            return minHorizAreaForComplete;
        }
    }

    [Tooltip("Minimum vertical area (wall) needed to finish the scan")]
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
    [Tooltip("Determines if we can tap in the HoloLens or not in order to finish the scan")]
    public bool clickIsAllowed = true;
    private uint trackedHandsCount = 0;
    bool clickDetected = false;

    private bool _triggered = false;

    [Tooltip("Determines if we show the Spatial Mapping in the HoloLens or not")]
    [SerializeField]
    private bool drawSpatialMapping = false;

    /*
     * detection de la cible vuforia pour créer la scéne
     */
    [Tooltip("Vuforia ImageTarget used to finish the scan when detected")]
    [SerializeField]
    private TargetDetection targetDetection;

    [Tooltip("Script which is used when whe finish the scan in order to create the virtual world")]
    public ObjectPlacer Placer;


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

    /*initialisation des variables fournies dans le fichier config*/
    private void Start()
    {
        if (Config.Instance.FetchDoubleFromConfig("minAreaForComplete"))
        {
            minAreaForComplete = (float)Config.Instance.GetDouble("minAreaForComplete");
        }

        if (Config.Instance.FetchDoubleFromConfig("minHorizontalAreaForComplete"))
        {
            minHorizAreaForComplete = (float)Config.Instance.GetDouble("minHorizontalAreaForComplete");
        }

        if (Config.Instance.FetchDoubleFromConfig("minVerticalAreaForComplete"))
        {
            minWallAreaForComplete = (float)Config.Instance.GetDouble("minVerticalAreaForComplete");
        }
       
        if (Config.Instance.FetchBoolFromConfig("clickIsAllowed"))
        {
            clickIsAllowed = Config.Instance.GetBool("clickIsAllowed");
        }
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

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
            drawSpatialMapping = false;
            _triggered = Placer.CreateScene();
        }
        else
        {
            // hide mesh
            var customMesh = SpatialUnderstanding.Instance.GetComponent<SpatialUnderstandingCustomMesh>();
            customMesh.DrawProcessedMesh = false;
        }

        SpatialMappingManager.Instance.DrawVisualMeshes = drawSpatialMapping;

    }
    

    /*
     * ces trois events est appelée lorsque AirTap est detectée
     * Ceci est utiliséé dans le cas de debug 
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