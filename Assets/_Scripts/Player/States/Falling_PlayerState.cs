using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Falling_PlayerState : GameCore.System.State
    {
        PlayerMovement m_playerMovement;

        Vector3 m_velocity; //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()

        public Falling_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerMovement = (PlayerMovement)owner;
            m_velocity = m_playerMovement.Velocity;
        }

        public override void Manage()
        {
            m_playerMovement.Velocity += (Vector3.down * m_playerMovement.Gravity) * Time.deltaTime;

            //if grounded transition to default state
            if (m_playerMovement.IsGrounded())
            {
                m_playerMovement.Velocity = new Vector3(m_playerMovement.Velocity.x, 0, m_playerMovement.Velocity.z);
                m_owner.SetState(new Default_PlayerState(m_owner));
            }

            //Directional input, slower in mid air
            Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
            Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
            m_playerMovement.Direction = (forwardMovement + rightMovement).normalized;

            //if there is an input, move in that direction, but clamp the magnitude (speed) so they cannot exceed max speed while in the air
            if (m_playerMovement.Direction != Vector3.zero)
            {
                m_velocity += (m_playerMovement.Direction * m_playerMovement.AerialAccelleration) * Time.deltaTime;
                m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerMovement.MaxSpeed);
            }

            m_playerMovement.Velocity = new Vector3(m_velocity.x, m_playerMovement.Velocity.y, m_velocity.z);
        }
    }
}
