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

            //foreach (string line in lines) 
            //{
            //    if (string.IsNullOrEmpty(line)) continue;

            //    Debug.Log($"Segmenting Line '{line}'");
            //    DIALOGUE_LINE dlline = DialogueAnalyzer.Analyze(line); 

            //    int i = 0;
            //    foreach(DIALOGUE_DATA.DIALOGUE_SEGMENT segment in dlline.dialogue.segments)
            //    {
            //        Debug.Log($"Segment [{i++}] = '{segment.dialogue}'  [singal = {segment.startSignal.ToString()}{ (segment.signalDelay > 0 ? $"{segment.signalDelay}" : $"")}]");
            //    }

            //}

            DialogueSystem.instance.Say(lines);
        }
    }
}
