using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ce script est utilisé dans le prefab CluePrefabWithParticlesAndText afin d'afficher le text pour attirer l'utilisateur
 */

public class ClueBillboard : MonoBehaviour {

    [SerializeField]
    private TextMesh StatusDisplay;

    private bool hideText;


    private string PrimaryText
    {
        get
        {
            if (hideText)
                return string.Empty;
            
            return string.Format("Get closer to catch the key");

        }
    }

    private Color PrimaryColor
    {
        get
        {
            return Color.green;
        }
    }


    private void Start()
    {
        /*on positionne l'objet au centre de son parent*/
        transform.position = gameObject.transform.parent.position + new Vector3(0,-0.1f,0);
        hideText = true;
    }


    private void Update()
    {
        // Basic checks
        if (StatusDisplay == null)
        {
            return;
        }

        // Update display text
        StatusDisplay.text = PrimaryText;
        StatusDisplay.color = PrimaryColor;
    }

    bool clueCollected = false;

    void OnGazeEnter()
    {
        if (!clueCollected)
        {
            hideText = false;
        }
    }

    void OnGazeExit()
    {
        hideText = true;
    }

    void ClueCollected()
    {
        clueCollected = true;
        hideText = true;
    }
}
