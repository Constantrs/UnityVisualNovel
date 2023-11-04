using Dialogue;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AdvCharacter
{
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name = "";
        public string alias = "";
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;

        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;

        private static Color defualtColor => DialogueSystem.instance.config.defualtColor;
        private static TMP_FontAsset defaultFont => DialogueSystem.instance.config.defaultFont;

        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                result.name = "";
                result.alias = "";
                result.characterType = Character.CharacterType.Text;

                result.nameFont = defaultFont;
                result.dialogueFont = defaultFont;

                result.nameColor = defualtColor;
                result.dialogueColor = defualtColor;

                return result;
            }
        }

        public CharacterConfigData Copy()
        {
            CharacterConfigData result = new CharacterConfigData();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;
            result.nameFont = nameFont;
            result.dialogueFont = dialogueFont;

           result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
           result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            return result;
        }
    }
}
