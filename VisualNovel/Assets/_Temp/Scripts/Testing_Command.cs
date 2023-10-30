using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class Testing_Command : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //StartCoroutine(CoRunning());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CommandManager.instance.Exetute("moveCharTest", "left");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CommandManager.instance.Exetute("moveCharTest", "right");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                CommandManager.instance.Exetute("flipCharTest");
            }
        }

        IEnumerator CoRunning()
        {
            yield return CommandManager.instance.Exetute("print");
            yield return CommandManager.instance.Exetute("print_lp", "Hello World");
            yield return CommandManager.instance.Exetute("print_mp", "Line1", "Line2", "Line3");

            yield return CommandManager.instance.Exetute("lambda");
            yield return CommandManager.instance.Exetute("lambda_lp", "Hello Lambda");
            yield return CommandManager.instance.Exetute("lambda_mp", "Lambda1", "Lambda2", "Lambda3");

            yield return CommandManager.instance.Exetute("process");
            yield return CommandManager.instance.Exetute("process_lp", "3");
            yield return CommandManager.instance.Exetute("process_mp", "Process1", "Process2", "Process3");
        }
    }
}
