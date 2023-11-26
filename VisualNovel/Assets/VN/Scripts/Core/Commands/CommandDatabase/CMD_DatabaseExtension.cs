using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public abstract class CMD_DatabaseExtension
    {
        public static void Extend(CommandDatabase database)
        {

        }

        public CommandParameters ConvertDataToParameters(string[] data) => new CommandParameters(data);
    }
}
