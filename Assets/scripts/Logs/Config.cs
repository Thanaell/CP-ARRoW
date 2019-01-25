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
    private static Config instance;
    private string path;    
    private XDocument myXmlDoc;
    private readonly XmlNode root;
    private XmlReader xmlReader;

    //Pour fonctionner, ce code nécessite un fichier myConfig.xml mis dans l'HoloLens : 
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogErrorFormat("Trying to instantiate a second instance of singleton class {0}", GetType().Name);
        }
        else
        {
            instance = (Config)this;
            path = Path.Combine(Application.persistentDataPath, "myConfig.xml");

            xmlReader = XmlReader.Create(path);
            myXmlDoc = XDocument.Load(xmlReader);
        }
    }

    //getter des dernières valeurs lues pour chaque type

    public int GetInt(string variableName)
    {
        int IntRead = 0;
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {
            if (variableNode.Attribute("name").Value == variableName)
            {
                IntRead = System.Convert.ToInt32(variableNode.Attribute("value").Value);
            }
        }
        return IntRead;
    }

    public double GetDouble(string variableName)
    {
        double doubleRead = 0.0;
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {

            if (variableNode.Attribute("name").Value == variableName)
            {
                doubleRead = System.Convert.ToDouble(variableNode.Attribute("value").Value);
            }

        }
        return doubleRead;
    }

    public string GetString(string variableName)
    {
        string stringRead = "";
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {
            if (variableNode.Attribute("name").Value == variableName)
            {
                stringRead = variableNode.Attribute("value").Value;
            }

        }
        return stringRead;
    }

    public bool GetBool(string variableName)
    {
        bool boolRead = false;
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {
            if (variableNode.Attribute("name").Value == variableName)
            {
                boolRead = System.Convert.ToBoolean(variableNode.Attribute("value").Value);
            }

        }
        return boolRead;
    }


    //récupère un int depuis le fichier de config. Renvoie false s'il n'a pas trouvé la variable
    public bool FetchIntFromConfig(string variableName)
    {
        var variableNodes = myXmlDoc.Descendants("variable");
        foreach (XElement variableNode in variableNodes)
        {
            if (variableNode.Attribute("name").Value == variableName)
            {
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
                return true;
            }

        }
        return false;
    }
}
