using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using Unity.VisualScripting;


public abstract class CommandExtension
{
    public static void CreateCommand(object[] param)
    {

    }

    public abstract string PrintDebugInfo();
}

public class CommandExtension_MoveCharacter : CommandExtension
{
    private Vector3 startPos;
    private Vector3 endPos;

    new public static CommandExtension CreateCommand(object[] param)
    {
        if (param == null)
        {
            return null;
        }
        CommandExtension cmd = new CommandExtension_MoveCharacter(param);
        Debug.Log(cmd.PrintDebugInfo());
        return cmd;
    }
    public CommandExtension_MoveCharacter(object[] param)
    {
        startPos = (Vector3)param[0];
        endPos = (Vector3)param[1];
    }

    public override string PrintDebugInfo() 
    {
        string message = string.Format("startPos: {0} endPos: {1}", startPos, endPos);
        return message;
    }
}

public class CommandExtension_TalkMessage : CommandExtension
{
    private int spearkerId;
    private int messageId;

    new public static CommandExtension CreateCommand(object[] param)
    {
        if (param == null)
        {
            return null;
        }
        CommandExtension cmd = new CommandExtension_TalkMessage(param);
        Debug.Log(cmd.PrintDebugInfo());
        return cmd;
    }

    public CommandExtension_TalkMessage(object[] param)
    {
        spearkerId = (int)param[0];
        messageId = (int)param[1];
    }

    public override string PrintDebugInfo()
    {
        string message = string.Format("speakerID: {0} messageID: {1}", spearkerId, messageId);
        return message;
    }
}


[System.Serializable, XmlRoot("CommandRoot"), XmlInclude(typeof(Command))]
public class XmlScriptData
{
    // コマンド
    [XmlType("Command")]
    public class Command
    {
        [XmlAttribute("Name")]
        public string name;

        [XmlAttribute("GroupId")]
        public int groupId;

        [XmlArray("Arguments"), XmlArrayItem("Argument")]
        public object[] arguments;

        public Command()
        {
            name = "invalid command";
        }

    }

    // コマンド変数
    //[XmlType("Argument")]
    //public class CommandArgument
    //{
    //    //[XmlAttribute("Value")]
    //    public object value;
    //}

    [XmlArray("Commands"), XmlArrayItem("Command")]
    public List<Command> commands = new List<Command>();
}

public class XmlExtension
{
    private static readonly string rootPath = "/newVN/Xml/";

    private static readonly Type[] extraType = { typeof(object), typeof(Vector3) };

// シリアライズ
#region Serializer
public static void Serialize<T>(string filename, T data)
    {
        string path = GetPath(filename);

        // IDisposable継承したら、自動解放
        using (var stream = new FileStream(path, FileMode.Create))
        {
            var serializer = new XmlSerializer(typeof(T), extraType);
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
            var serializer = new XmlSerializer(typeof(T), extraType);
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
    private Dictionary<string, Func<object[], CommandExtension>> commandDatabase = new Dictionary<string, Func<object[], CommandExtension>>();

    private void Start()
    {
        BuildMethod();

        //SaveXmlData();

        LoadXmlData();
    }

    private void BuildMethod()
    {
        commandDatabase.Add("moveCharacter", CommandExtension_MoveCharacter.CreateCommand);
        commandDatabase.Add("talkMessage", CommandExtension_TalkMessage.CreateCommand);
    }

    private void SaveXmlData()
    {
        var data = GetSampleData();

        XmlExtension.Serialize<XmlScriptData>("TestXml.xml", data);
    }

    private void LoadXmlData()
    {
        XmlScriptData data = XmlExtension.Deserialize<XmlScriptData>("TestXml.xml");

        foreach(XmlScriptData.Command command in data.commands)
        {
            Debug.Log(command.name);

            if (commandDatabase.ContainsKey(command.name))
            {
                // Delegate実行
                commandDatabase[command.name]?.Invoke(command.arguments);
            }
        }
    }

    private static XmlScriptData GetSampleData()
    {
        XmlScriptData data = new XmlScriptData();

        XmlScriptData.Command cmd011 = new XmlScriptData.Command();
        cmd011.name = "playSound";
        cmd011.groupId = 1;

        {
            //XmlScriptData.CommandArgument argument1101 = new XmlScriptData.CommandArgument();
            string strValue = "asd";
            cmd011.arguments = new object[] { strValue };
            //cmd011.arguments.Add(argument1101);
        }

        XmlScriptData.Command cmd012 = new XmlScriptData.Command();
        cmd012.name = "playEff";
        cmd012.groupId = 1;
        {
            //XmlScriptData.CommandArgument argument1201 = new XmlScriptData.CommandArgument();
            int value = 3;
            cmd012.arguments = new object[] { value };
            //cmd012.arguments.Add(argument1201);
        }

        XmlScriptData.Command cmd013 = new XmlScriptData.Command();
        cmd013.name = "moveCharacter";
        cmd013.groupId = 1;

        {
            //XmlScriptData.CommandArgument argument1301 = new XmlScriptData.CommandArgument();
            Vector3 valueX = new Vector3(0.0f, 1.0f, 0.0f);
            //argument1301.value = valueX;
            cmd013.arguments = new object[] { valueX };
            //XmlScriptData.CommandArgument argument1302 = new XmlScriptData.CommandArgument();
            Vector3 valueY = new Vector3(0.0f, -1.0f, 0.0f);
            //argument1302.value = valueY;
            cmd013.arguments = new object[] { valueX, valueY };

            //cmd013.arguments.Add(argument1301);
            //cmd013.arguments.Add(argument1302);
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
            //XmlScriptData.CommandArgument argument2201 = new XmlScriptData.CommandArgument();
            int valueA = 6;
            //argument2201.value = valueA;

            //XmlScriptData.CommandArgument argument2202 = new XmlScriptData.CommandArgument();
            int valueB = 7;
            //argument2202.value = valueB;

            cmd022.arguments = new object[] { valueA, valueB };

            //cmd022.arguments.Add(argument2201);
            //cmd022.arguments.Add(argument2202);
        }

        data.commands.Add(cmd011);
        data.commands.Add(cmd012);
        data.commands.Add(cmd013);
        data.commands.Add(cmd021);
        data.commands.Add(cmd022);
        return data;
    }
}
