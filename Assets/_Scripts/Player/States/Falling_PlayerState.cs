using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Falling_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        Vector3 m_velocity; //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()

        public Falling_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_velocity = m_playerEntity.Velocity;
        }

        public override void Manage()
        {
            m_playerEntity.Velocity += (Vector3.down * m_playerEntity.Gravity) * Time.deltaTime;

            //if grounded transition to default state
            if (m_playerEntity.IsGrounded())
            {
                m_playerEntity.Velocity = new Vector3(m_playerEntity.Velocity.x, 0, m_playerEntity.Velocity.z);
                m_owner.SetState(new Default_PlayerState(m_owner));
            }

            //Directional input, slower in mid air
            Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
            Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
            m_playerEntity.Direction = (forwardMovement + rightMovement).normalized;
            

            //if there is an input, move in that direction, but clamp the magnitude (speed) so they cannot exceed max speed while in the air
            if (m_playerEntity.Direction != Vector3.zero)
            {
                m_velocity += (m_playerEntity.Direction * m_playerEntity.AerialAccelleration) * Time.deltaTime;
                m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerEntity.MaxSpeed);
            }

            m_playerEntity.Velocity = new Vector3(m_velocity.x, m_playerEntity.Velocity.y, m_velocity.z);
        }
    }
}
