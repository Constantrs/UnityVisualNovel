using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class CMD_DatabaseExtension_Movement : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("flipCharTest", new Action(FlipCharacter));

            // Add action with no parameters
            database.AddCommand("moveCharTest", new Func<string, IEnumerator>(CoMoveCharacter));
        }

        private static void FlipCharacter()
        {
            Transform character = GameObject.Find("CharacterB").transform;
            float currentDir = MathF.Sign(character.localScale.x);
            character.localScale = new Vector3(-currentDir, character.localScale.y, character.localScale.z);
        }

        private static IEnumerator CoMoveCharacter(string direction)
        {
            bool left = direction.ToLower() == "left";

            Transform character = GameObject.Find("CharacterB").transform;
            float moveSpeed = 15.0f;

            float targetX = left ? -8.0f : 8.0f;

            float currentX = character.position.x;

            while (MathF.Abs(targetX - currentX) > 0.1f)
            {
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.position = new Vector3(currentX, character.position.y, character.position.z);
                yield return null;
            }
        }
    }
}
