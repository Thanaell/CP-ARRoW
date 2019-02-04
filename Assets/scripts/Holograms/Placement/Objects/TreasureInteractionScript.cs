using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TreasureInteractionScript est un script qui gère les interactions de tresor avec le regard d'utilisateur.
 * Ces fonctions sont appelées par GazeInteractionManager (le message est retransmis par MessageListener)
 */

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

    /*distance minimale à faire avant de debloquer le coffre*/
    int minDistanceTraveled;
    

    private void Start()
    {
        if (Config.Instance.FetchIntFromConfig("minDistanceTraveled"))
        {
            minDistanceTraveled = Config.Instance.GetInt("minDistanceTraveled");
        }
        else minDistanceTraveled = 0;
        if (Config.Instance.FetchIntFromConfig("distanceToActivateTreasure"))
        {
            distanceToActivate = Config.Instance.GetInt("distanceToActivateTreasure");
        }
        else distanceToActivate = 2;
        isOnGaze = false;
        timeLeft = 1;


        clueIdToActivate = gameObject.GetComponent<ID>().id;
    }
    

    private bool TreasureActivated=false;

    private bool autobusActivated = false;

    bool isOnGaze = false;

    void Update()
    {
        /*Si on a collecté toutes les clés et nous n'avons pas encore ouvert le coffre*/
        if(ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate & !autobusActivated)
        {
            SendMessage("CluesAreCollected", SendMessageOptions.DontRequireReceiver);
        }

        /*
         * si le trésor est activé (vu par l'utilisateur après avoir collecté toutes les clés)
         * cette boucle permet d'attendre un certain temps avant d'afficher l'autobus caché dans le coffre
         */
        if (TreasureActivated && !autobusActivated)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                for (int i = 0; i < newRenderer.Length; i++)
                {
                    newRenderer[i].enabled = true;
                }

                gameObject.transform.GetComponentInParent<Renderer>().enabled = false;
                gameObject.transform.GetChild(0).localScale = transform.localScale * 0.18f;
                gameObject.transform.GetChild(0).localPosition -= new Vector3(0, 0.3f, 0);
                SendMessage("ShowAutobus", SendMessageOptions.DontRequireReceiver);
                autobusActivated = true;
            }
        }
        if (isOnGaze)
        {
            if (!TreasureActivated)
            {
                if (distanceToCamera() < distanceToActivate)
                {
                    /*
                     * interaction avec le coffre : ouverture avec l'effet des particules
                     */ 
                    if (ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate && WalkedDistance.Instance.getWalkedDistance() > minDistanceTraveled)
                    {
                        TreasureActivated = true;
                        SendMessage("OpenTreasure", SendMessageOptions.DontRequireReceiver);
                        gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }
    }

    void OnGazeEnter()
    {
        ObjectCollectionManager.Instance.TreasureIsSeen = true;
        isOnGaze = true;
        if (TreasureActivated && timeLeft < 0)
        {
            SendMessage("OpenTreasure", SendMessageOptions.DontRequireReceiver);
        }
        else SendMessage("CloseTreasure", SendMessageOptions.DontRequireReceiver);
    }
    

    void OnGazeExit()
    {
        isOnGaze = false;
    }

    float distanceToCamera()
    {
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.parent.position.x - Camera.main.transform.position.x, 2) + Mathf.Pow(gameObject.transform.parent.position.z - Camera.main.transform.position.z, 2));
    }

}
