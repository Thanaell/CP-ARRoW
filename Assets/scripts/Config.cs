using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using UnityEngine;
using HoloToolkit.Unity;

//file that defines variables and gets their values from an xml file stored in the File Explore : LocalData/CPARRoW/LocalState
public class Config : Singleton<Config>
{
    private string lastStringRead="unupdated string";
    private int lastIntRead;
    private double lastDoubleRead;
    private bool lastBoolRead;

    private string path;
    private string pathToWrite;
    
    private XDocument myXmlDoc;

    private XmlNode root;
    private XmlReader xmlReader;


    //Pour fonctionner, ce code nécessite un fichier myConfig.xml mis dans l'HoloLens : 
    void Start()
    {
        
        pathToWrite = Path.Combine(Application.persistentDataPath, "TestXMLReader.txt");
        path = Path.Combine(Application.persistentDataPath, "myConfig.xml");

        xmlReader = XmlReader.Create(path);
        myXmlDoc = XDocument.Load(xmlReader);
        
        if (!FetchIntFromConfig("myInt"))
        {
            Debug.Log("erreur à la récupération de l'entier");
        }
        if (!FetchDoubleFromConfig("myDouble"))
        {
            Debug.Log("erreur à la récupération du double");
        }
        if (!FetchStringFromConfig("myString"))
        {
            Debug.Log("erreur à la récupération du string");
        }

        //on vérifie que ça a marché
        //Debug.Log(lastStringRead + " " + lastDoubleRead + " " + lastIntRead);
        //File.WriteAllText(pathToWrite, lastStringRead+ " " +lastDoubleRead+ " "+ lastIntRead);
   
    }


    //getter des dernières valeurs lues pour chaque type

    public int getLastIntRead()
    {
        return lastIntRead;
    }

    public double getLastDoubleRead()
    {
        return lastDoubleRead;
    }

    public string getLastStringRead()
    {
        return lastStringRead;
    }

    public bool getLastBoolRead()
    {
        return lastBoolRead;
    }


    //récupère un int depuis le fichier de config. Renvoie false s'il n'a pas trouvé la variable
    public bool FetchIntFromConfig(string variableName)
    {
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {
            if (variableNode.Attribute("name").Value == variableName)
            {
                Debug.Log(variableNode);
                lastIntRead = System.Convert.ToInt32(variableNode.Attribute("value").Value);
                return true;
            }

        }
        return false;
    }

    //récupère un double depuis le fichier de config. Renvoie false s'il n'a pas trouvé la variable
    public bool FetchDoubleFromConfig(string variableName)
    {
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {

            if (variableNode.Attribute("name").Value == variableName)
            {
                lastDoubleRead = System.Convert.ToDouble(variableNode.Attribute("value").Value);
                return true;
            }

        }
        return false;
    }

    //récupère un booléen depuis le fichier de config. Renvoie false s'il n'a pas trouvé la variable
    //Rq: Sont acceptés pour true : "True", tout entier strictement positif
    //    Sont acceptés pour false : "False", 0, null
    public bool FetchBoolFromConfig(string variableName)
    {
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {
            if (variableNode.Attribute("name").Value == variableName)
            {
                lastBoolRead = System.Convert.ToBoolean(variableNode.Attribute("value").Value);
                return true;
            }

        }
        return false;
    }

    //récupère un string depuis le fichier de config. Renvoie false s'il n'a pas trouvé la variable
    public bool FetchStringFromConfig(string variableName)
    {
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {
            if (variableNode.Attribute("name").Value == variableName)
            {
                lastStringRead = variableNode.Attribute("value").Value;
                return true;
            }

        }
        return false;
    }
}
