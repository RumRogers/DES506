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

            //transition from aiming
            m_spellWheel.SetVisible(false);
            SpellWheel.SetTargetEnchantable(null);
        }

        public override void Manage()
        {
        }
    }
}

