using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DIALOGUE_LINE
    {
        public string speaker;
        public DIALOGUE_DATA dialogue;
        public string commands;

        public bool hasSpeaker => speaker != null;
        public bool hasDialogue => dialogue.hasDialogue();
        public bool hasCommands => commands != null;

        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speaker = speaker;
            this.dialogue = new DIALOGUE_DATA(dialogue);
            this.commands = commands;
        }
    }

}