using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvCharacter;

namespace Testing
{
    public class Testing_Characters : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Test());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Test()
        {
            Character Elen = CharacterManager.instance.CreateCharacter("Elen");

            List<string> list = new List<string>()
            {
                "Hi There!",
                "My Name is Elen.",
                "Waht's your name?",
                "Oh,{wa 1} that`s very nice"
            };
            
            yield return Elen.Say(list);

            Debug.Log("Finish");
        }
    }
}
