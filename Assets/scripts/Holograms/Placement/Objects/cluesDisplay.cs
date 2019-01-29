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
    
	void Update () {
        /*
         * Si le joueur a vu le coffre et que les clés ne sont pas encore collectées on affiche les objets à
         * collecter au-dessus du coffre
         */ 
        if (clueTaken == 0 & ObjectCollectionManager.Instance.TreasureIsSeen)
        {
            /*
             * On instancie les copies des objets à ramasser 
             */ 
            for (int i = 0; i < ObjectCollectionManager.Instance.WallPrefabs.Count; i++)
            {
                GameObject newObject = Instantiate(ObjectCollectionManager.Instance.WallPrefabs[i], 
                    gameObject.transform.position + new Vector3(0, 0.4f, 0) + gameObject.transform.parent.transform.parent.transform.right * (-ObjectCollectionManager.Instance.WallPrefabs.Count * 0.025f + i * 0.05f), 
                    transform.parent.transform.parent.rotation * Quaternion.Euler(90f * ObjectCollectionManager.Instance.WallPrefabs[i].transform.forward), 
                    gameObject.transform) as GameObject;
                newObject.transform.SetSiblingIndex(i);
                /*Pour ne pas causer les soucis de collision avec le cursor, nous désactivons les colliders*/
                if (newObject.GetComponent<Collider>().enabled == true)
                {
                    newObject.GetComponent<Collider>().enabled = false;
                }
            }
        }

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
	}
}
