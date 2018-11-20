﻿using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public class ObjectCollectionManager : Singleton<ObjectCollectionManager>
{

    [Tooltip("A collection of Wide building prefabs to generate in the world.")]
    public List<GameObject> CubePrefabs;

    [Tooltip("The desired size of wide buildings in the world.")]
    public Vector3 SquareSize = new Vector3(.5f, .5f, .5f);

    [Tooltip("The ground to place under buildings.")]
    public GameObject BuildingGround;

    [Tooltip("Will be calculated at runtime if is not preset.")]
    public float ScaleFactor;

    public List<GameObject> ActiveHolograms = new List<GameObject>();

    public void CreateSquare(int number, Vector3 positionCenter, Quaternion rotation)
    {
        CreateBuilding(CubePrefabs[number], positionCenter, rotation, SquareSize);
    }

    private void CreateBuilding(GameObject buildingToCreate, Vector3 positionCenter, Quaternion rotation, Vector3 desiredSize)
    {
        // Stay center in the square but move down to the ground
        var position = positionCenter ; //new Vector3(0, desiredSize.y * .5f, 0);

        GameObject newObject = Instantiate(buildingToCreate, position, rotation) as GameObject;

        if (newObject != null)
        {
            // Set the parent of the new object the GameObject it was placed on
            newObject.transform.parent = gameObject.transform;

            newObject.transform.localScale = RescaleToSameScaleFactor(buildingToCreate);
            ActiveHolograms.Add(newObject);
        }
    }

    public void CreateGround(GameObject groundToCreate, Vector3 positionCenter, Quaternion rotation, Vector3 size)
    {
        // Stay center in the square but move down to the ground
        var position = positionCenter - new Vector3(0, size.y * .5f, 0);

        GameObject newObject = Instantiate(groundToCreate, position, rotation) as GameObject;

        if (newObject != null)
        {
            // Set the parent of the new object the GameObject it was placed on
            newObject.transform.parent = gameObject.transform;

            newObject.transform.localScale = StretchToFit(groundToCreate, size);
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

        var ratio = CalcScaleFactorHelper(CubePrefabs, SquareSize);
        if (ratio < maxScale)
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
