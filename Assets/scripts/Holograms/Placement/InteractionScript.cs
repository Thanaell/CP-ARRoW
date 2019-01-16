using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    Vector3 originalPosition;

    public int id;

    // Use this for initialization
    void Start()
    {
        // Grab the original local position of the sphere when the app starts.
        originalPosition = this.transform.localPosition;
    }


    private Color startColor;

    void OnGazeEnter()
    {
        var com = gameObject.GetComponent<Renderer>();
        startColor = com.material.color;
        if (id == 1)
        {
            com.material.color = Color.green;
        }
        if (id == 2)
        {
            com.material.color = Color.black;
        }
        else com.material.color = Color.yellow;

        if (distanceToCamera() < 2)
        {
            ObjectCollectionManager.Instance.activeObject = id;
        }
    }

    void OnGazeExit()
    {
        var com = gameObject.GetComponent<Renderer>();
        com.material.color = startColor;
    }

    float distanceToCamera()
    {
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x-Camera.main.transform.position.x,2)+ Mathf.Pow(gameObject.transform.position.y - Camera.main.transform.position.y, 2)) ;
    }

}