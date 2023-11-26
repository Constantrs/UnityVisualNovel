using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;

namespace Commands
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager instance { get; private set; }

        private CommandDatabase database;

        private List<CommandProcess> activeProcess = new List<CommandProcess>();
        private CommandProcess topProcess => activeProcess.Last();

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

        public CoroutineWrapper Exetute(string commandName, params string[] args)
        {
            Delegate command = database.GetCommand(commandName);

            if (command == null)
                return null;

            return StartProcess(commandName, command, args);
        }

        private CoroutineWrapper StartProcess(string commandName, Delegate command, string[] args)
        {
            System.Guid processID = System.Guid.NewGuid();
            CommandProcess cmd = new CommandProcess(processID, commandName, command, null, args, null);
            activeProcess.Add(cmd);

            Coroutine co = StartCoroutine(CoRunningProcess(cmd));
            cmd.runningProcess = new CoroutineWrapper(this, co);
            return cmd.runningProcess;
        }

        public void StopCurrentProcess()
        {
            if (topProcess != null)
            {
                KillProcess(topProcess);
            }
        }

        public void StopAllProcesses()
        {
            foreach (var c in activeProcess)
            {
                if (c.runningProcess != null && !c.runningProcess.isDone)
                {
                    c.runningProcess.Stop();
                }

                c.onTerminateAction?.Invoke();
            }

            activeProcess.Clear();
        }

        public void KillProcess(CommandProcess process)
        {
            activeProcess.Remove(process);

            if (process.runningProcess != null && !process.runningProcess.isDone)
                process.runningProcess.Stop();

            process.onTerminateAction?.Invoke();
        }

        private IEnumerator CoRunningProcess(CommandProcess process)
        {
            yield return WaitingForProcessToComplete(process.command, process.args);

            KillProcess(process);
        }

        private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
            {
                command.DynamicInvoke();
            }
            else if (command is Action<string>)
            {
                command.DynamicInvoke(args[0]);
            }
            else if (command is Action<string[]>)
            {
                command.DynamicInvoke((object)args);
            }
            else if (command is Func<IEnumerator>)
            {
                yield return ((Func<IEnumerator>)command)();
            }
            else if (command is Func<string, IEnumerator>)
            {
                yield return ((Func<string, IEnumerator>)command)(args[0]);
            }
            else if (command is Func<string[], IEnumerator>)
            {
                yield return ((Func<string[], IEnumerator>)command)(args);
            }
        }

        public void AddTerminationActionToCurrentProcess(UnityAction action)
        {
            CommandProcess process = topProcess;

            if (process == null)
            {
                return;
            }

            process.onTerminateAction = new UnityEvent();
            process.onTerminateAction.AddListener(action);
        }
    }
}
