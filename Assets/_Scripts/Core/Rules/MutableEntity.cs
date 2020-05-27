using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Rules
{
    // The class responsible to actually apply rules will use a RuleDelegate to point to the correct function of MutableEntity
    public delegate void RuleDelegate(string lexeme);

    // Each game entity that can be affected by run-time rules will have to inherit from this abstract class
    public abstract class MutableEntity : MonoBehaviour
    {
        public virtual void Is(string lexeme) { Debug.Log($"MutableEntity: called Is({lexeme})"); }
        public virtual void Can(string lexeme) { Debug.Log($"MutableEntity: called Can({lexeme})"); }
        public virtual void Has(string lexeme) { Debug.Log($"MutableEntity: called Has({lexeme})"); }
        public virtual void UndoIs(string lexeme) { Debug.Log($"MutableEntity: called UndoIs({lexeme})"); }
        public virtual void UndoCan(string lexeme) { Debug.Log($"MutableEntity: called UndoCan({lexeme})"); }
        public virtual void UndoHas(string lexeme) { Debug.Log($"MutableEntity: called UndoHas({lexeme})"); }
    }
}
