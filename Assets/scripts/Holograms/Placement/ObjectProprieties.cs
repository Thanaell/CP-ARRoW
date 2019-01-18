using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProprieties : MonoBehaviour {
    
    public ObjectProprieties(
        ObjectType type,
        int positionInList,
        Vector3 positionCenter,
        Quaternion rotation,
        int objectId)
    {
        Type=type;
        PositionInList = positionInList;
        PositionCenter = positionCenter;
        Rotation = rotation;
        ObjectId = objectId;
    }

    public readonly ObjectType Type;
    public readonly int PositionInList;
    public readonly Vector3 PositionCenter;
    public readonly Quaternion Rotation;

    /*
     * Pour WallObject c'est id propre à l'objet
     * Pour FloorObject c'est l'objet de ce id qui active le trésor
     */ 
    public readonly int ObjectId;
}
