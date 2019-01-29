using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ce script est utilisé dans le prefab TreasurePrefabWithClueWithText afin d'afficher le nombre d'objet nécessaires
 * pour débloquer le trésor
 */

public class TreasureBillboard : MonoBehaviour {

    [SerializeField]
    private TextMesh StatusDisplay;

    private bool hideText = false;

    private int clueIdToActivate;
    

    private string PrimaryText
    {
        get
        {
            if (hideText)
                return string.Empty;

            if (ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate) hideText = true;

            return string.Format("{0} / {1}", ObjectCollectionManager.Instance.ActiveObject , clueIdToActivate);
            
        }
    }

    private Color PrimaryColor
    {
        get
        {
           return ObjectCollectionManager.Instance.ActiveObject==3 ? Color.green : Color.magenta;
        }
    }


    private void Start()
    {
        /*on positionne l'objet au centre de son parent*/
        transform.position = gameObject.transform.parent.position;
    }


    private void Update()
    {
        // Basic checks
        if (StatusDisplay == null)
        {
            return;
        }

        clueIdToActivate =gameObject.transform.parent.GetComponent<ID>().id;
        // Update display text
        StatusDisplay.text = PrimaryText;
        StatusDisplay.color = PrimaryColor;
    }


}
