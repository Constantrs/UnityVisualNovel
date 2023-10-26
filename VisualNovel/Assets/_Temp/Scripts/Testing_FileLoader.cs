using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class Testing_FileLoader : MonoBehaviour
    {
        private string fileName = "testFile_en.txt";

        [SerializeField] private TextAsset asset;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CoReadAsset());
        }

        IEnumerator CoReadFile()
        {
            List<string> lines = FileManager.ReadTextFromFile(fileName, true);

            foreach (var line in lines)
            {
                Debug.Log(line);
            }

            yield return null;
        }

        IEnumerator CoReadAsset()
        {
            List<string> lines = FileManager.ReadTextAsset(asset, false);

            foreach (var line in lines)
            {
                Debug.Log(line);
            }

            yield return null;
        }
    }
}