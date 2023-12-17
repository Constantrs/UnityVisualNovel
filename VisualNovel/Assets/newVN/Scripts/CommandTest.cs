using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using Cysharp.Threading.Tasks;
using System.Threading;


public class ObjectManager
{
    private Dictionary<string, GameObject> _objDB = new Dictionary<string, GameObject>();

    public void SetObjecets(GameObject obj)
    {
        Transform[] Children = obj.GetComponentsInChildren<Transform>();

        foreach (Transform child in Children) 
        {
            _objDB.Add(child.name, child.gameObject);
        }
    }

    public GameObject GetObject(string name)
    {
        if (_objDB.ContainsKey(name))
        {
            return _objDB[name];
        }
        return null;
    }
}

public abstract class CommandExtension
{
    public static void CreateCommand(object[] param)
    {

    }

    public abstract string PrintDebugInfo();

    public abstract UniTask Execute(PlayerLoopTiming timing, CancellationToken taken);
}

public class CommandExtension_MoveCharacter : CommandExtension
{
    private Transform target = null;
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

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
        if (CommandTest.instance != null)
        {
            string name = (string)param[0];
            target = CommandTest.instance.GetManager().GetObject(name).GetComponent<Transform>();
        }
        startPos = (Vector3)param[1];
        endPos = (Vector3)param[2];
    }

    public override string PrintDebugInfo() 
    {
        string message = string.Format("startPos: {0} endPos: {1}", startPos, endPos);
        return message;
    }

    public async override UniTask Execute(PlayerLoopTiming inTiming, CancellationToken token)
    {
        Debug.Log("MoveCharacter Start");
        float timer = 0.0f;
        float totalTime = 300.0f;
        while (timer < totalTime)
        {
            var manager = UniTaskTest.instance;
            if (manager == null)
            {
                return;
            }

            target.position = Vector3.Lerp(startPos, endPos, timer / totalTime);
            timer += manager.GetTimeScale();
            await UniTask.Yield(timing: inTiming, cancellationToken: token);
        }
        Debug.Log("MoveCharacter End");
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

    public async override UniTask Execute(PlayerLoopTiming timing, CancellationToken taken)
    {
        Debug.Log("TalkMessage Start");
        await UniTask.Delay(1000, delayTiming: timing, cancellationToken: taken);
        Debug.Log("TalkMessage End");
    }
}

public class CommandProcess
{
    private CancellationTokenSource _cts;
    private PlayerLoopTiming _timing;
    private CommandExtension _command;

    public CommandProcess(CommandExtension command, PlayerLoopTiming timing = PlayerLoopTiming.Update)
    {
        _cts = new CancellationTokenSource();
        _timing = timing;
        _command = command;
    }

    public async UniTask Process()
    {
        try
        {
            await _command.Execute(_timing, _cts.Token);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(" UniTaskYieldProcess has been cancel!");
        }
    }

    public void StopProcess()
    {
        _cts.Cancel();
        _cts.Dispose();
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
    public static CommandTest instance { get; private set; }

    public GameObject objRoot;

    private Dictionary<string, Func<object[], CommandExtension>> commandDatabase = new Dictionary<string, Func<object[], CommandExtension>>();

    private List<CommandProcess> commandProcesses = new List<CommandProcess>();

    private ObjectManager objectManager = new ObjectManager();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        BuildMethod();

        objectManager.SetObjecets(objRoot);
    }

    private void Start()
    {
        //SaveXmlData();

        //LoadXmlData();

        ExecuteCommand();

        if (commandProcesses.Count > 0)
        {
            UpdateMainLoop().Forget();
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public ObjectManager GetManager()
    {
        return objectManager;
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

    private void ExecuteCommand()
    {
        XmlScriptData data = XmlExtension.Deserialize<XmlScriptData>("TestXml.xml");

        foreach (XmlScriptData.Command command in data.commands)
        {
            if (commandDatabase.ContainsKey(command.name))
            {
                CommandProcess process = new CommandProcess(commandDatabase[command.name]?.Invoke(command.arguments));
                commandProcesses.Add(process);
                Debug.Log($"Add Process {command.name}");
            }
        }
    }

    public async UniTask UpdateMainLoop()
    {
        Debug.Log("Task Start");

        List<UniTask> tasks = new List<UniTask>();
        foreach (var process in commandProcesses) 
        {
            tasks.Add(process.Process());
        }

        await UniTask.WhenAll(tasks.ToArray());

        Debug.Log("Task End");
    }

    private static XmlScriptData GetSampleData()
    {
        XmlScriptData data = new XmlScriptData();

        XmlScriptData.Command cmd011 = new XmlScriptData.Command();
        cmd011.name = "playSound";
        cmd011.groupId = 1;

        {
            string strValue = "asd";
            cmd011.arguments = new object[] { strValue };
        }

        XmlScriptData.Command cmd012 = new XmlScriptData.Command();
        cmd012.name = "playEff";
        cmd012.groupId = 1;
        {
            int value = 3;
            cmd012.arguments = new object[] { value };
        }

        XmlScriptData.Command cmd013 = new XmlScriptData.Command();
        cmd013.name = "moveCharacter";
        cmd013.groupId = 1;

        {
            string targetname = "Cube";
            Vector3 valueX = new Vector3(-2.0f, 0.0f, 0.0f);
            Vector3 valueY = new Vector3(-2.0f, 3.0f, 0.0f);
            cmd013.arguments = new object[] { targetname, valueX, valueY };
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
            int valueA = 6;
            int valueB = 7;

            cmd022.arguments = new object[] { valueA, valueB };
        }

        data.commands.Add(cmd021);
        data.commands.Add(cmd022);
        return data;
    }
}
