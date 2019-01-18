using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInteractionScript : MonoBehaviour {
    
    
    private int clueIdToActivate;

    [SerializeField]
    private float distanceToActivate=2;

    public int ClueIdToActivate
    {
        set
        {
            clueIdToActivate = value;
        }
    }

    private Color startColor;

    void OnGazeEnter()
    {
        var com = gameObject.GetComponent<Renderer>();
        startColor = com.material.color;
        
        if (distanceToCamera() < distanceToActivate)
        {
            if (ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate)
            {
                com.material.color = Color.green;
            }
            else com.material.color = Color.red;
        }
        else com.material.color = Color.yellow;
    }

    void OnGazeExit()
    {
        var com = gameObject.GetComponent<Renderer>();
        com.material.color = startColor;
    }

    float distanceToCamera()
    {
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - Camera.main.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.z - Camera.main.transform.position.z, 2));
    }

}
