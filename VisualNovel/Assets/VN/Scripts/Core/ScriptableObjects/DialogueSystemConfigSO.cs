using UnityEngine;
using TMPro;
using AdvCharacter;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigSO : ScriptableObject
    {
        public CharacterConfigSO characterConfigurationAsset;

        public Color defualtColor = Color.white;
        public TMP_FontAsset defaultFont;
    }
}