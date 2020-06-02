using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;

namespace GameMutables
{
    public class Level2_Tree : MutableEntity
    {
        [SerializeField]
        Animator m_animator;

        const string BIG = "big";
        const string SHRINK_TRIGGER = "Shrink";
        const string NORMAL_TRIGGER = "Normal";

        // Start is called before the first frame update
        void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        public override void Is(string lexeme)
        {
            if(lexeme.Equals(BIG))
            {
                m_animator.SetTrigger(NORMAL_TRIGGER);
            }
        }
        public override void UndoIs(string lexeme)
        {
            if (lexeme.Equals(BIG))
            {
                m_animator.SetTrigger(SHRINK_TRIGGER);
            }
        }
    }
}