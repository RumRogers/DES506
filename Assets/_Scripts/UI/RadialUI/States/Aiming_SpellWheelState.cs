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
        }

        public override void Manage()
        {
            if(!Input.GetMouseButton(1))
            {
                m_spellWheel.SetState(new Idle_SpellWheelState(m_spellWheel));
            }

            if (HasTargetEnchantedChanged())
            {
                OnTargetEnchantedChanged();
            }
            
            if(m_lastTargetEnchantable != null)
            {
                if (Input.mouseScrollDelta.y != 0f)
                {
                    //m_spellWheel.AimAtSlot(m_spellWheel.p_TargetSlot - (int)Mathf.Sign(Input.mouseScrollDelta.y));
                }
            }
            /*else if(Input.mouseScrollDelta.y != 0f)
            {
                m_spellWheel.AimAtSlot(m_spellWheel.p_TargetSlot - (int)Input.mouseScrollDelta.y);
            }*/
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
                m_spellWheel.AimAtFirstAvailableSlot();
            }

            m_lastTargetEnchantable = newTargetEnchantable;
        }
    }
}

