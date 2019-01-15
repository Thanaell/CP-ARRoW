using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProprieties : MonoBehaviour {
    
    public ObjectProprieties(
        ObjectType type,
        int numberInList,
        Vector3 positionCenter,
        Quaternion rotation,
        int objectId)
    {
        Type=type;
        NumberInList = numberInList;
        PositionCenter = positionCenter;
        Rotation = rotation;
        ObjectId = objectId;
    }

    public readonly ObjectType Type;
    public readonly int NumberInList;
    public readonly Vector3 PositionCenter;
    public readonly Quaternion Rotation;
    public readonly int ObjectId;
}
