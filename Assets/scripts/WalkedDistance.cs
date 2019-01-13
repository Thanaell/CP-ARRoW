using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WalkedDistance : MonoBehaviour
{
    private float oldX;
    private float newX;
    private float oldZ;
    private float newZ;
    private float timer;

    public float walkedDistance;
    public string txtDistance { get; set; }
    public GameObject TextPrefab;
    GameObject tip;
    private string path;


    // Use this for initialization
    void Start()
    {
        oldX = Camera.main.transform.localPosition.x;
        oldZ = Camera.main.transform.localPosition.z;

       tip = (GameObject)Instantiate(TextPrefab);

        path = Path.Combine(Application.persistentDataPath, "MyFile.txt");
        File.WriteAllText(path, "Walked distance every half second in last session : " + walkedDistance.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0.5)
        {
            timer = 0;
            walkedDistance += Mathf.Sqrt((oldX - newX) * (oldX - newX) + (oldZ - newZ) * (oldZ - newZ));
            Debug.Log(walkedDistance);
            //unit is meter
            txtDistance = string.Format("Distance : {0:#.00}  m", walkedDistance);
            tip.GetComponent<TextMesh>().text = txtDistance ;

            oldZ = newZ;
            oldX = newX;
            newZ = Camera.main.transform.localPosition.x;
            newX = Camera.main.transform.localPosition.z;

            File.AppendAllText(path, walkedDistance.ToString());
        }
        timer += Time.deltaTime;



    }
}