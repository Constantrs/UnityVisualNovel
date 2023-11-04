using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace AdvCharacter
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigurationAsset;

        private class CHARACTER_INFO
        {
            public string name;

            public CharacterConfigData config = null;

        }

        private void Awake()
        {
            instance = this;
        }

        public Character CreateCharacter(string characterName)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogError($"A Character called {characterName} already exists.");
                return null;
            }

            CHARACTER_INFO info = GetCharacterInfo(characterName);
            Character character = CreateCharacterFromInfo(info);
            characters.Add(characterName.ToLower(), character);

            return character;
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO resuslt = new CHARACTER_INFO();
            resuslt.name = characterName;
            resuslt.config = config.GetConfig(characterName);
            return resuslt;
        }


        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;

            if(config.characterType == Character.CharacterType.Text)
            {
                return new Character_Text(info.name);
            }
            else if (config.characterType == Character.CharacterType.Sprite)
            {
                return new Character_Sprite(info.name);
            }

            return null;
        }
    }
}
