using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEnchantables
{
    public class TreePuzzle : Enchantable
    {
        Animator m_animator;
        const string ENLARGE_TRIGGER = "Enlarge";
        const string SHRINK_TRIGGER = "Shrink";

        private void Start()
        {
            m_animator = transform.parent.GetComponent<Animator>();
        }
        protected override void SpellSizeBig(Spell spell)
        {
            m_animator.ResetTrigger(SHRINK_TRIGGER);
            m_animator.SetTrigger(ENLARGE_TRIGGER);
        }

        protected override void SpellSizeSmall(Spell spell)
        {
            m_animator.ResetTrigger(ENLARGE_TRIGGER);
            m_animator.SetTrigger(SHRINK_TRIGGER);
        }
    }
}