using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class ConversationManager
    {
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;

        public bool isRunning => process != null;

        private TextArchitect architect = null;

        private bool userPrompt = false;


        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public void StartConversation(List<string> conversation)
        {
            dialogueSystem.StartCoroutine(CoRunningConversation(conversation));
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator CoRunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                DIALOGUE_LINE line = DialogueAnalyzer.Analyze(conversation[i]);

                if (line.hasDialogue)
                {
                    yield return CoLine_RunDialogue(line);
                }

                if (line.hasCommands)
                {
                    yield return CoLine_RunCommands(line);
                }

                //yield return new WaitForSeconds(1);
            }
        }

        IEnumerator CoLine_RunDialogue(DIALOGUE_LINE line)
        {
            if (line.hasSpeaker)
            {
                dialogueSystem.ShowSpeakerName(line.speaker.displayName);
            }
            //else
            //{
            //    dialogueSystem.HideSpearkerName();
            //}

            yield return CoBuildLineSegmengs(line.dialogue);

            yield return CoWaitForUserInput();
        }

        IEnumerator CoLine_RunCommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.commands);
            yield return null;
        }

        IEnumerator CoBuildLineSegmengs(DIALOGUE_DATA line)
        {
            for (int i = 0; i < line.segments.Count;i++) 
            {
                DIALOGUE_DATA.DialogueSegment segment = line.segments[i];

                yield return WaitForDialogueSegmentToBeTriggered(segment);

                yield return CoBuildDialogue(segment.dialogue , segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentToBeTriggered(DIALOGUE_DATA.DialogueSegment segment)
        {
            switch(segment.startSignal) 
            {
                case DIALOGUE_DATA.StartSignal.C:
                case DIALOGUE_DATA.StartSignal.A:
                    yield return CoWaitForUserInput();
                    break;
                case DIALOGUE_DATA.StartSignal.WC:
                case DIALOGUE_DATA.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
            }
        }

        IEnumerator CoBuildDialogue(string dialogue, bool append = false)
        {
            if(!append)
            {
                architect.Build(dialogue);
            }
            else
            {
                architect.Append(dialogue);
            }

            while (architect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architect.immediatelyText)
                    {
                        architect.immediatelyText = true;
                    }
                    else
                    {
                        architect.ForceComplete();
                    }
                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator CoWaitForUserInput()
        {
            while (!userPrompt)
            {
                yield return null;
            }

            userPrompt = false;
        }
    }
}
