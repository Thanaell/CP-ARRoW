using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProprieties : MonoBehaviour {
    
    public ObjectProprieties(
        GameObject objectToCreate,
        Vector3 positionCenter,
        Quaternion rotation,
        int id)
    {
        ObjectToCreate = ObjectToCreate;
        PositionCenter = positionCenter;
        Rotation = rotation;
        Id = id;
    }

    public readonly GameObject ObjectToCreate;
    public readonly Vector3 PositionCenter;
    public readonly Quaternion Rotation;
    public readonly int Id;
}
