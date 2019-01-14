using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;

//file that defines variables and gets their values from an xml file stored in the File Explore : LocalData/CPARRoW/LocalState
public class Config : MonoBehaviour
{

    private string myString;
    private int myInt;
    private double myDouble;

    private string path;
    private string pathToWrite;

    //Piur fonctionner, ce code nécessite un fichier myConfig.xml mis dans l'HoloLens : 
    void Start()
    {
        pathToWrite = Path.Combine(Application.persistentDataPath, "TestXMLReader.txt");
        path = Path.Combine(Application.persistentDataPath, "myConfig.xml");
        XmlReader xmlReader = XmlReader.Create(path);
        while (xmlReader.Read())
        {
            if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "variable"))
            {
                if (xmlReader.GetAttribute("name") == "myInt")
                {
                    string tempMyInt = xmlReader.GetAttribute("value");
                    myInt = System.Convert.ToInt32(tempMyInt);
                }
                if (xmlReader.GetAttribute("name") == "myString")
                {
                    myString = xmlReader.GetAttribute("value");
                }
                if (xmlReader.GetAttribute("name") == "myDouble")
                {
                    string tempMyDouble = xmlReader.GetAttribute("value");
                    myDouble = System.Convert.ToDouble(tempMyDouble);
                }
            }

            //on vérifie que ça a marché
            Debug.Log(myString + myInt + myDouble);
            File.WriteAllText(pathToWrite, myString + myInt.ToString() + myDouble.ToString());

        }
    }
}
