using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public void Exit()
    {
        
        Application.Quit();
    }
    public void LanceSpatialMapping()
    
    {
        SceneManager.LoadScene(1);

    }

    public void LanceSpatialAnchors()

    {
        SceneManager.LoadScene(2);

    }

    public void LanceNoEffect()

    {
        SceneManager.LoadScene(3);

    }
}
