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
            CommandManager.instance.Exetute("print");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
