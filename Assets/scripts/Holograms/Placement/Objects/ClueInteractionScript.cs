using UnityEngine;


public class ClueInteractionScript : MonoBehaviour
{
    private int id=0;

    private void Start()
    {
       
    }

    private Color startColor;

    [SerializeField]
    private float distanceToActivate = 1.5f;

    void OnGazeEnter()
    {
        id = gameObject.GetComponent<ID>().id;
        var com = gameObject.transform.GetComponentInParent<Renderer>();
        /*on memorise l'ancienne valeur*/
        startColor = com.material.color;

        Debug.Log(distanceToCamera());
        if (distanceToCamera() < distanceToActivate)
        {
            ObjectCollectionManager.Instance.ActiveObject = id;
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        else com.material.color = Color.yellow;
    }

    void OnGazeExit()
    {
        var com = gameObject.transform.GetComponentInParent<Renderer>();
        com.material.color = startColor;
    }

    /*distance entre la camera et l'objet selectionné*/
    float distanceToCamera()
    {
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.parent.gameObject.transform.position.x-Camera.main.transform.position.x,2)+ Mathf.Pow(gameObject.transform.parent.gameObject.transform.position.z - Camera.main.transform.position.z, 2)) ;
    }

}