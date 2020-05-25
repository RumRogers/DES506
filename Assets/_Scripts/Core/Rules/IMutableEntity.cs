using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Rules
{
    // Each game entity that can be affected by run-time rules will have to implement this interface
    public interface IMutableEntity
    {
        void Is(List<GrammarLexemes.Object> ruleObjects);
        void Can(List<GrammarLexemes.Object> ruleObjects);
        void Has(List<GrammarLexemes.Object> ruleObjects);
    }
}