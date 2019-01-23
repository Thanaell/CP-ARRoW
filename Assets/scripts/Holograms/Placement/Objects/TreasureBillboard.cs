using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
