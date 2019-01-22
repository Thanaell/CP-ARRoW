using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInteractionScript : MonoBehaviour {
    
    
    private int clueIdToActivate;

    [SerializeField]
    private float distanceToActivate=2;

    [SerializeField]
    private Config myConfig;
    
    private GameObject rewardObject;
    
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
        var com = gameObject.transform.GetComponentInParent<Renderer>(); 
        startColor = com.material.color;

        Debug.Log(distanceToCamera());
        if (distanceToCamera() < distanceToActivate)
        {
            
            if (ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate)
            {
                rewardObject = Instantiate(ObjectCollectionManager.Instance.OpenTreasurePrefabs[0], transform.position, transform.rotation) as GameObject;
                if (rewardObject != null) {
                    rewardObject.transform.parent = transform.parent;
                    rewardObject.transform.localScale = transform.localScale*0.2f;
                    gameObject.transform.parent.GetComponent<MeshFilter>().mesh.Clear();
                    SendMessage("OpenTreasure", SendMessageOptions.DontRequireReceiver);
                }

            }
       
            else com.material.color = Color.red;
        }
        else com.material.color = Color.yellow;
    }
    

    void OnGazeExit()
    {
        var com = gameObject.transform.GetComponentInParent<Renderer>();
        com.material.color = startColor;
    }

    float distanceToCamera()
    {
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.parent.position.x - Camera.main.transform.position.x, 2) + Mathf.Pow(gameObject.transform.parent.position.z - Camera.main.transform.position.z, 2));
    }

}
