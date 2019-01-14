using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPositionText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward*2;
	}
	
}
