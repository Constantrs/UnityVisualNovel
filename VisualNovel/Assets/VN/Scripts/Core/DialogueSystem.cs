using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private DialogueContainer dialogueContainer = new DialogueContainer();

        public static DialogueSystem instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                //
            }
        }

        public DialogueContainer GetContainer()
        {
            return dialogueContainer;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}