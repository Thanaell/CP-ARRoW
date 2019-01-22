using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

public class AddAnchor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.parent.gameObject.AddComponent<Interpolator>();
        transform.parent.gameObject.AddComponent<TapToPlace>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
