using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Classe créeant un fichier Log_Position_date.txt et qui écrit dessus les positions et rotations de la caméra 
/// </summary>
public class LogPosition : MonoBehaviour {
    //temps d'échantillonnage à définir dans l'inspecteur
    public float tempsEchantillon;
    private string path;
    private float x;
    private float y;
    private float z;
    private float alpha;
    private float beta;
    private float gamma;
    private float timer=0;
    private float globalTimer=0;

    void Start ()
    {
        DateTime now = DateTime.Now;
        path = Path.Combine(Application.persistentDataPath, "LogPosition_" + now.Day + "_" + now.Month + "_" + now.Year + "_" + now.Hour + "_" + now.Minute + "_" + now.Second + ".txt");
        File.WriteAllText(path,"Timer (s);X (m);Y (m);Z (m); alpha (selon x); beta (selon y); gamma (selon z)" + System.Environment.NewLine);
    }
	
	void Update () {
        if (timer >= tempsEchantillon)
        {
            timer = 0;
            x = Camera.main.transform.position.x;
            y = Camera.main.transform.position.y;
            z = Camera.main.transform.position.z;
            alpha = Camera.main.transform.rotation.x;
            beta = Camera.main.transform.rotation.y;
            gamma = Camera.main.transform.rotation.z;  

            File.AppendAllText(path, globalTimer.ToString() + ";" + x + ";" + y + ";" + z + ";" + alpha + ";" + beta + ";" + gamma + System.Environment.NewLine);
        }
        timer += Time.deltaTime;
        globalTimer += Time.deltaTime;
    }
}
