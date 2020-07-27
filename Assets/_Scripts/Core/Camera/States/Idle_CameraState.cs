using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Idle_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;

        public Idle_CameraState(PlayerMoveCamera owner) : base (owner)
        {
            m_playerMoveCamera = owner;
        }

        public override void Manage()
        {
            
        }

    }
}