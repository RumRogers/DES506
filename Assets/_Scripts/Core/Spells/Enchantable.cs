using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameCore.System;

namespace GameCore.Spells
{
    public abstract class Enchantable : Automaton
    {
        delegate void SpellDelegate(Spell spell);

        [Serializable]
        protected struct CastableSpells
        {
            public bool sizeSpell;
            public bool temperatureSpell;
        }

        [Serializable]
        protected struct MagicState
        {
            public SpellState size;
            public SpellState temperature;
        }

        public const string s_EnchantableTag = "Enchantable";
        [SerializeField]
        protected CastableSpells m_castableSpells;
        [SerializeField]
        protected MagicState m_magicState;
        private SpellDelegate m_spellDelegate;

        // Programmers, beware! If you decide to override this, you either call base.Awake() OR set the tag manually from the editor!
        // If you don't, the "Enchantable" tag won't be automatically applied to the enchantable game object. You've been warned! :)
        protected virtual void Awake()
        {
            gameObject.tag = s_EnchantableTag;
        }

        public void CastSpell(Spell spell)
        {
            switch (spell.m_type)
            {
                case SpellType.TRANSFORM_SIZE:
                    m_spellDelegate = SpellSize;
                    break;
                case SpellType.TRANSFORM_TEMPERATURE:
                    m_spellDelegate = SpellTemperature;
                    break;
                case SpellType.TRANSFORM_RESET:
                    m_spellDelegate = SpellReset;
                    m_spellDelegate += ResetMagicState;
                    break;
                default:
                    throw new UnityException($"{this} received invalid spell: {spell}");
            }

            Debug.Log($"Casting {spell} onto {this}");
            m_spellDelegate(spell);
        }

        void SpellSize(Spell spell)
        {
            if (m_magicState.size != SpellState.SPELLED)
            {
                SpellSizeBig(spell);
                m_magicState.size = SpellState.SPELLED;
            }
            else
            {
                SpellSizeSmall(spell);
                m_magicState.size = SpellState.COUNTERSPELLED;
            }
        }

        void SpellTemperature(Spell spell)
        {
            if (m_magicState.temperature != SpellState.SPELLED)
            {
                SpellTemperatureHot(spell);
                m_magicState.temperature = SpellState.SPELLED;
            }
            else
            {
                SpellTemperatureCold(spell);
                m_magicState.temperature = SpellState.COUNTERSPELLED;
            }
        }
        protected virtual void SpellSizeBig(Spell spell) { }
        protected virtual void SpellSizeSmall(Spell spell) {  }
        protected virtual void SpellTemperatureHot(Spell spell) { }
        protected virtual void SpellTemperatureCold(Spell spell) { }
        protected virtual void SpellReset(Spell spell) { }

        public override string ToString()
        {
            return $"Enchantable object (name: {gameObject.name})";
        }

        public bool IsCastable(SpellType spellType)
        {
            switch(spellType)
            {
                case SpellType.TRANSFORM_SIZE:
                    return m_castableSpells.sizeSpell;
                case SpellType.TRANSFORM_TEMPERATURE:
                    return m_castableSpells.temperatureSpell;
                default: 
                    return false;
            }
        }

        public SpellState GetMagicState(SpellType spellType) 
        {
            switch(spellType)
            {
                case SpellType.TRANSFORM_SIZE:
                    return m_magicState.size;
                case SpellType.TRANSFORM_TEMPERATURE:
                    return m_magicState.temperature;
                default:
                    throw new UnityException("GetMagicState: invalid spell type!");
            }
        }

        private void ResetMagicState(Spell spell)
        {
            m_magicState.size = m_magicState.temperature = SpellState.NORMAL;
        }

        /////// Remove this once Josh's controller is complete and can interact with Enchantable instances //////
        private void OnMouseDown()
        {
            print(this);
            DebugTestSpell.s_enchantbaleMouseOver = this;
        }
    }
}

