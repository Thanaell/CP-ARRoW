using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class RotateCube : MonoBehaviour, IInputClickHandler
{
    private float speed;
    private Vector3 up;
    private bool movable = false;
    // Use this for initialization
    void Start()
    {
        speed = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(up, speed * Time.deltaTime);
        if (movable)
        {
            this.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
        }
    }

    void OnMove()
    {
        movable = true;
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    void OnStop()
    {
        movable = false;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    void OnGazeEnter()
    {
        //this.GetComponent<Renderer>().material.color = Color.green;
    }

    void OnGazeExit()
    {
        //this.GetComponent<Renderer>().material.color = Color.red;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        this.GetComponent<Renderer>().material.color = Color.green;
    }
}