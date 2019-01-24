using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInteractionScript : MonoBehaviour {
    
    
    private int clueIdToActivate=0;

    [SerializeField]
    private float distanceToActivate=2;
    

    Renderer[] newRenderer;


    private void Awake()
    {
        newRenderer = GetComponentsInChildren<Renderer>();
    }

    int minDistanceTraveled;

    private void Start()
    {
        if (Config.Instance.FetchIntFromConfig("minDistanceTraveled"))
        {
            minDistanceTraveled = Config.Instance.GetInt("minDistanceTraveled");
        }
        else minDistanceTraveled = 0;
    }

    private Color startColor;

    private bool TreasureActivated=false;

    void OnGazeEnter()
    {

        clueIdToActivate = gameObject.GetComponent<ID>().id;
        var com = gameObject.transform.GetComponentInParent<Renderer>();
        startColor = com.material.color;

        Debug.Log(distanceToCamera());
        if (distanceToCamera() < distanceToActivate)
        {
            if (ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate && WalkedDistance.Instance.getWalkedDistance() > minDistanceTraveled)
            {
                for (int i = 0; i < newRenderer.Length; i++)
                {
                    newRenderer[i].enabled = true;
                }

                gameObject.transform.GetComponentInParent<Renderer>().enabled = false;
                SendMessage("OpenTreasure", SendMessageOptions.DontRequireReceiver);
                gameObject.transform.GetChild(0).localScale = transform.localScale * 0.2f;
                TreasureActivated = true;

                /*
                rewardObject = Instantiate(gameObject.transform.GetChild(0), transform.position, transform.rotation) as GameObject;
                if (rewardObject != null) {
                    rewardObject.transform.parent = transform.parent;
                    rewardObject.transform.localScale = transform.localScale*0.2f;
                    gameObject.transform.parent.GetComponent<MeshFilter>().mesh.Clear();
                    SendMessage("OpenTreasure", SendMessageOptions.DontRequireReceiver);
                }
                */

            }

            else
            {
                com.material.color = Color.red;
                SendMessage("CloseTreasure", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            if (TreasureActivated) SendMessage("OpenTreasure", SendMessageOptions.DontRequireReceiver) ;
            else SendMessage("CloseTreasure", SendMessageOptions.DontRequireReceiver);
            com.material.color = Color.yellow;
        }
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
