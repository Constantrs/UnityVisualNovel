using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvCharacter
{
    public abstract class Character
    {
        public enum CharacterType
        {
            Text,
            Sprite,
            Live2D,
            Model3D
        }

        public string name = "";
        public string displayName = "";
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        public Coroutine Say(string text) => Say(new List<string> { text });

        public Coroutine Say(List<string> text)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            return dialogueSystem.Say(text);
        }

        public Character(string name)
        {
            this.name = name;
            this.displayName = name;
        }

    }
}
