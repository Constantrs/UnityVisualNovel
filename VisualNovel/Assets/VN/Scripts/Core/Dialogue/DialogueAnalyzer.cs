using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Dialogue
{
    public class DialogueAnalyzer : MonoBehaviour
    {
        private const string commandRegexPattern = @"[\w[\]]*[^\s]\(";

        public static DIALOGUE_LINE Analyze(string rawline)
        {
            Debug.Log($"Analyzing line - '{rawline}' ");

            (string speaker, string dialogue, string commands) = RipContent(rawline);

            Debug.Log($"Spearker = '{speaker}'\nDialogue = '{dialogue}'\nCommand = '{commands}' ");

            return new DIALOGUE_LINE(speaker, dialogue, commands);  
        }

        private static (string, string, string) RipContent(string rawline)
        {
            string speaker = "", dialogue = "", commands = "";

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            for (int i = 0; i < rawline.Length; i++) 
            {
               char currentchar = rawline[i];
               
                if(currentchar == '\\')
                {
                    isEscaped = true;
                }
                else if (currentchar == '"' && !isEscaped)
                {
                    if(dialogueStart == -1)
                    {
                        dialogueStart = i;
                    }
                    else if(dialogueEnd == -1)
                    {
                        dialogueEnd = i;
                    }
                }
                else
                {
                    isEscaped = false;
                }
            }

            Regex commandRegex = new Regex(commandRegexPattern);
            MatchCollection matches = commandRegex.Matches(rawline);
            int commandStart = -1;
            
            foreach (Match match in matches) 
            {
                if(match.Index < dialogueStart || match.Index > dialogueEnd) 
                {
                    commandStart = match.Index;
                    break;
                }
            }

            if (commandStart == -1 && (dialogueStart == -1 && dialogueEnd == -1))
            {
                return ("", "", rawline.Trim());
            }

            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                speaker = rawline.Substring(0, dialogueStart).Trim();
                dialogue = rawline.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"", "\"");
                if(commandStart !=  -1)
                {
                    commands = rawline.Substring(commandStart).Trim();
                }
            }
            else if(commandStart != -1 && dialogueStart > commandStart)
            {
                commands = rawline;
            }
            else
            {
                speaker = rawline;
            }

            return (speaker, dialogue, commands);
        }

    }
}
