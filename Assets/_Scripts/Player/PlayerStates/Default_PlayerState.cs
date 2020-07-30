using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Default_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;
        GameCore.Camera.PlayerMoveCamera m_camera;

        bool m_landingAnimationFinished = true;
        Vector3 m_velocity;  //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()
        Vector3 m_lastDirection;
        

        public Default_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_velocity = new Vector3(m_playerEntity.Velocity.x, 0.0f, m_playerEntity.Velocity.z);
            m_lastDirection = m_playerEntity.Direction;

            if (!Camera.main.transform.TryGetComponent<GameCore.Camera.PlayerMoveCamera>(out m_camera))
            {
                Debug.LogError("Cannot get PlayerMoveCamera Component on Main Camera!");
            }

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
            //Storing a reference to this state object to transition back to after a fall
            m_playerEntity.PreviousGroundState = PlayerGroundStates.DEFAULT;

            m_playerEntity.Reticle.gameObject.SetActive(false);
        }

        public override void Manage()
        {
            
        }

        public override void FixedManage()
        {
            if (m_landingAnimationFinished)
            {
                if (m_playerEntity.HasProperty(PlayerEntityProperties.PLAYABLE))
                {
                    //Slope detection 
                    //if the slope is climable, modify the direction the player is traveling in 
                    float slopeAngle = Vector3.Angle(m_playerEntity.GroundHitInfo.normal, Vector3.up);
                    if (slopeAngle < m_playerEntity.MaxClimableAngle && slopeAngle > 0)
                    {
                        Vector3 slopeDirection = Vector3.zero;
                        if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
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

                    //If there is input, add velocity in that direction, but clamp the movement speed, Y velocity should always equal zero in this state, so no need to preserve down / up velocity
                    if (m_playerEntity.Direction != Vector3.zero)
                    {
                        float maxSpeed = 0;

                        m_playerEntity.transform.rotation = Quaternion.LookRotation(new Vector3(m_playerEntity.Direction.normalized.x, 0, m_playerEntity.Direction.z));
                        //UNCOMMENT BLOCK TO ENABLE TURNING ANIMATION
                        //if rotation is more than 90 degrees play the turn around animation
                        //if (Vector3.Angle(m_playerEntity.Direction, m_lastDirection) > 30)
                        //{
                        //    //if cross of x1y2 is less than x2y1 then the rotation was counter clockwise, therefore play the left turn animation
                        //    if (m_playerEntity.Direction.x * m_lastDirection.z < m_playerEntity.Direction.z * m_lastDirection.x)
                        //    {
                        //        m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.LEFT_TURN);
                        //    }
                        //    else
                        //    {
                        //        m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.RIGHT_TURN);
                        //    }
                        //}

                        //rotate the player to face the direction they are traveling in

                        if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                        {
                            maxSpeed = m_playerEntity.IceMaxSpeed;
                            m_velocity += (m_playerEntity.Direction * m_playerEntity.IceAcceleration) * Time.fixedDeltaTime;
                            m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerEntity.IceMaxSpeed);
                        }
                        else
                        {
                            maxSpeed = m_playerEntity.MaxSpeed;
                            m_velocity = m_velocity.magnitude * m_playerEntity.Direction;
                            m_velocity += (m_playerEntity.Direction * m_playerEntity.WalkingAcceleration) * Time.fixedDeltaTime;
                        }

                        m_velocity = Vector3.ClampMagnitude(m_velocity, maxSpeed);

                        //setting animation state based on velocity
                        if (m_velocity.magnitude < maxSpeed)
                        {
                            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.WALKING);
                        }
                        else
                        {
                            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.RUNNING);
                        }

                    }
                    else if (Mathf.Abs(m_velocity.x - m_playerEntity.GroundAddedVelocity.x) > 0.1 || Mathf.Abs(m_velocity.z - m_playerEntity.GroundAddedVelocity.z) > 0.1)
                    {
                        if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                        {
                            m_velocity += (m_velocity * -1) * m_playerEntity.IceDeceleration * Time.fixedDeltaTime;
                        }
                        else
                        {
                            m_velocity += (m_velocity * -1) * m_playerEntity.WalkingDeceleration * Time.fixedDeltaTime;
                        }
                        if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                        {
                            if (m_playerEntity.Velocity.magnitude / m_playerEntity.IceMaxSpeed < m_playerEntity.SlidingAnimationSpeedThreshold)
                            {
                                m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.SLIDING);
                            }
                        }
                        else
                        {
                            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
                        }
                    }
                    else
                    {
                        m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
                        m_velocity.x = 0.0f;
                        m_velocity.z = 0.0f;
                    }
                }
                else
                {
                    m_velocity.x = 0;
                    m_velocity.z = 0;
                }

                m_lastDirection = m_playerEntity.Direction;
                m_playerEntity.Velocity = m_velocity;

            }
        }

        IEnumerator WaitForLandingAnimation()
        {
            m_landingAnimationFinished = false;
            while (m_playerEntity.Animator.GetState().GetType() == typeof(JumpEnd_AnimationState))
            {
                yield return null;
            }
            Debug.Log("Finished jump");
            m_landingAnimationFinished = true;
        }
    }
}
