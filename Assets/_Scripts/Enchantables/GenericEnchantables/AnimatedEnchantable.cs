using GameCore.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEnchantables
{
    [System.Serializable]
    struct SpellToTrigger
    {
        public SpellType spellType;
        public string triggerName;
    }

    [RequireComponent(typeof(Animator))]
    public class AnimatedEnchantable : Enchantable
    {
        Animator m_animatorController;
        [SerializeField]
        List<SpellToTrigger> m_spellToTriggerAssociations = new List<SpellToTrigger>();
        Dictionary<SpellType, string> m_mapSpellTypeToTrigger = new Dictionary<SpellType, string>();

        private void Start()
        {
            m_animatorController = GetComponent<Animator>();
            foreach(var entry in m_spellToTriggerAssociations)
            {
                m_mapSpellTypeToTrigger[entry.spellType] = entry.triggerName;
            }
        }

        protected override void SpellSizeBig(Spell spell)
        {
            m_animatorController.SetTrigger(m_mapSpellTypeToTrigger[SpellType.TRANSFORM_SIZE_BIG]);
        }

        protected override void SpellSizeSmall(Spell spell)
        {
            m_animatorController.SetTrigger(m_mapSpellTypeToTrigger[SpellType.TRANSFORM_SIZE_SMALL]);
        }
    }
}