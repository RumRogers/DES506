using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCore.Rules
{
    public static class GrammarLexemes
    {
        public static Dictionary<RuleChunk.ChunkType, List<string>> s_mapOfValidLexemes = new Dictionary<RuleChunk.ChunkType, List<string>>(3)
        {
            { 
                RuleChunk.ChunkType.SUBJECT, new List<string>()
                {
                    "PLAYER", "APPLE", "SKY", "TIME", "WEATHER"
                }
            },
            {
                RuleChunk.ChunkType.VERB, new List<string>()
                {
                    "IS", "CAN", "HAS"
                }
            },
            {
                RuleChunk.ChunkType.OBJECT, new List<string>()
                {
                    "JUMP", "DARK", "RED", "GREEN", "FAST", "SLOW"
                }
            }//,
            /*{
                RuleChunk.ChunkType.LOGICAL_OP, new List<string>()
                {
                    "AND"
                }
            }*/
        };

        public static bool IsValidLexeme(RuleChunk.ChunkType type, string lexeme)
        {
            return s_mapOfValidLexemes[type].Contains(lexeme);
        }

        public static string GetTagFromLexeme(string lexeme)
        {
            if(lexeme.Length == 0)
            {
                return null;
            }

            return char.ToUpper(lexeme[0]) + lexeme.Substring(1).ToLower();
        }
    }
}