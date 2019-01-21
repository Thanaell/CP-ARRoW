using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInteractionScript : MonoBehaviour {
    
    
    private int clueIdToActivate;

    [SerializeField]
    private float distanceToActivate=2;

    [SerializeField]
    private Config myConfig;

    private MeshFilter rewardFilter;
    private GameObject newObject;

    private void Start()
    {
        // rewardFilter = ObjectCollectionManager.Instance.OpenTreasurePrefabs[0].GetComponent<MeshFilter>();
      
      /*  myConfig = GameObject.FindGameObjectWithTag("Config").GetComponent<Config>();
        if (myConfig.FetchDoubleFromConfig("distanceToActivateTreasure"))
        {
            distanceToActivate = (float) myConfig.getLastDoubleRead();
        }*/
    }

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
        var com = gameObject.GetComponent<Renderer>();
        startColor = com.material.color;

        if (distanceToCamera() < distanceToActivate)
        {

            Debug.Log("Proche");
            if (ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate)
            {
                newObject = Instantiate(ObjectCollectionManager.Instance.OpenTreasurePrefabs[0], transform.position, transform.rotation) as GameObject;
                if (newObject != null) {
                    newObject.transform.parent = transform;
                    newObject.transform.localScale = transform.localScale*0.2f;
                    gameObject.GetComponent<MeshFilter>().mesh.Clear();
                    Debug.Log("Activé");
                }
                else Debug.Log("Ca marche paaaaaaaaas!");
                // com.material.color = Color.green;

                //gameObject.GetComponent<MeshFilter>().mesh = rewardFilter.mesh;

            }
       
            else com.material.color = Color.red;
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
        return Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - Camera.main.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.z - Camera.main.transform.position.z, 2));
    }

}
