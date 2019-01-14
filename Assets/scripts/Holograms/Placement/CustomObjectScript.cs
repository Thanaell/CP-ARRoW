using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObjectScript : MonoBehaviour {

    public void addComponents(GameObject go)
    {
        go.AddComponent<InteractionScript>();
    }
}
