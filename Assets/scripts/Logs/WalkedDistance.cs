using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using HoloToolkit.Unity;

public class WalkedDistance : Singleton<WalkedDistance>
{

    public float tempsEchantillon;

    private float oldX;
    private float newX;
    private float oldZ;
    private float newZ;
    private float timer=0;
    private float globalTimer=0;
    private float walkedDistance=0;

    private string path;

    //getter de walked distance
    public float getWalkedDistance()
    {
        return walkedDistance;
    }
   

    void Start()
    {
        oldX = Camera.main.transform.position.x;
        oldZ = Camera.main.transform.position.z;

        path = Path.Combine(Application.persistentDataPath, "WalkedDistance.txt");
        File.WriteAllText(path, "0;0" + System.Environment.NewLine);

    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= tempsEchantillon)
        {
            timer = 0;
            walkedDistance += Mathf.Sqrt((oldX - newX) * (oldX - newX) + (oldZ - newZ) * (oldZ - newZ));

            oldZ = newZ;
            oldX = newX;
            newZ = Camera.main.transform.position.x;
            newX = Camera.main.transform.position.z;

            File.AppendAllText(path, globalTimer.ToString() + ";" + walkedDistance.ToString() + System.Environment.NewLine);
        }
        timer += Time.deltaTime;
        globalTimer += Time.deltaTime;
        //Debug.Log(walkedDistance);
    }


    //permet d'écrire dans le fichier walkedDistance la vitesse moyenne depuis le début de la session quand on quitte l'application

    private void OnApplicationQuit()
    {
        File.AppendAllText(path, "Vitesse moyenne au bout de " + globalTimer.ToString() + " : " + walkedDistance / globalTimer + " m/s");
    }
}