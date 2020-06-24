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

        public Jumping_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)m_owner;

            //Adds velocity based on entity property flags
            if (m_playerEntity.HasProperty(PlayerEntityProperties.JUMP_NORMAL))
                m_playerEntity.Velocity = new Vector3(m_playerEntity.Velocity.x, m_playerEntity.JumpVelocity, m_playerEntity.Velocity.z);

            if (m_playerEntity.HasProperty(PlayerEntityProperties.JUMP_HIGH))
                m_playerEntity.Velocity = new Vector3(m_playerEntity.Velocity.x, m_playerEntity.HighJumpVelocity, m_playerEntity.Velocity.z);

            m_additionalJumpForce = m_playerEntity.Velocity.y * m_playerEntity.JumpHeldMutliplier;

            m_velocity = m_playerEntity.Velocity;

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.JUMPING);
        }

        public override void Manage()
        {
            if (Input.GetButton("Jump") && m_time < m_maxButtonHoldTime)
            {
                //adding additional jump force gained by holding the jump button
                m_playerEntity.Velocity += (Vector3.up * m_additionalJumpForce) * Time.deltaTime;
                m_time += Time.deltaTime;
            }
            else if (Input.GetButtonUp("Jump") || m_time > m_maxButtonHoldTime)
            {
                m_time = m_maxButtonHoldTime;// setting it to max time so you cannot press jump again to gain height once you've already released it
            }

            //subtracting gravity from upwards velocity until forces equalise
            m_playerEntity.Velocity += (Vector3.down * m_playerEntity.Gravity) * Time.deltaTime;

            //Directional input, slower in mid air
            Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
            Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
            m_playerEntity.Direction = (forwardMovement + rightMovement).normalized;

            if (m_playerEntity.Direction != Vector3.zero)
            {
                m_velocity += (m_playerEntity.Direction * m_playerEntity.AerialAccelleration) * Time.deltaTime;
                m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerEntity.MaxSpeed);
            }

            m_playerEntity.Velocity = new Vector3(m_velocity.x, m_playerEntity.Velocity.y, m_velocity.z);

            if (m_playerEntity.Velocity.y < 0)
            {
                m_owner.SetState(new Falling_PlayerState(m_owner));
                return;
            }
        }
    }
}
