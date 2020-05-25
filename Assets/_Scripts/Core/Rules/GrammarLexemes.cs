using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCore.Rules
{
    public static class GrammarLexemes
    {
        public enum Subject
        {
            PLAYER, WEATHER, APPLE, TIME, SKY
        }

        public enum Verb
        {
            IS, CAN, HAS
        }

        public enum Object
        {
            JUMP, DARK, RED, FAST, SLOW
        }

        public enum LogicalOperator
        {
            AND
        }

        public static string GetTagFromLexeme(Subject lexeme)
        {
            switch(lexeme)
            {
                case Subject.PLAYER: return "Player";
                case Subject.APPLE: return "Apple";
                case Subject.SKY: return "Sky";
                case Subject.TIME: return "Player";
                case Subject.WEATHER: return "Player";
                default: return null;
            }
        }
    }
}