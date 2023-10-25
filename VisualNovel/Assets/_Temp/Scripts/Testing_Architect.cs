using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class Testing_Architect : MonoBehaviour
    {
        private DialogueSystem ds;
        private TextArchitect architect;

        public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.instant;

        private string[] lines = new string[5]
        {
            "This is random line of dialogue",
            "I want to say something come over here!",
            "The world is is a carzy place sometimes",
            "Don`t lose hope, things will get better!",
            "It`s a bird? Is`s a plane? No! - It`s Super Sheltie!"
        };

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.GetContainer().dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.typewriter;
        }

        // Update is called once per frame
        void Update()
        {
            string longline = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            if(bm != architect.buildMethod)
            {
                architect.buildMethod = bm;
                architect.Stop();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                architect.Stop();
            }

            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                if (architect.isBuilding)
                {
                    if (!architect.immediatelyText)
                    {
                        architect.immediatelyText = true;
                    }
                    else
                    {
                        architect.ForceComplete();
                    }
                }
                else
                {
                    //architect.Build(lines[Random.Range(0, lines.Length)]);
                    architect.Build(longline);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                //architect.Append(lines[Random.Range(0, lines.Length)]);
                architect.Append(longline);
            }
        }
    }
}
