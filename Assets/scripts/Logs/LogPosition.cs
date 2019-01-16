using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogPosition : MonoBehaviour {
    //temps d'échantillonnage à définir dans l'inspecteur
    public float tempsEchantillon;
    private string path;
    private float x;
    private float y;
    private float z;
    private float timer=0;
    private float globalTimer=0;

    void Start () {
        path = Path.Combine(Application.persistentDataPath, "LogPosition.txt");
        File.WriteAllText(path,"");
    }
	
	void Update () {
        if (timer >= tempsEchantillon)
        {
            timer = 0;
            x = Camera.main.transform.position.x;
            y = Camera.main.transform.position.y;
            z = Camera.main.transform.position.z;

            File.AppendAllText(path, globalTimer.ToString() + ";" + x + ";" + y + ";" + z + System.Environment.NewLine);
        }
        timer += Time.deltaTime;
        globalTimer += Time.deltaTime;
    }
}
