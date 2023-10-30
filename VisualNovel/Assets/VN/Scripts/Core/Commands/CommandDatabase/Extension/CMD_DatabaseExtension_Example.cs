using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_DatabaseExtension_Example : CMD_DatabaseExtension
{
    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("print", new Action(PrintDefaultMessage));
    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("DebugInfo");
    }
}
