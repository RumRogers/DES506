using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Default_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        Vector3 m_velocity;  //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()

        public Default_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_velocity = new Vector3(m_playerEntity.Velocity.x, 0.0f, m_playerEntity.Velocity.z);
        }

        public override void Manage()
        {

            //Jumping / falling state transitions
            if (m_playerEntity.IsGrounded())
            {
                //need to check if it's playable here before jump as we still want it to enter the falling state if it's not grounded, regardless of it's properties
                if (Input.GetButtonDown("Jump") && m_playerEntity.HasProperty(PlayerEntityProperties.CAN_JUMP)&& m_playerEntity.HasProperty(PlayerEntityProperties.PLAYABLE))
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
            if (m_playerEntity.HasProperty(PlayerEntityProperties.PLAYABLE))
            {
                //Pushing state transition, gets closest interactable and switches state if there are interactables in range 
                if (Input.GetButtonDown("Interact"))
                {
                    if (m_playerEntity.InteractablesInRange.Count > 0)
                    {
                        float closestDistance = (m_playerEntity.InteractablesInRange[0].position - m_playerEntity.transform.position).magnitude;
                        m_playerEntity.ClosestInteractable = m_playerEntity.InteractablesInRange[0];
                        foreach (Transform t in m_playerEntity.InteractablesInRange)
                        {
                            float distance = (t.position - m_playerEntity.transform.position).magnitude;
                            if (distance < closestDistance)
                            {
                                m_playerEntity.ClosestInteractable = t;
                                closestDistance = distance;
                            }
                        }
                        m_owner.SetState(new Pushing_PlayerState(m_owner));
                        return;
                    }
                }

                //Directional input
                Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
                Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
                m_playerEntity.Direction = (forwardMovement + rightMovement).normalized;

                //Slope detection 
                //if the slope is climable, modify the direction the player is traveling in 
                if (Vector3.Angle(m_playerEntity.GroundHitInfo.normal, m_playerEntity.transform.up) < m_playerEntity.MaxClimableAngle)
                {
                    //get the players right based on direction of movement, then use it to calculate the new direction of travel
                    Vector3 playerRight = Vector3.Cross(m_playerEntity.Direction, -m_playerEntity.transform.up);
                    //getting the slope angle for the ground the player is walking on
                    Vector3 slopeDirection = Vector3.Cross(playerRight, m_playerEntity.GroundHitInfo.normal);

                    m_playerEntity.Direction = slopeDirection;
                }

                //Overlap recovery (ground)
                if (m_playerEntity.GroundHitInfo.distance < (m_playerEntity.PlayerCollider.bounds.extents.y) - m_playerEntity.GroundOverlapPadding)
                {
                    float lerpedY = Mathf.Lerp(m_playerEntity.transform.position.y, m_playerEntity.transform.position.y + 1, Time.deltaTime);
                    m_playerEntity.transform.position = new Vector3(m_playerEntity.transform.position.x, lerpedY, m_playerEntity.transform.position.z);
                }

                //If there is input, add velocity in that direction, but clamp the movement speed, Y velocity should always equal zero in this state, so no need to preserve down / up velocity
                if (m_playerEntity.Direction != Vector3.zero)
                {
                    if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                    {
                        m_velocity += (m_playerEntity.Direction * m_playerEntity.IceAcceleration) * Time.deltaTime;
                    }
                    else
                    {
                        m_velocity += (m_playerEntity.Direction * m_playerEntity.WalkingAcceleration) * Time.deltaTime;
                    }
                    m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerEntity.MaxSpeed);

                    m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.RUNNING);
                }
                else if (Mathf.Abs(m_velocity.x) > 0.3f || Mathf.Abs(m_velocity.z) > 0.3f)
                {
                    if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                    {
                        m_velocity += (m_playerEntity.Direction * m_playerEntity.IceDeceleration) * Time.deltaTime;
                    }
                    else
                    {
                        m_velocity += (((m_velocity.normalized) * -1) * m_playerEntity.WalkingDeceleration) * Time.deltaTime;
                    }
                }
                else
                {
                    m_velocity.x = 0.0f;
                    m_velocity.z = 0.0f;

                    m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
                }
            }

            else
            {
                m_velocity.x = 0;
                m_velocity.z = 0;
            }

            m_playerEntity.Velocity = m_velocity;
        }
    }
}
