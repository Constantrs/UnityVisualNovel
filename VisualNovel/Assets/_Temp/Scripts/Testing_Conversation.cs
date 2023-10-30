using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class Testing_Conversation : MonoBehaviour
    {
        public TextAsset textAsset;

        // Start is called before the first frame update
        void Start()
        {
            StartConversation();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StartConversation()
        {
            List<string> lines = FileManager.ReadTextAsset(textAsset);

            DialogueSystem.instance.Say(lines);
        }
    }
}
