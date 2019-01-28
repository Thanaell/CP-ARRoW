using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cluesDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = 0; i < ObjectCollectionManager.Instance.WallPrefabs.Count; i++)
        {
            GameObject newObject = Instantiate(ObjectCollectionManager.Instance.WallPrefabs[i], gameObject.transform.position + new Vector3(0, 0.4f, 0) + gameObject.transform.parent.transform.parent.transform.right*(-ObjectCollectionManager.Instance.WallPrefabs.Count * 0.025f + i * 0.05f),new Quaternion(0,0,0,0)/*Quaternion.Euler(90f * gameObject.transform.forward)*/, gameObject.transform) as GameObject;
            newObject.transform.SetSiblingIndex(i);
            if (newObject.GetComponent<Collider>().enabled == true)
            {
                newObject.GetComponent<Collider>().enabled = false;
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
