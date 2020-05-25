using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Rules
{
    // The class responsible to actually apply rules will use a RuleDelegate to point to the correct function of IMutableEntity
    public delegate void RuleDelegate(ref string lexeme);

    // Each game entity that can be affected by run-time rules will have to implement this interface
    public interface IMutableEntity
    {
        void Is(ref string lexeme);
        void Can(ref string lexeme);
        void Has(ref string lexeme);
    }
}