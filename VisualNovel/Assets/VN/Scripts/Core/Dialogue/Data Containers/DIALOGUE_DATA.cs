using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static Dialogue.DIALOGUE_DATA;
using static UnityEngine.Rendering.HableCurve;

namespace Dialogue
{
    public class DIALOGUE_DATA
    {
        public enum StartSignal
        {
            None = 0,
            C,      // clear
            A,      // append
            WC,     // wait clear
            WA,     // wait append
        }

        public class DialogueSegment
        {
            public string dialogue;
            public StartSignal startSignal;
            public float signalDelay;

            public bool appendText => (startSignal == DIALOGUE_DATA.StartSignal.A || startSignal == DIALOGUE_DATA.StartSignal.WA);
        }

        public List<DialogueSegment> segments;
        private const string segmentIdentifierPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";
        // {c} / {a}
        // {wc n} / {wa n}

        public bool hasDialogue()
        {
            return segments.Count > 0;
        }

        public DIALOGUE_DATA(string rawDialogue)
        {
            segments = RipSegments(rawDialogue);
        }

        private List<DialogueSegment> RipSegments(string rawDialogue)
        {
            List<DialogueSegment> localsegments = new List<DialogueSegment>();

            MatchCollection matches = Regex.Matches(rawDialogue, segmentIdentifierPattern);

            int lastIndex = 0;
            // Find the first or only
            DialogueSegment segment = new DialogueSegment();
            segment.dialogue = (matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index));
            segment.startSignal = StartSignal.None;
            segment.signalDelay = 0;
            localsegments.Add(segment);

            if(matches.Count == 0)
            {
                return localsegments;
            }
            else
            {
                lastIndex = matches[0].Index;
            }

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                segment = new DialogueSegment();

                // get the start signal for the segment 
                string signalMatch = match.Value;
                signalMatch = signalMatch.Substring(1, match.Length - 2);
                string[] signalSplit = signalMatch.Split(' ');

                // ‘å•¶Žš‚É•ÏX
                segment.startSignal = (StartSignal)Enum.Parse(typeof(StartSignal), signalSplit[0].ToUpper());

                // get the signal delay
                if(signalSplit.Length > 1)
                {
                    float.TryParse(signalSplit[1], out segment.signalDelay);
                }

                // get The dialogue for  the segment
                int nextIndex = i + 1 < matches.Count ? matches[i + 1].Index : rawDialogue.Length;
                segment.dialogue = rawDialogue.Substring(lastIndex + match.Length, nextIndex - (lastIndex + match.Length));
                lastIndex = nextIndex;

                localsegments.Add(segment);
            }

            return localsegments;
        }
    }
}