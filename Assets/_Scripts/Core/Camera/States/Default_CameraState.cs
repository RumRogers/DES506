using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Default_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;

        public Default_CameraState(Automaton owner) : base(owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;
        }
        public override void Manage()
        {
            if(Input.GetKeyDown(KeyCode.F12))
            {
                m_owner.SetState(new Controlling_CameraState(m_owner));
                return;
            }

            if(m_playerMoveCamera.p_SmoothMovement)
            {                
                m_owner.transform.position = Vector3.Lerp(
                    m_owner.transform.position,
                    m_playerMoveCamera.p_CameraTarget.position + m_playerMoveCamera.p_CameraOffset,
                    Time.deltaTime * m_playerMoveCamera.p_LerpSpeed);
            }
            else
            {
                m_playerMoveCamera.transform.position = m_playerMoveCamera.p_CameraTarget.position + m_playerMoveCamera.p_CameraOffset;
            }        
        }

    }
}