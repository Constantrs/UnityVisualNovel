using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Commands
{
    public class CMD_DatabaseExtension_Example : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            // Add action with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print_lp", new Action<string>(PrintUserMessage));
            database.AddCommand("print_mp", new Action<string[]>(PrintLines));

            // Add lambda with no parameters
            database.AddCommand("lambda", new Action(() => { Debug.Log("DebugInfo from lambda"); }));
            database.AddCommand("lambda_lp", new Action<string>((arg) => { Debug.Log($"User Lambda Message : '{arg}'"); }));
            database.AddCommand("lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));

            // Add coroutine with no parameters
            database.AddCommand("process", new Func<IEnumerator>(CoSampleProcess));
            database.AddCommand("process_lp", new Func<string, IEnumerator>(CoLineProcess));
            database.AddCommand("process_mp", new Func<string[], IEnumerator>(CoMultLineProcess));
        }

        private static void PrintDefaultMessage()
        {
            Debug.Log("DebugInfo");
        }


        private static void PrintUserMessage(string message)
        {
            Debug.Log($"User Message : '{message}'");
        }

        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. : '{line}'");
            }
        }

        private static IEnumerator CoSampleProcess()
        {
            for (int i = 1; i <= 5; i++)
            {
                Debug.Log($"Process Running... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator CoLineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 1; i <= 5; i++)
                {
                    Debug.Log($"Process Running... [{i}]");
                    yield return new WaitForSeconds(1);
                }
            }
        }

        private static IEnumerator CoMultLineProcess(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Process Message: [{line}]");
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}