using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Dialogue_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;
        GameCore.Camera.PlayerMoveCamera m_playerMoveCamera;
        GameObject m_speaker;

        GameUI.Dialogue.Dialogue m_dialogue;

        public Dialogue_PlayerState(GameCore.System.Automaton owner, GameObject target = null) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_speaker = target;

            if (!m_speaker.TryGetComponent<GameUI.Dialogue.Dialogue>(out m_dialogue))
            {
                Debug.Log("Target has no dialogue script attached");
                m_playerEntity.SetState(new Default_PlayerState(m_playerEntity));
            }

            m_dialogue.ResetDialogue();

            if (!m_speaker)
            {
                float closestDistance = 100;

                foreach (Transform t in m_playerEntity.SpeakersInRange)
                {
                    float distance = (t.position - m_playerEntity.transform.position).magnitude;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        m_speaker = t.gameObject;
                    }
                }
            }

            if (!Camera.main.TryGetComponent<GameCore.Camera.PlayerMoveCamera>(out m_playerMoveCamera))
            {
                Debug.LogError("No PlayerMoveCamera component found on main camera!");
                m_playerEntity.SetState(new Default_PlayerState(m_playerEntity));
            }
            else
            {
                m_playerMoveCamera.SetState(new GameCore.Camera.Dialogue_CameraState(m_playerMoveCamera, m_speaker.transform));
            }

            Vector3 lookDir = m_speaker.transform.position - m_playerEntity.transform.position;
            lookDir.y = 0;
            m_playerEntity.Rotation = Quaternion.LookRotation(lookDir);
            m_playerEntity.Velocity = Vector3.zero;

            m_playerEntity.Animator.SetExpression(PlayerFacialExpression.TALKING);
            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
            Debug.Log("In Dialogue");

            LevelManager.ForceSpellWheelClose();
        }

        public override void Manage()
        {

            if (!m_dialogue.GetDialogueHasStarted())
            {
                m_dialogue.StartDialogue();
                m_playerEntity.ShowOtherUIsInDialogue(false);
            }

           // if (Input.GetButtonDown("Submit"))
            if (Input.GetKeyUp(KeyCode.Return))
            {
                Debug.Log("is it");
                if (!m_dialogue.GetLastLineReached())
                {
                    //advance dialogue here
                    m_dialogue.FillNextLine();
                }
                else
                {
                    m_dialogue.DisableUI();
                    //m_playerEntity.ShowOtherUIsInDialogue(true);
                    m_playerEntity.Animator.SetExpression(PlayerFacialExpression.NATURAL);
                    m_owner.SetState(new Default_PlayerState(m_owner));
                    m_playerMoveCamera.SetState(new GameCore.Camera.Default_CameraState(m_playerMoveCamera));
                }

            }
           // else if (Input.GetButtonDown("Cancel"))
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                m_dialogue.DisableUI();
                //m_playerEntity.ShowOtherUIsInDialogue(true);
                m_playerEntity.Animator.SetExpression(PlayerFacialExpression.NATURAL);
                m_owner.SetState(new Default_PlayerState(m_owner));
                m_playerMoveCamera.SetState(new GameCore.Camera.Default_CameraState(m_playerMoveCamera));
            }
        }
    }
}