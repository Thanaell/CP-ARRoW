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

        ObjectCollectionManager.Instance.activeObject = id;
    }

    void OnGazeExit()
    {
        var com = gameObject.GetComponent<Renderer>();
        com.material.color = startColor;
    }

}