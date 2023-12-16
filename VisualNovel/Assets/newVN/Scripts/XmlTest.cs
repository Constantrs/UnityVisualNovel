using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using static UnityEngine.Mesh;

[System.Serializable, XmlRoot("CommandRoot")]
public class XmlScriptCommands
{
    [XmlElement("Command")]
    public XmlScriptCommand[] command;
}

public class XmlScriptCommand
{
    [XmlAttribute("Name")]
    public string name;

    public Vector3 postion = Vector3.zero;
}

public class XmlExtension
{
    private static readonly string rootPath = "/newVN/Xml/";

    // シリアライズ
    #region Serializer
    public static void Serialize<T>(string filename, T data)
    {
        string path = GetPath(filename);

        // IDisposable継承したら、自動解放
        using (var stream = new FileStream(path, FileMode.Create))
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, data);
            Debug.Log($"Serialize XmlFile: {path}");
        }
    }
    #endregion

    // デシリアライズ
    #region Deserialize
    public static T Deserialize<T>(string filename)
    {
        string path = GetPath(filename);

        using (var stream = new FileStream(path, FileMode.Open))
        {
            var serializer = new XmlSerializer(typeof(T));
            Debug.Log($"Deserialize XmlFile: {path}");
            return (T)serializer.Deserialize(stream);
        }
    }
    #endregion

    private static string GetPath(string path)
    {
        string fullpath = Application.dataPath;
        fullpath += rootPath;
        fullpath += path;
        return fullpath;
    }
}

public class XmlTest : MonoBehaviour
{
    void Start()
    {
        //SaveXmlData();
        
        LoadXmlData();
    }

    private void SaveXmlData()
    {
        XmlScriptCommands commands = new XmlScriptCommands();
        commands.command = new XmlScriptCommand[3];
        commands.command[0] = new XmlScriptCommand();
        commands.command[0].name = "playSound";
        commands.command[0].postion = new Vector3(0.0f, 0.0f, 1.0f);
        commands.command[1] = new XmlScriptCommand();
        commands.command[1].name = "playEff";
        commands.command[1].postion = new Vector3(1.0f, 0.0f, 0.0f);
        commands.command[2] = new XmlScriptCommand();
        commands.command[2].name = "talkMessage";

        XmlExtension.Serialize<XmlScriptCommands>("TestXml.xml", commands);
    }

    private void LoadXmlData()
    {
        XmlScriptCommands data = XmlExtension.Deserialize<XmlScriptCommands>("TestXml.xml");

        foreach(XmlScriptCommand command in data.command)
        {
            Debug.Log(command.name);
        }
    }

}
