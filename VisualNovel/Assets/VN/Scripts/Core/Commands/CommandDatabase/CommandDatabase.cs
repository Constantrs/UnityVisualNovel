using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class CommandDatabase
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
}
