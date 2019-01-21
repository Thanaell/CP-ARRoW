﻿using UnityEngine;

public class ClueInteractionScript : MonoBehaviour
{
    private int id;

    public int Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    private Color startColor;

    [SerializeField]
    private float distanceToActivate = 1.5f;

    void OnGazeEnter()
    {
        var com = gameObject.GetComponent<Renderer>();
        /*on memorise l'ancienne valeur*/
        startColor = com.material.color;
        
        if (distanceToCamera() < distanceToActivate)
        {
            ObjectCollectionManager.Instance.ActiveObject = id;
            com.material.color = Color.green;
        }
        else com.material.color = Color.yellow;
    }

    void OnGazeExit()
    {
        var com = gameObject.GetComponent<Renderer>();
        com.material.color = startColor;
    }

    /*distance entre la camera et l'objet selectionné*/
    float distanceToCamera()
    {
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x-Camera.main.transform.position.x,2)+ Mathf.Pow(gameObject.transform.position.z - Camera.main.transform.position.z, 2)) ;
    }

}