using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Jumping_PlayerState : GameCore.System.State
    {
        PlayerMovement m_playerMovement;

        Vector3 m_velocity = Vector3.zero;

        public Jumping_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerMovement = (PlayerMovement)owner;

            //Adds velocity based on entity property flags
            if (m_playerMovement.PlayerEntity.HasProperty(PlayerEntityProperties.JUMP_NORMAL))
                m_playerMovement.Velocity = new Vector3(m_playerMovement.Velocity.x, m_playerMovement.JumpVelocity, m_playerMovement.Velocity.z);

            if (m_playerMovement.PlayerEntity.HasProperty(PlayerEntityProperties.JUMP_HIGH))
                m_playerMovement.Velocity = new Vector3(m_playerMovement.Velocity.x, m_playerMovement.HighJumpVelocity, m_playerMovement.Velocity.z);

            m_velocity = m_playerMovement.Velocity;
        }

        public override void Manage()
        {
            //subtracting gravity from upwards velocity until forces equalise
            m_playerMovement.Velocity += (Vector3.down * m_playerMovement.Gravity) * Time.deltaTime;

            //Directional input, slower in mid air
            Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
            Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
            m_playerMovement.Direction = (forwardMovement + rightMovement).normalized;

            if (m_playerMovement.Direction != Vector3.zero)
            {
                m_velocity += (m_playerMovement.Direction * m_playerMovement.AerialAccelleration) * Time.deltaTime;
                m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerMovement.MaxSpeed);
            }

            m_playerMovement.Velocity = new Vector3(m_velocity.x, m_playerMovement.Velocity.y, m_velocity.z);

            if (m_playerMovement.Velocity.y < 0)
            {
                m_owner.SetState(new Falling_PlayerState(m_owner));
            }
        }
    }
}
