﻿using System;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

/*
 * ObjectPlacer détermine les position des objets tout en respectant les contraintes grâce au SpatialUnderstanding.
 * Dans notre démo nous l'utilisons pour placer les objets à des endoits accessibles aux utilisateurs. 
 */ 

public class ObjectPlacer : MonoBehaviour
{
    /**
     * Comme son nom indique ce sont des boîtes de debug. L'objet est alignés en y avec les DebugBoxes.
     * Et ce sont ces boîtes là qui sont en contact direct avec les autres objets (murs/sol).
     * Si l'objet semble d'être mal placé, passez DrawDebugBoxes en true.
     **/ 
     [SerializeField]
    private bool DrawDebugBoxes = false;

    /**
     * Permet de dessiner les halogrammes. Sa valeur est toujeour TRUE 
     * sauf si on veut travailler qu'avec les DebugBoxes pour les positions précis
     **/ 
     [SerializeField]
    private bool DrawObjects = true;

 
    /**
     * Recuperation de spatialUnderstanding pour placer les objets sur la mesh
     **/
    [SerializeField]
    private SpatialUnderstandingCustomMesh SpatialUnderstandingMesh;

    /**
     * List de lignes tracant les boîtes de debug
     **/
    private readonly List<BoxDrawer.Box> _lineBoxList = new List<BoxDrawer.Box>();

    private BoxDrawer _boxDrawing;

    /**
     * List des contraintes pour chaque objet nécessaire pour placer correctement des objets
     **/
    private readonly Queue<PlacementResult> _results = new Queue<PlacementResult>();

    /**
     * Quand on lance le tracé des objets, on désactive l'affichage de Mesh
     **/ 
    private bool _timeToHideMesh;

    /*
     * permet de cacher l'aperçu des objets derriere des objets du monde réel
     * */
    public Material OccludedMaterial;

    /*
     * distance minimale entre objets de la scene 
     * et distance minimale entre la position de depart de la camera et chaque objet
     */ 
     [SerializeField]
    private float distanceFromOtherObject=0.5f;



    // Use this for initialization

    void Start()
    {
        if (DrawDebugBoxes)
        {
            /**
             * Tracé des boîtes de debug
             **/ 
            _boxDrawing = new BoxDrawer(gameObject);
        }
        if (Config.Instance.FetchDoubleFromConfig("distanceBetweenObjects"))
        {
            distanceFromOtherObject = (float)Config.Instance.GetDouble("distanceBetweenObjects");
        }
    }

    void Update()
    {
        ProcessPlacementResults();

        if (_timeToHideMesh)
        {
            BillboardScript.Instance.HideText = true;            
            HideGridEnableOcclulsion();

            /*
             * permet de ne pas lancer plusieurs fois la focntion
             */ 
            _timeToHideMesh = false;
        }

        if (DrawDebugBoxes)
        {
            _boxDrawing.UpdateBoxes(_lineBoxList);
        }

    }

    private void HideGridEnableOcclulsion()
    {
        SpatialUnderstandingMesh.MeshMaterial = OccludedMaterial;
    }

    /*
     * Est appelé par SpatialUnderstandingUniqueState
     * Lance la création de la scéne avec ses objets
     */ 
    public bool CreateScene()
    {
       
        // Only if we're enabled
        if (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding )
        {
            return false;
        }

        if (SpatialUnderstandingDllObjectPlacement.Solver_Init() > 0)
        {

            BillboardScript.Instance.SpaceQueryDescription = "Generating World";

            /*Liste des contraintes*/
            List<PlacementQuery> queries = new List<PlacementQuery>();

            if (DrawObjects)
            {
                queries.AddRange(AddObjects());
            }

            GetLocationsFromSolver(queries);
            return true;
        }
        return false;
    }

    /**
     * renvoie la liste des contraintes completée plus loin
     **/ 
    private List<PlacementQuery> AddObjects()
    {

        var queries = CreateLocationQueriesForSolver(ObjectCollectionManager.Instance.WallPrefabs.Count, ObjectCollectionManager.Instance.WallObjectSize, ObjectType.WallObject);
        queries.AddRange(CreateLocationQueriesForSolver(ObjectCollectionManager.Instance.FloorPrefabs.Count, ObjectCollectionManager.Instance.FloorObjectSize, ObjectType.FloorObject));
        return queries;
    }

   

    private int _placedFloorObjects;
    private int _placedWallObjects;

    /*
     * affichage des objets à chaque frame
     */ 
    private void ProcessPlacementResults()
    {
        /*
         * vérification si les objets exists
         */ 
        if (_results.Count > 0)
        {
            var toPlace = _results.Dequeue();
            // Output
            if (DrawDebugBoxes)
            {
                DrawBox(toPlace, Color.red);
            }
            var rotation = Quaternion.LookRotation(toPlace.Normal, Vector3.up);

            switch (toPlace.ObjType)
            {
                case ObjectType.FloorObject:
                    ObjectCollectionManager.Instance.CreateFloorObjects(_placedFloorObjects++, toPlace.Position, rotation);
                    break;
                case ObjectType.WallObject:
                    /*On change la hauteur de la position de l'objet pour le mettre à l'hauteur de l'utilisateur*/
                    ObjectCollectionManager.Instance.CreateWallObjects(_placedWallObjects++, new Vector3(toPlace.Position.x, Camera.main.transform.position.y, toPlace.Position.z), rotation);
                    break;
            }
        }
    }

    /*
     * Draw debug boxes
     */ 
    private void DrawBox(PlacementResult boxLocation, Color color)
    {
        if (boxLocation != null)
        {
            _lineBoxList.Add(
                new BoxDrawer.Box(
                    boxLocation.Position,
                    Quaternion.LookRotation(boxLocation.Normal, Vector3.up),
                    color,
                    boxLocation.Dimensions * 0.5f)
            );
        }
    }

    private void GetLocationsFromSolver(List<PlacementQuery> placementQueries)
    {
#if UNITY_WSA && !UNITY_EDITOR
        System.Threading.Tasks.Task.Run(() =>
        {
            // Go through the queries in the list
            for (int i = 0; i < placementQueries.Count; ++i)
            {
                var result = PlaceObject(placementQueries[i].ObjType.ToString() + i,
                                         placementQueries[i].PlacementDefinition,
                                         placementQueries[i].Dimensions,
                                         placementQueries[i].ObjType,
                                         placementQueries[i].PlacementRules,
                                         placementQueries[i].PlacementConstraints);
                if (result != null)
                {
                    _results.Enqueue(result);
                }
            }

            _timeToHideMesh = true;
        });
#else
        _timeToHideMesh = true;
#endif
    }



    private PlacementResult PlaceObject(string placementName,
        SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition,
        Vector3 boxFullDims,
        ObjectType objType,
        List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> placementRules = null,
        List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> placementConstraints = null)
    {

        // New query
        if (SpatialUnderstandingDllObjectPlacement.Solver_PlaceObject(
                placementName,
                SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementDefinition),
                (placementRules != null) ? placementRules.Count : 0,
                ((placementRules != null) && (placementRules.Count > 0)) ? SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementRules.ToArray()) : IntPtr.Zero,
                (placementConstraints != null) ? placementConstraints.Count : 0,
                ((placementConstraints != null) && (placementConstraints.Count > 0)) ? SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementConstraints.ToArray()) : IntPtr.Zero,
                SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticObjectPlacementResultPtr()) > 0)
        {
            SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult placementResult = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticObjectPlacementResult();

            return new PlacementResult(placementResult.Clone() as SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult, boxFullDims, objType);
        }

        return null;
    }


    /*
     * création des contraintes pour les objets
     */ 
    private List<PlacementQuery> CreateLocationQueriesForSolver(int desiredLocationCount, Vector3 boxFullDims, ObjectType objType)
    {
        List<PlacementQuery> placementQueries = new List<PlacementQuery>();

        var halfBoxDims = boxFullDims * .5f;


        for (int i = 0; i < desiredLocationCount; ++i)
        {
            var placementConstraints = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint>();
            SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition = SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnWall(halfBoxDims, Camera.main.transform.position.y - 2*halfBoxDims.y, Camera.main.transform.position.y +2* halfBoxDims.y);//Create_OnFloor(halfBoxDims);

            /**
           * Les regles de placement d'objet. La regle definit que l'objet doit être éloigné d'un autre objet ou point ou autre...
           * Variable à exploiter.
           **/
            var placementRules = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>
            {
                SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(distanceFromOtherObject),
                SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromPosition(Camera.main.transform.position, distanceFromOtherObject)
            };

            switch (objType)
            {
                case ObjectType.FloorObject:
                    /**
                    * Les contraintes de placement des objets. On peut placer un objet près d'un centre ou près d'un point à particulier.
                    * Cette variable est aussi à exploiter par rapport ce qu'on veux voir
                     **/
                    // var placementConstraints = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint>(); //->par défaut
                    placementConstraints = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_NearPoint(Camera.main.transform.position - 2 * Camera.main.transform.forward),
                        //SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_AwayFromPoint(2*Camera.main.transform.position)
                    };

                    /**
                     * Ici nous définissons où veut on placer les objets.
                     * Nous utilisons pour le moment que deux création d'objet : Create_OnFloor et Create_OnWall.
                     * On peut exploirer plus cette variable pour mieux définir la position des objets.
                     **/
                    placementDefinition = SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloor(halfBoxDims);

                    break;
                case ObjectType.WallObject:
                    /**
                   * Les contraintes de placement des objets. On peut placer un objet près d'un centre ou près d'un point à particulier.
                   * Cette variable est aussi à exploiter par rapport ce qu'on veux voir
                    **/
                    // var placementConstraints = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint>(); //->par défaut
                    placementConstraints = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_NearCenter(),
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_AwayFromPoint(Camera.main.transform.position)
                    };

                    /**
                     * Ici nous définissons où veut on placer les objets.
                     * Nous utilisons pour le moment que deux création d'objet : Create_OnFloor et Create_OnWall.
                     * On peut exploirer plus cette variable pour mieux définir la position des objets.
                     **/
                    placementDefinition = SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnWall(halfBoxDims, 1.6f, 1.8f);//Create_OnFloor(halfBoxDims);

                    break;
            }
          


            
            placementQueries.Add(
                new PlacementQuery(placementDefinition,
                    boxFullDims,
                    objType,
                    placementRules,
                    placementConstraints
                ));
        }

        return placementQueries;
    }

}