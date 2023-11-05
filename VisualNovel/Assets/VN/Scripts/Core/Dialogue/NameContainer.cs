using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class NameContainer
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        public void Show(string name = "")
        {
            root.SetActive(true);

            if (name != string.Empty)
            {
                nameText.text = name;
            }
        }

        public void Hide()
        {
            root.SetActive(false);
        }

        public void SetNameColor(Color color) => nameText.color = color;

        public void SetNameFont(TMP_FontAsset font) => nameText.font = font;
    }
}
