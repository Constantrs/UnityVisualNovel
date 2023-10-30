using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Dialogue
{

    public class SPEAKER_DATA
    {
        public string name;
        public string castName;
        public string displayName => (castName != string.Empty) ? castName : name;
        public Vector2 castPosition;

        public List<(int layer, string expression)> CastExpression { get; set; }

        private const string NAMECAST_ID = " as ";
        private const string POSITIONCAST_ID = " at ";
        private const string EXPRESSIONCAST_ID = @" [";
        private const char AXISDELIMITER = ':';
        private const char EXPRESSIONLAYER_JOINER = ',';
        private const char EXPRESSIONLAYER_DELIIMTER = ':';

        public SPEAKER_DATA(string rawSpeaker)
        {
            string pattern = @$"{NAMECAST_ID}|{POSITIONCAST_ID}|{EXPRESSIONCAST_ID.Insert(EXPRESSIONCAST_ID.Length - 1, @"\")}";

            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            castName = "";
            castPosition = Vector2.zero;
            CastExpression = new List<(int layer, string expression)>();

            if (matches.Count == 0)
            {
                name = rawSpeaker;
                return;
            }

            int index = matches[0].Index;

            name = rawSpeaker.Substring(0, index);
            castName = rawSpeaker.Substring(index + 1);

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                int startIndex = 0;
                int endIndex = 0;

                if(match.Value == NAMECAST_ID)
                {
                    startIndex = match.Index + NAMECAST_ID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;
                    castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }
                else if (match.Value == POSITIONCAST_ID)
                {
                    startIndex = match.Index + POSITIONCAST_ID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;
                    string castPos = rawSpeaker.Substring(startIndex, endIndex - startIndex);

                    string[] axis = castPos.Split(AXISDELIMITER, System.StringSplitOptions.RemoveEmptyEntries);

                    float.TryParse(axis[0], out castPosition.x);

                    if(axis.Length > 1) 
                    {
                        float.TryParse(axis[1], out castPosition.x);
                    }
                }
                else if (match.Value == EXPRESSIONCAST_ID)
                {
                    startIndex = match.Index + EXPRESSIONCAST_ID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;
                    string castExp = rawSpeaker.Substring(startIndex, endIndex - (startIndex + 1));

                    CastExpression = castExp.Split(EXPRESSIONLAYER_JOINER)
                        .Select(x =>
                        {
                            var parts = x.Trim().Split(EXPRESSIONLAYER_DELIIMTER);
                            return (int.Parse(parts[0]), parts[1]);
                        }).ToList();
                }
            }
        }
    }
}
