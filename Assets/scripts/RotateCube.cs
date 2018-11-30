using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private float speed;
    private Vector3 up;
    // Use this for initialization
    void Start()
    {
        speed = 20.0f;
        up = new Vector3(1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(up, speed * Time.deltaTime);
    }
}