using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DIALOGUE_LINE
    {
        public SPEAKER_DATA speakerData;
        public DIALOGUE_DATA dialogueData;
        public COMMAND_DATA commandsData;

        public bool hasSpeaker => speakerData != null;
        public bool hasDialogue => dialogueData.hasDialogue();
        public bool hasCommands => commandsData != null;

        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speakerData = (string.IsNullOrWhiteSpace(speaker) ? null : new SPEAKER_DATA(speaker));
            this.dialogueData = new DIALOGUE_DATA(dialogue);
            this.commandsData = (string.IsNullOrWhiteSpace(commands) ? null : new COMMAND_DATA(commands));
        }
    }

}