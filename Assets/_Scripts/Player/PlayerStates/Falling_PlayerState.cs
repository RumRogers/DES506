using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Falling_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        Vector3 m_velocity; //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()

        //Just an empty one to get rid of an error
        RFX4_EffectEvent temp;
        public Falling_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_velocity = m_playerEntity.Velocity;

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.FALLING);
            LevelManager.ForceSpellWheelClose();
        }

        public override void Manage()
        {
            
        }

        public override void FixedManage()
        {
            m_velocity += (Vector3.down * m_playerEntity.Gravity * 2) * Time.deltaTime;

            //if grounded transition to previous grounded state, or if there wasn't one, transition to the default state
            if (m_playerEntity.Grounded)
            {
                m_playerEntity.CanJumpTimer = 0;    //resetting the can jump timer when we land for the next jump
                m_playerEntity.HasJumped = false;

                //if the player is still holding a direction we can assume that they did intend to keep moving
                if (m_playerEntity.MovementInput == Vector2.zero)
                {
                    m_playerEntity.Velocity = Vector3.zero;
                }
                else
                {
                    m_playerEntity.Velocity = new Vector3(m_playerEntity.Velocity.x, 0, m_playerEntity.Velocity.z);
                }
                
                switch (m_playerEntity.PreviousGroundState)
                {
                    case PlayerGroundStates.AIMING:
                        m_playerEntity.SetState(new Aiming_PlayerState(m_playerEntity, temp));
                        break;
                    case PlayerGroundStates.DEFAULT:
                        m_playerEntity.SetState(new Default_PlayerState(m_playerEntity));
                        break;
                    default:
                        m_playerEntity.SetState(new Default_PlayerState(m_playerEntity));
                        break;
                }
                return;
            }            

            //if there is an input, move in that direction, but clamp the magnitude (speed) so they cannot exceed max speed while in the air
            if (m_playerEntity.Direction != Vector3.zero)
            {
                Vector3 nonVerticalMovement = new Vector3(m_velocity.x, 0, m_velocity.z) + (m_playerEntity.Direction * m_playerEntity.AerialAcceleration) * Time.fixedDeltaTime;

                m_playerEntity.Rotation = Quaternion.LookRotation(new Vector3(m_playerEntity.Direction.normalized.x, 0, m_playerEntity.Direction.normalized.z));
                if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                {
                    m_velocity = m_velocity.y * Vector3.up + Vector3.ClampMagnitude(nonVerticalMovement, m_playerEntity.IceMaxSpeed); //clamped at a higher value as we want to be able to jump further while sliding
                }
                else
                {
                    m_velocity = m_velocity.y * Vector3.up + Vector3.ClampMagnitude(nonVerticalMovement, m_playerEntity.MaxSpeed);
                }
            }
            else if (Mathf.Abs(m_velocity.x) > 0.1f || Mathf.Abs(m_velocity.z) > 0.1f)
            {
                m_velocity += (((m_velocity.normalized) * -1) * m_playerEntity.AerialDeceleration) * Time.fixedDeltaTime;
            }

            m_playerEntity.Velocity = m_velocity;
        }
    }
}
