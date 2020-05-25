using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Comeback_CameraState : State
    {
        // Need to cast down to child class as p_ComebackSpeed is a property of PlayerMoveCamera, not Automaton
        PlayerMoveCamera m_playerMoveCamera;
        // Used for lerping
        float t = 0f;
        Vector3 startingPos;

        public Comeback_CameraState(Automaton owner) : base(owner) 
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;
            startingPos = owner.transform.position;
        }

        public override void Manage()
        {
            m_playerMoveCamera.transform.position = Vector3.Lerp(startingPos, m_playerMoveCamera.p_CameraTarget.position + m_playerMoveCamera.p_CameraOffset, t);
            t += Time.deltaTime * m_playerMoveCamera.p_ComebackSpeed;

            if(t >= 1f)
            {
                m_owner.SetState(new Default_CameraState(m_owner));
            }
        }
    }
}