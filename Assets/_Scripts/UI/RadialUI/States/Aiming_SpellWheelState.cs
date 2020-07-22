using GameCore.Spells;
using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace GameUI
{
    public class Aiming_SpellWheelState : State
    {
        SpellWheel m_spellWheel;
        Enchantable m_lastTargetEnchantable = null;

        public Aiming_SpellWheelState(SpellWheel owner) : base(owner)
        {
            m_spellWheel = owner;

            //transition from idle
            m_spellWheel.SetVisible(true);
        }

        public override void Manage()
        {
            if(SpellWheel.p_Aiming)
            {
                return;
            }

            if (HasTargetEnchantedChanged())
            {
                OnTargetEnchantedChanged();
            }
            
            if(m_lastTargetEnchantable != null)
            {
                if (Input.mouseScrollDelta.y > 0f)
                {
                    m_spellWheel.AimAtPrevSlot();
                }
                else if(Input.mouseScrollDelta.y < 0f)
                {
                    m_spellWheel.AimAtNextSlot();
                }

                if(Input.GetMouseButtonDown(0))
                {
                    m_spellWheel.CastSelectedSpell();
                    return;
                }
            }
        }

        private bool HasTargetEnchantedChanged()
        {
            return m_lastTargetEnchantable != SpellWheel.GetTargetEnchantable();
        }
        private void OnTargetEnchantedChanged()
        {
            Enchantable newTargetEnchantable = SpellWheel.GetTargetEnchantable();

            m_spellWheel.SetVisible(true, newTargetEnchantable != null);

            if(newTargetEnchantable != null)
            {
                m_spellWheel.PopulateSpellSlots();
                if(m_spellWheel.p_AvailableSlots > 0)
                {
                    m_spellWheel.AimAtFirstAvailableSlot();
                }
                else
                {
                    m_spellWheel.HideSelectionArrow();
                }
            }

            m_lastTargetEnchantable = newTargetEnchantable;
        }
    }
}

