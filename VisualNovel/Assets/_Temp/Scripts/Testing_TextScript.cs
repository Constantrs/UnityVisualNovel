using UnityEngine;
using Dialogue;
using System.Collections.Generic;

namespace Testing
{
    public class Testing_TextScript : MonoBehaviour
    {
        [SerializeField] private TextAsset asset;

        // Start is called before the first frame update
        void Start()
        {
            SendFileToAnalyze();
        }

        private void SendFileToAnalyze()
        {
            List<string> lines = FileManager.ReadTextAsset(asset, false);

            foreach (string line in lines) 
            {
                if(line == string.Empty) continue;

                DIALOGUE_LINE dl = DialogueAnalyzer.Analyze(line);
            }
        }
    }
}
