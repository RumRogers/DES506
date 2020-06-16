using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class Aiming_SpellWheelState : State
    {
        SpellWheel m_spellWheel;

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
            else if(Input.mouseScrollDelta.y != 0f)
            {
                m_spellWheel.AimAtSlot(m_spellWheel.p_TargetSlot - (int)Input.mouseScrollDelta.y);
            }
        }
    }
}

