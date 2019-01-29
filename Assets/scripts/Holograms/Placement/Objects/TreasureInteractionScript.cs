using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInteractionScript : MonoBehaviour {
    
    
    private int clueIdToActivate=0;

    [SerializeField]
    private float distanceToActivate=2;
    float timeLeft;

    Renderer[] newRenderer;


    private void Awake()
    {
        newRenderer = GetComponentsInChildren<Renderer>();
    }

    int minDistanceTraveled;

    Renderer com;

    private void Start()
    {
        if (Config.Instance.FetchIntFromConfig("minDistanceTraveled"))
        {
            minDistanceTraveled = Config.Instance.GetInt("minDistanceTraveled");
        }
        else minDistanceTraveled = 0;
        isOnGaze = false;
        timeLeft = 1;


        clueIdToActivate = gameObject.GetComponent<ID>().id;
        com = gameObject.transform.GetComponentInParent<Renderer>();
        startColor = com.material.color;
    }

    private Color startColor;

    private bool TreasureActivated=false;

    bool isOnGaze = false;

    void Update()
    {
        if (TreasureActivated)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                for (int i = 0; i < newRenderer.Length; i++)
                {
                    newRenderer[i].enabled = true;
                }

                gameObject.transform.GetComponentInParent<Renderer>().enabled = false;
                gameObject.transform.GetChild(0).localScale = transform.localScale * 0.15f;
                SendMessage("OpenTreasure", SendMessageOptions.DontRequireReceiver);
            }
        }
        if (isOnGaze)
        {
            if (!TreasureActivated)
            {
                if (distanceToCamera() < distanceToActivate)
                {
                    if (ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate && WalkedDistance.Instance.getWalkedDistance() > minDistanceTraveled)
                    {
                        TreasureActivated = true;
                        gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                    }
                    else
                    {
                        com.material.color = Color.red;
                    }
                }
                else
                {
                    com.material.color = Color.yellow;
                }
            }
        }
    }

    void OnGazeEnter()
    {
        ObjectCollectionManager.Instance.TreasureIsSeen = true;
        isOnGaze = true;
        if (TreasureActivated) SendMessage("OpenTreasure", SendMessageOptions.DontRequireReceiver);
        else SendMessage("CloseTreasure", SendMessageOptions.DontRequireReceiver);
    }
    

    void OnGazeExit()
    {
        isOnGaze = false;
        //var com = gameObject.transform.GetComponentInParent<Renderer>();
        com.material.color = startColor;
    }

    float distanceToCamera()
    {
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.parent.position.x - Camera.main.transform.position.x, 2) + Mathf.Pow(gameObject.transform.parent.position.z - Camera.main.transform.position.z, 2));
    }

}
