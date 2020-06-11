using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Spells;

namespace GameEnchantables
{
    public class Enchantabletest : Enchantable
    {
        Animator m_animator;
        const string SHRINK_TRIGGER = "Shrink";
        const string NORMAL_TRIGGER = "Normal";
        const string ENLARGE_TRIGGER = "Enlarge";

        // Start is called before the first frame update
        void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        protected override void SpellSizeBig(Spell spell)
        {
            m_animator.SetTrigger(ENLARGE_TRIGGER);
        }

        protected override void SpellSizeSmall(Spell spell)
        {
            m_animator.SetTrigger(SHRINK_TRIGGER);
        }

        protected override void SpellReset(Spell spell)
        {
            m_animator.SetTrigger(NORMAL_TRIGGER);
            base.SpellReset(spell);
        }
    }
}