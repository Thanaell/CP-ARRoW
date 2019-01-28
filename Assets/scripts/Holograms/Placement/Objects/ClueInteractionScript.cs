using UnityEngine;


public class ClueInteractionScript : MonoBehaviour
{
    private int id=0;

    Renderer com;
    bool isOnGaze = false;
    private void Start()
    {

        id = gameObject.GetComponent<ID>().id;
        com = gameObject.transform.GetComponentInParent<Renderer>();
        isOnGaze = false;

        /*on memorise l'ancienne valeur*/
        startColor = com.material.color;
    }

    private Color startColor;

    [SerializeField]
    private float distanceToActivate = 1.5f;


    private void Update()
    {
        if (isOnGaze)
        {

            if (distanceToCamera() < distanceToActivate)
            {
                ObjectCollectionManager.Instance.ActiveObject = id;
                gameObject.transform.parent.gameObject.SetActive(false);
            }
            else com.material.color = Color.yellow;
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