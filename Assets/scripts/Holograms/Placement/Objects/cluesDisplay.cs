using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ce script est utilisé dans le prefab TreasurePrefabWithClueWithCluesDisplay
 * Ce script est utilisé afin d'afficher les objets restants à ramasser pour 
 * ouvrir le coffre.
 */ 
public class cluesDisplay : MonoBehaviour {

    /*
     * Cette variable indique le nombre de clés déjà ramasser et connu par le coffre
     */ 
    int clueTaken = 0;

    [SerializeField]
    private TextMesh StatusDisplay;

    private bool hideText = false;


    private int clueIdToActivate;

    private void Start()
    {

        /*on positionne l'objet au centre de son parent*/
        transform.position = gameObject.transform.parent.position;
        /*
         * On instancie les copies des objets à ramasser. Les objets sont créés mais affichés au-dessus du coffre
         */
        for (int i = 0; i < ObjectCollectionManager.Instance.WallPrefabs.Count; i++)
            {
                GameObject newObject = Instantiate(ObjectCollectionManager.Instance.WallPrefabs[i],
                    gameObject.transform.position + new Vector3(0, 0.4f, 0) + gameObject.transform.parent.transform.parent.transform.right * (-(ObjectCollectionManager.Instance.WallPrefabs.Count-1) * 0.025f + i * 0.05f),
                    transform.parent.transform.parent.rotation * Quaternion.Euler(90f * ObjectCollectionManager.Instance.WallPrefabs[i].transform.forward),
                    gameObject.transform) as GameObject;
                newObject.transform.SetSiblingIndex(i);
                /*Pour ne pas causer les soucis de collision avec le cursor, nous désactivons les colliders*/
                if (newObject.GetComponent<Collider>().enabled == true)
                {
                    newObject.GetComponent<Collider>().enabled = false;
                }
            }


        StatusDisplay.color = Color.white;
        StatusDisplay.transform.position += new Vector3(0, 0.3f, 0);

    }

    void Update () {
        /*
         * Si l'objet collecté enregistré par coffre n'est pas le même que l'objet activé sur la scène 
         * alors on désactive l'affichage de l'objet nécessaire à ramasser et on on change la variable
         * clueTaken
         */ 
        if (clueTaken != ObjectCollectionManager.Instance.ActiveObject)
        {
            gameObject.transform.GetChild(ObjectCollectionManager.Instance.ActiveObject - 1).gameObject.SetActive(false);
            clueTaken = ObjectCollectionManager.Instance.ActiveObject;
        }

        // Basic checks
        if (StatusDisplay == null)
        {
            return;
        }

        clueIdToActivate = gameObject.transform.parent.GetComponent<ID>().id;
        // Update display text
        StatusDisplay.text = PrimaryText;

    }



    private string PrimaryText
    {
        get
        {
            if (hideText)
                return string.Empty;

            if (ObjectCollectionManager.Instance.ActiveObject == clueIdToActivate) hideText = true;

            return string.Format("Find the {0} keys around you\n to open the treasure chest", clueIdToActivate);

        }
    }

}
