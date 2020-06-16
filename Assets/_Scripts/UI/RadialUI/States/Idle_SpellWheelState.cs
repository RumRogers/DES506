using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class Idle_SpellWheelState : State
    {
        SpellWheel m_spellWheel;
        public Idle_SpellWheelState(SpellWheel owner) : base(owner)
        {
            m_spellWheel = owner;
            m_spellWheel.SetVisible(false);
        }

        public override void Manage()
        {
            if (Input.GetMouseButtonDown(1))
            {                
                m_spellWheel.SetState(new Aiming_SpellWheelState(m_spellWheel));
                m_spellWheel.SetVisible(true);
                m_spellWheel.AimAtSlot(0);
            }
        }
    }
}

