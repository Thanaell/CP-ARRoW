using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageListener : MonoBehaviour {

    void OnGazeEnter()
    {
        gameObject.transform.GetChild(0).SendMessage("OnGazeEnter", SendMessageOptions.DontRequireReceiver);
    }
    void OnGazeExit()
    {
        gameObject.transform.GetChild(0).SendMessage("OnGazeExit", SendMessageOptions.DontRequireReceiver);
    }

}
