using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable, XmlRoot("CommandRoot"), XmlInclude(typeof(Command))]
public class XmlScriptData
{
    // コマンド
    [XmlType("Command"), XmlInclude(typeof(CommandArgument))]
    public class Command
    {
        [XmlAttribute("Name")]
        public string name;

        [XmlAttribute("GroupId")]
        public int groupId;

        [XmlArray("Arguments"), XmlArrayItem("Argument")]
        public List<CommandArgument> arguments;

        public Command()
        {
            name = "invalid command";
            arguments = new List<CommandArgument>();
        }

    }

    // コマンド変数
    [XmlType("Argument")]
    public class CommandArgument
    {
        //[XmlAttribute("Value")]
        public object value;
    }

    [XmlArray("Commands"), XmlArrayItem("Command")]
    public List<Command> commands = new List<Command>();
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
            var streamWriter = new StreamWriter(stream, System.Text.Encoding.UTF8);
            serializer.Serialize(streamWriter, data);
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

public class CommandTest : MonoBehaviour
{

    void Start()
    {
        //SaveXmlData();
        
        LoadXmlData();
    }

    private void SaveXmlData()
    {
        XmlScriptData data = new XmlScriptData();

        XmlScriptData.Command cmd011 = new XmlScriptData.Command();
        cmd011.name = "playSound";
        cmd011.groupId = 1;
        
        {
            XmlScriptData.CommandArgument argument1101 = new XmlScriptData.CommandArgument();
            string strValue = "asd";
            argument1101.value = strValue;
            cmd011.arguments.Add(argument1101);
        }
        
        XmlScriptData.Command cmd012 = new XmlScriptData.Command();
        cmd012.name = "playEff";
        cmd012.groupId = 1;
        {
            XmlScriptData.CommandArgument argument1201 = new XmlScriptData.CommandArgument();
            int value = 3;
            argument1201.value = value;
            cmd012.arguments.Add(argument1201);
        }

        XmlScriptData.Command cmd013 = new XmlScriptData.Command();
        cmd013.name = "talkMessage";
        cmd013.groupId = 1;

        {
            XmlScriptData.CommandArgument argument1301 = new XmlScriptData.CommandArgument();
            float valueX = 3.0f;
            argument1301.value = valueX;
            XmlScriptData.CommandArgument argument1302 = new XmlScriptData.CommandArgument();
            float valueY = 3.0f;
            argument1302.value = valueY;

            cmd013.arguments.Add(argument1301);
            cmd013.arguments.Add(argument1302);
        }

        data.commands.Add(cmd011);
        data.commands.Add(cmd012);
        data.commands.Add(cmd013);

        XmlScriptData.Command cmd021 = new XmlScriptData.Command();
        cmd021.name = "playBGM";
        cmd021.groupId = 2;

        XmlScriptData.Command cmd022 = new XmlScriptData.Command();
        cmd022.name = "talkMessage";
        cmd022.groupId = 2;

        {
            XmlScriptData.CommandArgument argument2201 = new XmlScriptData.CommandArgument();
            int valueA = 6;
            argument2201.value = valueA;
            XmlScriptData.CommandArgument argument2202 = new XmlScriptData.CommandArgument();
            int valueB = 7;
            argument2202.value = valueB;
            cmd022.arguments.Add(argument2201);
            cmd022.arguments.Add(argument2202);
        }

        data.commands.Add(cmd011);
        data.commands.Add(cmd012);
        data.commands.Add(cmd013);
        data.commands.Add(cmd021);
        data.commands.Add(cmd022);

        XmlExtension.Serialize<XmlScriptData>("TestXml.xml", data);
    }

    private void LoadXmlData()
    {
        XmlScriptData data = XmlExtension.Deserialize<XmlScriptData>("TestXml.xml");

        foreach(XmlScriptData.Command command in data.commands)
        {
            Debug.Log(command.name);
        }
    }

}
