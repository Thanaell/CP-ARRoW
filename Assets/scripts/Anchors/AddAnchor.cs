using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

/// <summary>
///  Classe ajoutant les éléments Interpolator et TapToPlace à son parent
///  afin d'utilier les Spatial Anchors
/// </summary>
public class AddAnchor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.parent.gameObject.AddComponent<Interpolator>();
        transform.parent.gameObject.AddComponent<TapToPlace>();
	}
}
