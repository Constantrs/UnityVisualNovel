using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DIALOGUE_LINE
    {
        public SPEAKER_DATA speaker;
        public DIALOGUE_DATA dialogue;
        public string commands;

        public bool hasSpeaker => speaker != null;
        public bool hasDialogue => dialogue.hasDialogue();
        public bool hasCommands => commands != string.Empty;

        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speaker = (string.IsNullOrWhiteSpace(speaker) ? null : new SPEAKER_DATA(speaker));
            this.dialogue = new DIALOGUE_DATA(dialogue);
            this.commands = commands;
        }
    }

}