using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Rules
{
    // The class responsible to actually apply rules will use a RuleDelegate to point to the correct function of MutableEntity
    public delegate void RuleDelegate(string lexeme);

    // Each game entity that can be affected by run-time rules will have to inherit from this abstract class
    public abstract class MutableEntity : Automaton
    {
        public const string s_MutableTag = "Mutable";

        [SerializeField]
        private string m_reactsToSubject = null;
        public string p_ReactsToSubject { get => m_reactsToSubject; }

        // Programmers, beware! If you decide to override this, you either call base.Awake() OR set the tag manually from the editor!
        // If you don't, the "Mutable" tag won't be automatically applied to the mutable game object. You've been warned! :)
        protected virtual void Awake()
        {
            gameObject.tag = s_MutableTag;
        }

        public virtual void Is(string lexeme) { Debug.Log($"{this}: called Is({lexeme})"); }
        public virtual void Can(string lexeme) { Debug.Log($"{this}: called Can({lexeme})"); }
        public virtual void Has(string lexeme) { Debug.Log($"{this}: called Has({lexeme})"); }
        public virtual void UndoIs(string lexeme) { Debug.Log($"{this}: called UndoIs({lexeme})"); }
        public virtual void UndoCan(string lexeme) { Debug.Log($"{this}: called UndoCan({lexeme})"); }
        public virtual void UndoHas(string lexeme) { Debug.Log($"{this}: called UndoHas({lexeme})"); }
        public override string ToString()
        {
            return $"MutableEntity for GameObject named \"{gameObject.name}\" and tagged \"{gameObject.tag}\"";
        }
    }
}
