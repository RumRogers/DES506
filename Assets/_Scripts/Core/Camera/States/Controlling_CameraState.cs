using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Controlling_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;

        public Controlling_CameraState(Automaton owner) : base(owner) 
        {
            // Need to cast down to child class as p_MovementSpeed is a property of PlayerMoveCamera, not Automaton
            m_playerMoveCamera = (PlayerMoveCamera)m_owner;
        }
   
        public override void Manage()
        {            
            if(Input.GetKeyDown(KeyCode.F12))
            {
                m_owner.SetState(new Comeback_CameraState(m_owner));
                return;
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if(h != 0 || v != 0)
            {
                m_playerMoveCamera.transform.position += new Vector3(h, 0, v) * m_playerMoveCamera.p_MovementSpeed * Time.deltaTime;
            }
        }
    }
}