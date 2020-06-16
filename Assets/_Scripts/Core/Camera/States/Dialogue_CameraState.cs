using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Dialogue_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;

        public Dialogue_CameraState(Automaton owner) : base(owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;
        }

        public override void Manage()
        {
        }
    }
}
