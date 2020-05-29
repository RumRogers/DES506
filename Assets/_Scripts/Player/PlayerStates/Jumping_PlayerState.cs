using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Jumping_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        Vector3 m_velocity = Vector3.zero;

        public Jumping_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)m_owner;

            //Adds velocity based on entity property flags
            if (m_playerEntity.HasProperty(PlayerEntityProperties.JUMP_NORMAL))
                m_playerEntity.Velocity = new Vector3(m_playerEntity.Velocity.x, m_playerEntity.JumpVelocity, m_playerEntity.Velocity.z);

            if (m_playerEntity.HasProperty(PlayerEntityProperties.JUMP_HIGH))
                m_playerEntity.Velocity = new Vector3(m_playerEntity.Velocity.x, m_playerEntity.HighJumpVelocity, m_playerEntity.Velocity.z);

            m_velocity = m_playerEntity.Velocity;

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.JUMPING);
        }

        public override void Manage()
        {
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
            }
        }
    }
}
