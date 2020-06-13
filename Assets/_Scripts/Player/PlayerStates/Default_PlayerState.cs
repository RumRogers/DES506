using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Default_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;
        GameCore.Camera.PlayerMoveCamera m_camera;

        Vector3 m_velocity;  //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()

        public Default_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_velocity = new Vector3(m_playerEntity.Velocity.x, 0.0f, m_playerEntity.Velocity.z);

            if (!Camera.main.transform.TryGetComponent<GameCore.Camera.PlayerMoveCamera>(out m_camera))
            {
                Debug.LogError("Cannot get PlayerMoveCamera Component on Main Camera!");
            }
            m_camera.SetState(new GameCore.Camera.Default_CameraState(m_camera));
            //Storing a reference to this state object to transition back to after a fall
            m_playerEntity.PreviousGroundState = this;
        }

        public override void Manage()
        {
            //Jumping / falling state transitions
            if (m_playerEntity.IsGrounded())
            {
                //need to check if it's playable here before jump as we still want it to enter the falling state if it's not grounded, regardless of it's properties
                if (Input.GetButtonDown("Jump") && m_playerEntity.HasProperty(PlayerEntityProperties.CAN_JUMP) && m_playerEntity.HasProperty(PlayerEntityProperties.PLAYABLE))
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
                if (Input.GetButtonDown("Aim") || Input.GetAxisRaw("Aim") == 1)
                {
                        m_owner.SetState(new Aiming_PlayerState(m_owner)); 
                        return;
                }

                //Directional input
                Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxis("Vertical"); // removing the y component from the camera's forward vector
                Vector3 rightMovement = Camera.main.transform.right * Input.GetAxis("Horizontal");
                m_playerEntity.Direction = (forwardMovement + rightMovement).normalized;

                //Slope detection 
                //if the slope is climable, modify the direction the player is traveling in 
                float slopeAngle = Vector3.Angle(m_playerEntity.GroundHitInfo.normal, Vector3.up);
                if (slopeAngle < m_playerEntity.MaxClimableAngle)
                {
                    Vector3 slopeDirection = Vector3.zero;
                    if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING) && slopeAngle > 1)
                    {                    
                        //get the players right based on direction of movement, then use it to calculate the new direction of travel
                        Vector3 playerRight = Vector3.Cross(m_playerEntity.transform.forward, -m_playerEntity.transform.up);
                        //getting the slope angle for the ground the player is walking on
                        slopeDirection = Vector3.Cross(playerRight, m_playerEntity.GroundHitInfo.normal);
                        if (slopeDirection.y > 0)
                        {
                            slopeDirection *= -1;
                        }
                    }
                    else
                    {
                        //get the players right based on direction of movement, then use it to calculate the new direction of travel
                        Vector3 playerRight = Vector3.Cross(m_playerEntity.Direction, -m_playerEntity.transform.up);
                        //getting the slope angle for the ground the player is walking on
                        slopeDirection = Vector3.Cross(playerRight, m_playerEntity.GroundHitInfo.normal);
                    }

                    m_playerEntity.Direction = slopeDirection;
                }

                //Overlap recovery (ground)
                if (Vector3.Distance(m_playerEntity.transform.position, m_playerEntity.GroundHitInfo.point) < m_playerEntity.PlayerCollider.bounds.extents.y)
                {
                    float targetY = m_playerEntity.GroundHitInfo.point.y + (m_playerEntity.PlayerCollider.bounds.extents.y);
                    float lerpedY = Mathf.Lerp(m_playerEntity.transform.position.y, targetY, Time.deltaTime * 10);
                    m_playerEntity.transform.position = new Vector3(m_playerEntity.transform.position.x, lerpedY, m_playerEntity.transform.position.z);
                }

                //If there is input, add velocity in that direction, but clamp the movement speed, Y velocity should always equal zero in this state, so no need to preserve down / up velocity
                if (m_playerEntity.Direction != Vector3.zero)
                {
                    //rotate the player to face the direction they are traveling in
                    m_playerEntity.transform.rotation = Quaternion.LookRotation(m_playerEntity.Direction);
                    if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                    {
                        m_velocity += (m_playerEntity.Direction * m_playerEntity.IceAcceleration) * Time.deltaTime;
                        m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerEntity.IceMaxSpeed);
                    }
                    else
                    {
                        m_velocity += (m_playerEntity.Direction * m_playerEntity.WalkingAcceleration) * Time.deltaTime;
                        m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerEntity.MaxSpeed);
                    }
                    m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.RUNNING);
                }
                else if (Mathf.Abs(m_velocity.x) > 0.1f || Mathf.Abs(m_velocity.z) > 0.1f)
                {
                    if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                    {
                        m_velocity += (((m_velocity.normalized) * -1) * m_playerEntity.IceDeceleration) * Time.deltaTime;
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
