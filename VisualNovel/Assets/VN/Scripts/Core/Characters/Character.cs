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
        public RectTransform root = null;
        public CharacterConfigData config;

        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        public Character(string name, CharacterConfigData config)
        {
            this.name = name;
            this.displayName = name;
            this.config = config;
        }

        public Coroutine Say(string text) => Say(new List<string> { text });

        public Coroutine Say(List<string> text)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            dialogueSystem.ApplySperakerDataToDialogueContainer(name);
            return dialogueSystem.Say(text);
        }

    }
}
