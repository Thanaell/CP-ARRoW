using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using UnityEngine;
using HoloToolkit.Unity;

/// <summary>
///  Classe récupérant les données contenus dans un fichier XML, récupérable dans le File Explorer
///  de l'HoloLens à l'emplacement : LocalData/CPARRoW/LocalState, afin d'utiliser ces données dans les autres scripts
/// </summary>
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

    /// <summary>
    /// Méthode récupérant la valeur d'une variable Int
    /// </summary>
    /// <param name="variableName"> Nom de la variable </param>
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

    /// <summary>
    /// Méthode récupérant la valeur d'une variable Double
    /// </summary>
    /// <param name="variableName">Nom de la variable</param>
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

    /// <summary>
    /// Méthode récupérant la valeur d'une valeur String
    /// </summary>
    /// <param name="variableName">Nom de la variable</param>
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

    /// <summary>
    /// Méthode récupérant la valeur d'une variable Boolean
    /// </summary>
    /// <param name="variableName">Nom de la variable</param>
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


    /// <summary>
    /// Méthode permettant de vérifier l'existence d'un entier en fonction de son nom
    /// </summary>
    /// <param name="variableName">Nom de la variable</param>
    /// <returns>True si l'élément existe, False sinon.</returns>
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

    /// <summary>
    /// Méthode permettant de vérifier l'existence d'un double en fonction de son nom
    /// </summary>
    /// <param name="variableName">Nom de la variable</param>
    /// <returns>True si l'élément existe, False sinon.</returns>    
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

    /// <summary>
    /// Méthode permettant de vérifier l'existence d'un booléen en fonction de son nom
    /// Remarque : sont acceptés pour true : "True", tout entier strictement positif
    ///            sont acceptés pour false : "False", 0, null
    /// </summary>
    /// <param name="variableName">Nom de la variable</param>
    /// <returns>True si l'élément existe, False sinon.</returns> 
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

    /// <summary>
    /// Méthode permettant de vérifier l'existence d'un string en fonction de son nom
    /// </summary>
    /// <param name="variableName">Nom de la variable</param>
    /// <returns>True si l'élément existe, False sinon.</returns> 
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
