using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCore.Rules
{
    public static class GrammarLexemes
    {
        public enum Subject
        {
            PLAYER, WEATHER, APPLE, TIME
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
    }
}