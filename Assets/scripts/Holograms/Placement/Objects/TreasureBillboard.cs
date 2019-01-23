using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBillboard : MonoBehaviour {

    [SerializeField]
    private TextMesh StatusDisplay;

    private bool hideText = false;
    
   

    public bool HideText
    {
        get
        {
            return hideText;
        }
        set
        {
            hideText = value;
        }
    }



    private string PrimaryText
    {
        get
        {
            if (HideText)
                return string.Empty;

            return string.Format("{0} / {1}", ObjectCollectionManager.Instance.ActiveObject , 3);
            
        }
    }

    private Color PrimaryColor
    {
        get
        {
           return ObjectCollectionManager.Instance.ActiveObject==3 ? Color.green : Color.green;
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

        // Update display text
        StatusDisplay.text = PrimaryText;
        StatusDisplay.color = PrimaryColor;
    }


}
