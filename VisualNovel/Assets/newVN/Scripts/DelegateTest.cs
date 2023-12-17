using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;


public class CommandDB
{
    private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

    public bool HasCommand(string commandName) => database.ContainsKey(commandName);

    public void AddCommand(string commandName, Delegate command)
    {
        commandName = commandName.ToLower();

        if (!database.ContainsKey(commandName))
        {
            database.Add(commandName, command);
        }
        else
        {
            Debug.LogError($"Command already exists in database! '{commandName}'");
        }
    }

    public Delegate GetCommand(string commandName)
    {
        commandName = commandName.ToLower();

        if (!database.ContainsKey(commandName))
        {
            Debug.LogError($"Command '{commandName}' does not exists in the database!");
            return null;
        }
        else
        {
            return database[commandName];
        }
    }
}

public class TaskProcess
{
    private CancellationTokenSource _cts;
    private PlayerLoopTiming _timing;
    //private UniTaskCommand command;

    //public TaskProcess(UniTaskCommand command)
    //{
    //    _cts = new CancellationTokenSource();
    //    _timing = PlayerLoopTiming.LastEarlyUpdate;
    //}

    //public async UniTask Process(UniTaskCommand command)
    //{
    //    try
    //    {
    //        await command(_timing, _cts.Token);
    //    }
    //    catch (OperationCanceledException e)
    //    {
    //        Debug.Log(" UniTaskYieldProcess has been cancel!");
    //    }
    //}
}

public class DelegateTest : MonoBehaviour
{ 
    public static DelegateTest instance { get; private set; }

    private CommandDB database;

    //private List<CommandProcess> activeProcess = new List<CommandProcess>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            database = new CommandDB();

            //Assembly assembly = typeof(DelegateTest).Assembly;
            //Type[] extensionType = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CommandExtension))).ToArray();

            //foreach (Type extension in extensionType)
            //{
            //    MethodInfo extendMethod = extension.GetMethod("Extend");
            //    extendMethod.Invoke(instance, new object[] { database });
            //}
        }
        else
        {

        }
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
