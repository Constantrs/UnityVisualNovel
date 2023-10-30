using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager instance { get; private set; }
    private CommandDatabase database;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            database = new CommandDatabase();

            Assembly assembly = typeof(CommandManager).Assembly;
            Type[] extensionType = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

            foreach (Type extension in extensionType)
            {
                MethodInfo extendMethod = extension.GetMethod("Extend");
                extendMethod.Invoke(instance, new object[] { database });
            }
        }
        else
        {

        }
    }

    public void Exetute(string commandName)
    {
        Delegate command = database.GetCommand(commandName);

        if(command != null)
        {
            command.DynamicInvoke();
        }
    }

}
