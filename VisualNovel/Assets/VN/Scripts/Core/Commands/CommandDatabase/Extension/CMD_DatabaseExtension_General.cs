using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            // Add action with no parameters
            database.AddCommand("wait", new Func<string, IEnumerator>(CoWait));
        }
        private static IEnumerator CoWait(string data)
        {
            if (float.TryParse(data, out float time)) 
            {
                yield return new WaitForSeconds(time);
            }
        }
    }
}