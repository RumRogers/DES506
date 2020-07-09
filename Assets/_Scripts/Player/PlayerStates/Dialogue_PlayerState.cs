using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Dialogue_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;
        GameCore.Camera.PlayerMoveCamera m_playerMoveCamera;
        Transform m_speaker;

        public Dialogue_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;


            float closestDistance = 100;

            foreach (Transform t in m_playerEntity.SpeakersInRange)
            {
                float distance = (t.position - m_playerEntity.transform.position).magnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    m_speaker = t;
                }
            }

            if (!Camera.main.TryGetComponent<GameCore.Camera.PlayerMoveCamera>(out m_playerMoveCamera))
            {
                Debug.LogError("No PlayerMoveCamera component found on main camera!");
                m_playerEntity.SetState(new Default_PlayerState(m_playerEntity));
            }
            else
            {
                m_playerMoveCamera.SetState(new GameCore.Camera.Dialogue_CameraState(m_playerMoveCamera, m_speaker));
            }

            Vector3 lookDir = m_speaker.position - m_playerEntity.transform.position;
            lookDir.y = 0;
            m_playerEntity.transform.rotation = Quaternion.LookRotation(lookDir);
            m_playerEntity.Velocity = Vector3.zero;

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
            Debug.Log("In Dialogue");
        }

        public override void Manage()
        {
            if (Input.GetButtonDown("Submit"))
            {
                //advance dialogue here
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                m_owner.SetState(new Default_PlayerState(m_owner));
            }
        }
    }
}