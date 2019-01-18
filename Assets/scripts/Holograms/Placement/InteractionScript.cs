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

        Debug.Log(id);
        Debug.Log(id);
        Debug.Log(id);
        Debug.Log(id);
        if (distanceToCamera() < 1.5)
        {
            ObjectCollectionManager.Instance.activeObject = id;
            com.material.color = Color.green;
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
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x-Camera.main.transform.position.x,2)+ Mathf.Pow(gameObject.transform.position.z - Camera.main.transform.position.z, 2)) ;
    }

}