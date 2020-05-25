using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Rules
{
    // It's not allowed to inherit from RuleChunk
    public sealed class RuleChunk
    {
        public enum ChunkType
        {
            SUBJECT, VERB, OBJECT, LOGICAL_OP
        }

        public RuleChunk(ChunkType type, string lexeme)
        {
            m_chunkType = type;
            m_lexeme = lexeme;
        }

        // Declared as readonly so that they get defined in the constructor and then become constant
        public readonly ChunkType m_chunkType;
        public readonly string m_lexeme;
    }
}