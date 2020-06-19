using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameCore.System;

namespace GameCore.Spells
{
    [RequireComponent(typeof(MagicProfile))]
    public abstract class Enchantable : Automaton
    {
        delegate void SpellDelegate(Spell spell);
        public const string s_EnchantableTag = "Enchantable";
        private SpellDelegate m_spellDelegate;
        [SerializeField]
        private MagicProfile m_magicProfile;


        // NOT overridable. DO NOT redeclare this method in child classes. Use Start() or something else. Thank you.
        protected void Awake()
        {
            gameObject.tag = s_EnchantableTag;
            m_magicProfile = GetComponent<MagicProfile>();
        }

        public void CastSpell(Spell spell)
        {

            switch (spell.m_type)
            {
                case SpellType.TRANSFORM_SIZE_BIG:
                case SpellType.TRANSFORM_SIZE_SMALL:
                    m_spellDelegate = SpellSize;                    
                    break;                
                case SpellType.TRANSFORM_TEMPERATURE_HOT:
                case SpellType.TRANSFORM_TEMPERATURE_COLD:
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
            var magicState = m_magicProfile.GetMagicFingerprint().magicState;
            
            if (spell.m_type == SpellType.TRANSFORM_SIZE_BIG && magicState.size != SpellState.SPELLED)
            {
                SpellSizeBig(spell);
                m_magicProfile.SetSizeMagicState(SpellState.SPELLED);
            }
            else if(spell.m_type == SpellType.TRANSFORM_SIZE_SMALL && magicState.size != SpellState.COUNTERSPELLED)
            {
                SpellSizeSmall(spell);
                m_magicProfile.SetSizeMagicState(SpellState.COUNTERSPELLED);
            }
        }

        void SpellTemperature(Spell spell)
        {
            var magicState = m_magicProfile.GetMagicFingerprint().magicState;

            if (spell.m_type == SpellType.TRANSFORM_TEMPERATURE_HOT && magicState.temperature != SpellState.SPELLED)
            {
                SpellTemperatureHot(spell);
                m_magicProfile.SetTemperatureMagicState(SpellState.SPELLED);                
            }
            else if (spell.m_type == SpellType.TRANSFORM_TEMPERATURE_COLD && magicState.temperature != SpellState.COUNTERSPELLED)
            {
                SpellTemperatureCold(spell);
                m_magicProfile.SetTemperatureMagicState(SpellState.COUNTERSPELLED);
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
            var castableSpells = m_magicProfile.GetMagicFingerprint().castableSpells;

            switch(spellType)
            {
                case SpellType.TRANSFORM_SIZE_BIG:
                case SpellType.TRANSFORM_SIZE_SMALL:
                    return castableSpells.sizeSpell;
                case SpellType.TRANSFORM_TEMPERATURE_HOT:
                case SpellType.TRANSFORM_TEMPERATURE_COLD:
                    return castableSpells.temperatureSpell;
                default: 
                    return false;
            }
        }

        public MagicProfile.CastableSpells GetCastableSpells()
        {
            return m_magicProfile.GetMagicFingerprint().castableSpells;
        }

        public MagicProfile.MagicState GetFullMagicState()
        {
            return m_magicProfile.GetMagicFingerprint().magicState;
        }

        public SpellState GetMagicState(SpellType spellType) 
        {
            var magicState = m_magicProfile.GetMagicFingerprint().magicState;

            switch (spellType)
            {
                case SpellType.TRANSFORM_SIZE_BIG:
                case SpellType.TRANSFORM_SIZE_SMALL:
                    return magicState.size;
                case SpellType.TRANSFORM_TEMPERATURE_HOT:
                case SpellType.TRANSFORM_TEMPERATURE_COLD:
                    return magicState.temperature;
                default:
                    throw new UnityException("GetMagicState: invalid spell type!");
            }
        }

        private void ResetMagicState(Spell spell)
        {
            m_magicProfile.ResetMagicState();
        }

        /////// Remove this once Josh's controller is complete and can interact with Enchantable instances //////
        private void OnMouseDown()
        {
            print(this);
            DebugTestSpell.SetHighlighted(gameObject);
            DebugTestSpell.RegisterEnchantable(this);
        }
    }
}

