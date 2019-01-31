using UnityEngine;


public class ClueInteractionScript : MonoBehaviour
{
    private int id=0;

    Renderer com;
    bool isOnGaze = false;
    float timeLeft;
    bool objectActivated;
    private void Start()
    {

        id = gameObject.GetComponent<ID>().id;
        com = gameObject.transform.GetComponentInParent<Renderer>();
        isOnGaze = false;
        objectActivated = false;
        timeLeft = 0.5f;

        /*on memorise l'ancienne valeur*/
        startColor = com.material.color;
    }

    private Color startColor;

    [SerializeField]
    private float distanceToActivate = 1.5f;


    private void Update()
    {
        if (objectActivated)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
        if (isOnGaze && !objectActivated)
        {
            if (distanceToCamera() < distanceToActivate)
            {
                SendMessage("ClueCollected", SendMessageOptions.DontRequireReceiver);
                ObjectCollectionManager.Instance.ActiveObject = id;
                gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                objectActivated = true;
            }
            else
            {
                com.material.color = 0.6f * startColor;
                SendMessage("ClueSeen", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnGazeEnter()
    {
        isOnGaze = true;
    }

    void OnGazeExit()
    {
        com.material.color = startColor;
        isOnGaze = false;
    }

    /*distance entre la camera et l'objet selectionné*/
    float distanceToCamera()
    {
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.parent.gameObject.transform.position.x-Camera.main.transform.position.x,2)+ Mathf.Pow(gameObject.transform.parent.gameObject.transform.position.z - Camera.main.transform.position.z, 2)) ;
    }

}