using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public class ObjectCollectionManager : Singleton<ObjectCollectionManager>
{
    private int cluePrefabIndex;

    private int treasurePrefabIndex;

    private int clueNumber;

    [Tooltip("A collection of objects prefabs to generate on floor in the world.")]
    public List<GameObject> FloorPrefabs;

    [Tooltip("A collection of objects prefabs to generate on wall in the world.")]
    public List<GameObject> WallPrefabs;

    [Tooltip("A collection of meshs to use to generate objects on wall in the world")]
    public List<GameObject> Meshs;

    [Tooltip("The desired size of floor objects in the world.")]
    public Vector3 FloorObjectSize = new Vector3(.5f, .5f, .5f);

    [Tooltip("The desired size of wall objects in the world.")]
    public Vector3 WallObjectSize = new Vector3(.25f, .25f, .25f);

    [Tooltip("Will be calculated at runtime if is not preset.")]
    public float WallScaleFactor;

    [Tooltip("Will be calculated at runtime if is not preset.")]
    public float FloorScaleFactor;

    private List<GameObject> WallActiveHolograms = new List<GameObject>();
    private List<ObjectProprieties> WallHologramsToCreate = new List<ObjectProprieties>();
    private List<GameObject> FloorActiveHolograms = new List<GameObject>();


    private int idDistributed = 0;

    /*indique id de WallObject activé */
    private int activeObject = 0;

    public int ActiveObject
    {
        get
        {
            return activeObject;
        }
        set
        {
            activeObject = value;
        }
    }

    private bool treasureIsSeen = false;
    public bool TreasureIsSeen
    {
        get
        {
            return treasureIsSeen;
        }
        set
        {
            treasureIsSeen = value;
        }
    }

    void Start()
    {
        if (Config.Instance.FetchIntFromConfig("cluePrefabIndex"))
        {
            cluePrefabIndex = Config.Instance.GetInt("cluePrefabIndex");
        }

        if (Config.Instance.FetchIntFromConfig("treasurePrefabIndex"))
        {
            treasurePrefabIndex = Config.Instance.GetInt("treasurePrefabIndex");
        }

        if (Config.Instance.FetchDoubleFromConfig("wallScaleFactor"))
        {
            WallScaleFactor = (float)Config.Instance.GetDouble("wallScaleFactor");
        }

        if (Config.Instance.FetchDoubleFromConfig("floorScaleFactor"))
        {
            FloorScaleFactor = (float)Config.Instance.GetDouble("floorScaleFactor");
        } 
        if (Config.Instance.FetchIntFromConfig("clueNumberToUnlock"))
        {
            WallPrefabs.Clear();
            clueNumber = Config.Instance.GetInt("clueNumberToUnlock");
            if (clueNumber <= Meshs.Count)
            {
                for (int i = 0; i < clueNumber; i++)
                {
                    WallPrefabs.Add(Meshs[i]);
                }
            }
            else
            {
                for (int i = 0; i < clueNumber; i++)
                {
                    WallPrefabs.Add(Meshs[Random.Range(0, Meshs.Count)]);
                }
            }
        }
    }
    /*
     * differents prefabs possible à integrer sur les objets
     * permet de personnaliser les objets
     */
    public List<GameObject> listPrefabs;

    /*
     * les objets du sol sont créés imediatement
     */
    public void CreateFloorObjects(int number, Vector3 positionCenter, Quaternion rotation)
    {
        CreateObject(FloorPrefabs[number], positionCenter, rotation, FloorObjectSize, idDistributed, ObjectType.FloorObject);
    }

    /*
     * les objets sur le mur sont crées au fur à mesure
     */
    public void CreateWallObjects(int number, Vector3 positionCenter, Quaternion rotation)
    {
        idDistributed++;
        WallHologramsToCreate.Add(new ObjectProprieties(ObjectType.WallObject, number, positionCenter, rotation, idDistributed));
    }

    /*création de l'objet*/
    private void CreateObject(GameObject objectToCreate, Vector3 positionCenter, Quaternion rotation, Vector3 desiredSize,int objectId,ObjectType type)
    {

        GameObject newObject = Instantiate(objectToCreate, positionCenter, rotation) as GameObject;

        if (newObject != null)
        {
           if (type == ObjectType.WallObject)
           {
                newObject.name = "WallObject" + objectId;
                GameObject prefab = Instantiate(listPrefabs[cluePrefabIndex], positionCenter, rotation, newObject.transform) as GameObject;
                if (prefab != null)
                {
                    // Set the parent of the new object the GameObject it was placed on
                    newObject.transform.parent = gameObject.transform;

                    prefab.transform.SetSiblingIndex(0);

                    RescaleToSameScaleFactor();

                    newObject.transform.localScale = new Vector3(newObject.transform.localScale.x * WallObjectSize.x, newObject.transform.localScale.y * WallObjectSize.y, newObject.transform.localScale.z * WallObjectSize.z) * WallScaleFactor / WallObjectSize.y;

                    prefab.AddComponent<ID>().id = objectId;
                    newObject.AddComponent<MessageListener>();

                    WallActiveHolograms.Add(newObject);
                }
            }
           if (type == ObjectType.FloorObject)
           {
                newObject.name = "FloorObject" + objectId;
                GameObject prefab = Instantiate(listPrefabs[treasurePrefabIndex], positionCenter, rotation, newObject.transform) as GameObject;
                if (prefab != null)
                {
                    // Set the parent of the new object the GameObject it was placed on
                    newObject.transform.parent = gameObject.transform;

                    prefab.transform.SetSiblingIndex(0);

                    RescaleToSameScaleFactor();

                    newObject.transform.localScale = new Vector3(newObject.transform.localScale.x * FloorObjectSize.x, newObject.transform.localScale.y * FloorObjectSize.y, newObject.transform.localScale.z * FloorObjectSize.z) * FloorScaleFactor / FloorObjectSize.y;

                    prefab.AddComponent<ID>().id = objectId;
                    newObject.AddComponent<MessageListener>();

                    FloorActiveHolograms.Add(newObject);
                }
           }



        }


    }

    //TODO : commenter
    private void Update()
    {
        if (idDistributed > 0 & treasureIsSeen)
        {
            if (WallActiveHolograms.Count <= activeObject & activeObject<idDistributed)
            {
                if (WallHologramsToCreate[activeObject].Type == ObjectType.WallObject)
                {
                    CreateObject(WallPrefabs[WallHologramsToCreate[activeObject].PositionInList], WallHologramsToCreate[activeObject].PositionCenter,
                        WallHologramsToCreate[activeObject].Rotation, WallObjectSize, WallHologramsToCreate[activeObject].ObjectId, ObjectType.WallObject);
                }
            }
        }
    }





    private void RescaleToSameScaleFactor()
    {
        float maxScale = float.MaxValue;

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (WallScaleFactor == 0f )
        {
            var ratio = CalcScaleFactorHelper(WallPrefabs, WallObjectSize);
            if (ratio < maxScale & ratio > 0)
            {
                maxScale = ratio;
            }

            WallScaleFactor = maxScale;
        }
        maxScale = float.MaxValue;
        if ( FloorScaleFactor == 0f)
        {
            var ratio = CalcScaleFactorHelper(FloorPrefabs, FloorObjectSize);
            if (ratio < maxScale & ratio > 0)
            {
                maxScale = ratio;
            }

            FloorScaleFactor = maxScale;
        }

        return ;
    }

    private float CalcScaleFactorHelper(List<GameObject> objects, Vector3 desiredSize)
    {
        float maxScale = float.MaxValue;

        foreach (var obj in objects)
        {
            var curBounds = GetBoundsForAllChildren(obj).size;
            float ratio;
            ratio = desiredSize.y / curBounds.y;
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
