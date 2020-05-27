using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Default_PlayerState : GameCore.System.State
    {
        PlayerMovement m_playerMovement;

        Vector3 m_velocity;  //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()

        public Default_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerMovement = (PlayerMovement)owner;
            m_velocity = new Vector3(m_playerMovement.Velocity.x, 0.0f, m_playerMovement.Velocity.z);
        }

        public override void Manage()
        {
            //Jumping / falling state transitions
            if (m_playerMovement.IsGrounded())
            {
                if (Input.GetButtonDown("Jump") && m_playerMovement.PlayerEntity.HasProperty(PlayerEntityProperties.CAN_JUMP))
                {
                    m_owner.SetState(new Jumping_PlayerState(m_owner));
                    return;
                }
            }
            else
            {
                m_owner.SetState(new Falling_PlayerState(m_owner));
                return;
            }

            //Pushing state transition, gets closest interactable and switches state if there are interactables in range 
            if (Input.GetButtonDown("Interact"))
            {
                if (m_playerMovement.InteractablesInRange.Count > 0)
                {
                    float closestDistance = (m_playerMovement.InteractablesInRange[0].position - m_playerMovement.transform.position).magnitude;
                    m_playerMovement.ClosestInteractable = m_playerMovement.InteractablesInRange[0];
                    foreach (Transform t in m_playerMovement.InteractablesInRange)
                    {
                        float distance = (t.position - m_playerMovement.transform.position).magnitude;
                        if (distance < closestDistance)
                        {
                            m_playerMovement.ClosestInteractable = t;
                            closestDistance = distance;
                        }
                    }
                    m_owner.SetState(new Pushing_PlayerState(m_owner));
                }
            }

            //Directional input
            Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
            Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
            m_playerMovement.Direction = (forwardMovement + rightMovement).normalized;

            //Slope detection 
            //if the slope is climable, modify the direction the player is traveling in 
            if (Vector3.Angle(m_playerMovement.GroundHitInfo.normal, m_playerMovement.transform.up) < m_playerMovement.MaxClimableAngle)
            {
                //get the players right based on direction of movement, then use it to calculate the new direction of travel
                Vector3 playerRight = Vector3.Cross(m_playerMovement.Direction, -m_playerMovement.transform.up);
                //getting the slope angle for the ground the player is walking on
                Vector3 slopeDirection = Vector3.Cross(playerRight, m_playerMovement.GroundHitInfo.normal);

                m_playerMovement.Direction = slopeDirection;
            }

            //Overlap recovery (ground)
            if (m_playerMovement.GroundHitInfo.distance < (m_playerMovement.PlayerCollider.bounds.extents.y) - m_playerMovement.GroundOverlapPadding)
            {
                float lerpedY = Mathf.Lerp(m_playerMovement.transform.position.y, m_playerMovement.transform.position.y + 1, Time.deltaTime);
                m_playerMovement.transform.position = new Vector3(m_playerMovement.transform.position.x, lerpedY, m_playerMovement.transform.position.z);
            }

            //If there is input, add velocity in that direction, but clamp the movement speed, Y velocity should always equal zero in this state, so no need to preserve down / up velocity
            if (m_playerMovement.Direction != Vector3.zero)
            {
                m_velocity += (m_playerMovement.Direction * m_playerMovement.WalkingAcceleration) * Time.deltaTime;
                m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerMovement.MaxSpeed);
            }
            else if (Mathf.Abs(m_velocity.x) > 0.3f || Mathf.Abs(m_velocity.z) > 0.3f)
            {
                m_velocity += (((m_velocity.normalized) * -1) * m_playerMovement.WalkingDeceleration) * Time.deltaTime;
            }
            else
            {
                m_velocity.x = 0.0f;
                m_velocity.z = 0.0f;
            }

            m_playerMovement.Velocity = m_velocity;
        }
    }
}
