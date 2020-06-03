using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Rules;

namespace GameMutables
{
    public class HidableMutable : MutableEntity
    {
        Renderer m_renderer;

        private void Start()
        {
            m_renderer = GetComponent<Renderer>();
            if(m_renderer == null)
            {
                Destroy(this);
            }
        }

        public override void Is(string lexeme)
        {
            base.Is(lexeme);
            switch (lexeme)
            {
                case "gone":
                    m_renderer.enabled = false;
                    break;                
                default:
                    break;
            }
        }

        public override void UndoIs(string lexeme)
        {
            base.Is(lexeme);
            switch (lexeme)
            {
                case "gone":
                    m_renderer.enabled = true;
                    break;
                default:
                    break;
            }
        }
    }
}
