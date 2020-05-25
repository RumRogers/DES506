using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Rules
{
    public sealed class Rule
    {
        public static List<RuleChunk.ChunkType> s_basicRule = new List<RuleChunk.ChunkType>(3)
        {
            RuleChunk.ChunkType.SUBJECT,
            RuleChunk.ChunkType.VERB,
            RuleChunk.ChunkType.OBJECT,
        };
        public static List<RuleChunk.ChunkType> s_complexRuleA = new List<RuleChunk.ChunkType>(5)
        {
            RuleChunk.ChunkType.SUBJECT,
            RuleChunk.ChunkType.VERB,
            RuleChunk.ChunkType.OBJECT,
            RuleChunk.ChunkType.LOGICAL_OP,
            RuleChunk.ChunkType.OBJECT,

        };
        public static List<RuleChunk.ChunkType> s_complexRuleB = new List<RuleChunk.ChunkType>(5)
        {
            RuleChunk.ChunkType.SUBJECT,
            RuleChunk.ChunkType.LOGICAL_OP,
            RuleChunk.ChunkType.SUBJECT,
            RuleChunk.ChunkType.VERB,
            RuleChunk.ChunkType.OBJECT
        };

        // Keyword "params" allows the constructor to be called with a comma-separated list of (zero or more) RuleChunk instances
        public Rule(params RuleChunk[] ruleChunks)
        {
            m_ruleChunks = new List<RuleChunk>(ruleChunks);
        }

        public readonly List<RuleChunk> m_ruleChunks;        
        private RuleDelegate m_ptrToMutables;

        private bool IsValid()
        {
            // Don't even bother to check if the rule is valid if we don't have at least 3 chunks (needed for the basic rule)
            if(m_ruleChunks.Count < s_basicRule.Count)
            {
                return false;
            }

            Stack<RuleChunk> stack = new Stack<RuleChunk>();

            bool valid = false;

            for (int i = 0; i < m_ruleChunks.Count; i++)
            {
                
            }

            return valid;
        }

        /// <summary>
        /// Returns true if the rule chunk at the specified index is of the expected type in the list.
        /// </summary>
        /// <param name="idx">The index being considered in the list of rule chunks.</param>
        /// <param name="type">The type of chunk being expected.</param>
        /// <returns></returns>
        private bool Expect(int idx, RuleChunk.ChunkType type)
        {
            return m_ruleChunks[idx].m_chunkType == type;
        }

    }
}