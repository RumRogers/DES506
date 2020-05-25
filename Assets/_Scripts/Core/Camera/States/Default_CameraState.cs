using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Default_CameraState : State
    {
        public Default_CameraState(Automaton owner) : base(owner) { }
        public override void Manage()
        {
            if(Input.GetKeyDown(KeyCode.F12))
            {
                m_owner.SetState(new Controlling_CameraState(m_owner));
            }
        }

    }
}