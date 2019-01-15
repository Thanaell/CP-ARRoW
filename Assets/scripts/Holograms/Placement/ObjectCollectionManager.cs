using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public class ObjectCollectionManager : Singleton<ObjectCollectionManager>
{
    

    [Tooltip("A collection of objects prefabs to generate on floor in the world.")]
    public List<GameObject> FloorPrefabs;

    [Tooltip("A collection of objects prefabs to generate on wall in the world.")]
    public List<GameObject> WallPrefabs;

    [Tooltip("The desired size of floor objects in the world.")]
    public Vector3 FloorObjectSize = new Vector3(.25f, .25f, .25f);

    [Tooltip("The desired size of wall objects in the world.")]
    public Vector3 WallObjectSize = new Vector3(.25f, .25f, .25f);

    [Tooltip("Will be calculated at runtime if is not preset.")]
    public float ScaleFactor;

    public List<GameObject> ActiveHolograms = new List<GameObject>();
    public List<ObjectProprieties> HologramsToCreate = new List<ObjectProprieties>();
    private int id = 0;

    public CustomObjectScript customObject;
    
    private int activeObject = 0;

    public void CreateFloorObjects(int number, Vector3 positionCenter, Quaternion rotation)
    {
        CreateObject(FloorPrefabs[number], positionCenter, rotation, FloorObjectSize);
    }
    public void CreateWallObjects(int number, Vector3 positionCenter, Quaternion rotation)
    {
        CreateObject(WallPrefabs[number], positionCenter, rotation, WallObjectSize);
    }

    private void CreateObject(GameObject objectToCreate, Vector3 positionCenter, Quaternion rotation, Vector3 desiredSize)
    {
        
        GameObject newObject = Instantiate(objectToCreate, positionCenter, rotation) as GameObject;

        if (newObject != null)
        {
            // Set the parent of the new object the GameObject it was placed on
            newObject.transform.parent = gameObject.transform;

            newObject.transform.localScale = RescaleToSameScaleFactor(objectToCreate);

            id++;
            customObject.addComponents(newObject,id);

            ActiveHolograms.Add(newObject);
            
        }
    }


    private Vector3 RescaleToSameScaleFactor(GameObject objectToScale)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (ScaleFactor == 0f)
        {
            CalculateScaleFactor();
        }

        return objectToScale.transform.localScale * ScaleFactor;
    }

    private Vector3 StretchToFit(GameObject obj, Vector3 desiredSize)
    {
        var curBounds = GetBoundsForAllChildren(obj).size;

        return new Vector3(desiredSize.x / curBounds.x / 2, desiredSize.y, desiredSize.z / curBounds.z / 2);
    }

    private void CalculateScaleFactor()
    {
        float maxScale = float.MaxValue;

        var ratio = CalcScaleFactorHelper(FloorPrefabs, FloorObjectSize);
        if (ratio < maxScale & ratio>0)
        {
            maxScale = ratio;
        }
        ratio = CalcScaleFactorHelper(WallPrefabs, WallObjectSize);
        if (ratio < maxScale & ratio >0)
        {
            maxScale = ratio;
        }

        ScaleFactor = maxScale;
    }

    private float CalcScaleFactorHelper(List<GameObject> objects, Vector3 desiredSize)
    {
        float maxScale = float.MaxValue;

        foreach (var obj in objects)
        {
            var curBounds = GetBoundsForAllChildren(obj).size;
            var difference = curBounds - desiredSize;

            float ratio;

            if (difference.x > difference.y && difference.x > difference.z)
            {
                ratio = desiredSize.x / curBounds.x;
            }
            else if (difference.y > difference.x && difference.y > difference.z)
            {
                ratio = desiredSize.y / curBounds.y;
            }
            else
            {
                ratio = desiredSize.z / curBounds.z;
            }

            if (ratio < maxScale)
            {
                maxScale = ratio;
            }
        }

        return maxScale;
    }

    private Bounds GetBoundsForAllChildren(GameObject findMyBounds)
    {
        Bounds result = new Bounds(Vector3.zero, Vector3.zero);

        foreach (var curRenderer in findMyBounds.GetComponentsInChildren<Renderer>())
        {
            if (result.extents == Vector3.zero)
            {
                result = curRenderer.bounds;
            }
            else
            {
                result.Encapsulate(curRenderer.bounds);
            }
        }

        return result;
    }
}
