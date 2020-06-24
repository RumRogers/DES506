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
        Quaternion startingRot; 

        public Comeback_CameraState(Automaton owner) : base(owner) 
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;
            startingPos = owner.transform.position;
            startingRot = owner.transform.rotation;

            //if we the camera angle is not fixed, then transition to the default state as it handles the transtion
            if(!m_playerMoveCamera.p_FixedDefaultCamera)
            {
                owner.SetState(new Default_CameraState(owner));
            }
        }

        public override void Manage()
        {
            m_playerMoveCamera.transform.position = Vector3.Lerp(startingPos, m_playerMoveCamera.p_CameraTarget.position + m_playerMoveCamera.p_CameraOffset, t);
            m_playerMoveCamera.transform.rotation = Quaternion.Lerp(startingRot, m_playerMoveCamera.p_DefaultRotation, t);
            t += Time.deltaTime * m_playerMoveCamera.p_ComebackSpeed;

            if(t >= 1f)
            {
                m_owner.SetState(new Default_CameraState(m_owner));
            }
        }
    }
}