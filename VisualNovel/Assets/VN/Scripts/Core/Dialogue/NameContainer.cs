using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    }
}
