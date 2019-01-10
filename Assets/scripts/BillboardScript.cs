using System;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;

public class BillboardScript : Singleton<BillboardScript>
{
    public TextMesh DebugDisplay;
    public TextMesh DebugSubDisplay;
        
    public bool HideText = false;

    private bool ready = false;

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
                        if (SpatialUnderstandingUniqueState.Instance.DoesScanMeetMinBarForCompletion)
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
            ready = SpatialUnderstandingUniqueState.Instance.DoesScanMeetMinBarForCompletion;
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
                if (stats.TotalSurfaceArea > SpatialUnderstandingUniqueState.Instance.MinAreaForStats)
                {
                    SpatialMappingManager.Instance.DrawVisualMeshes = false;


                    string subDisplayText = string.Format("totalArea : {0:0.0} / {1},  ", stats.TotalSurfaceArea, SpatialUnderstandingUniqueState.Instance.MinAreaForComplete);
                    subDisplayText += string.Format("horizArea : {0:0.0} / {1},  ", stats.HorizSurfaceArea, SpatialUnderstandingUniqueState.Instance.MinHorizAreaForComplete);
                    subDisplayText += string.Format("wallArea : {0:0.0} / {1}", stats.WallSurfaceArea, SpatialUnderstandingUniqueState.Instance.MinWallAreaForComplete);

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
    }
    

    private void Update()
    {
        // Updates
        Update_DebugDisplay();

    }


}