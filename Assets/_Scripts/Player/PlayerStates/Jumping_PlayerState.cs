using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Jumping_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        Vector3 m_velocity = Vector3.zero;
        float m_additionalJumpForce = 10f;
        float m_maxButtonHoldTime = 0.5f;
        float m_time = 0;
        bool m_animationFinished = false;
        float m_entrySpeed = 0;

        public Jumping_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)m_owner;
            //set the y component to 0 as we cannot guarantee that the player wasn't falling or moving downwards on a slope when we enter this state
            m_playerEntity.Velocity = new Vector3(m_playerEntity.Velocity.x, 0, m_playerEntity.Velocity.z);
            //get the speed when entering the state before the jump vel is added. used to store the speed for multiplying in the direction held after jump is finished
            m_entrySpeed = m_playerEntity.Velocity.magnitude;
            //Adds velocity based on entity property flags
            if (m_playerEntity.HasProperty(PlayerEntityProperties.JUMP_NORMAL))
                m_velocity = new Vector3(m_playerEntity.Velocity.x, m_playerEntity.JumpVelocity, m_playerEntity.Velocity.z);

            if (m_playerEntity.HasProperty(PlayerEntityProperties.JUMP_HIGH))
                m_velocity = new Vector3(m_playerEntity.Velocity.x, m_playerEntity.HighJumpVelocity, m_playerEntity.Velocity.z);

            m_additionalJumpForce = m_velocity.y * m_playerEntity.JumpHeldMutliplier;

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.JUMPING);

            m_playerEntity.StopAllCoroutines();
            m_playerEntity.StartCoroutine(WaitForJumpAnimation());
        }

        public override void Manage()
        {            
            if (m_animationFinished)
            {
                //Holding jump to go higher
                if (Input.GetButton("Jump") && m_time < m_maxButtonHoldTime)
                {
                    //adding additional jump force gained by holding the jump button
                    m_velocity += (Vector3.up * m_additionalJumpForce) * Time.fixedDeltaTime;
                    m_time += Time.fixedDeltaTime;
                }
                else if (Input.GetButtonUp("Jump") || m_time > m_maxButtonHoldTime)
                {
                    m_time = m_maxButtonHoldTime;// setting it to max time so you cannot press jump again to gain height once you've already released it
                }

                //subtracting gravity from upwards velocity until forces equalise
                m_velocity += (Vector3.down * m_playerEntity.Gravity) * Time.fixedDeltaTime;

                if (m_playerEntity.Direction != Vector3.zero)
                {
                    Vector3 nonVerticalMovement = new Vector3(m_velocity.x, 0, m_velocity.z) + (m_playerEntity.Direction * m_playerEntity.AerialAcceleration) * Time.fixedDeltaTime;
                    m_playerEntity.transform.rotation = Quaternion.LookRotation(new Vector3(m_playerEntity.Velocity.normalized.x, 0, m_playerEntity.Velocity.normalized.z));
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

                if (m_velocity.y < 0)
                {
                    m_owner.SetState(new Falling_PlayerState(m_owner));
                    return;
                }
            }
        }

        IEnumerator WaitForJumpAnimation()
        {
            while (m_playerEntity.Animator.GetState().GetType() == typeof(Jumping_AnimationState))
            {
                yield return null;
            }
            Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
            Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");

            //if there is some movement, change the direction of the jump. Allows the player to re asses the jump before leaving the ground
            if (forwardMovement + rightMovement != Vector3.zero)
            {
                m_velocity = (Vector3.up * m_velocity.y) + ((forwardMovement + rightMovement).normalized * m_entrySpeed);
            }

            m_animationFinished = true;
        }
    }
}
