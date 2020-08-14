using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Default_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;
        GameCore.Camera.PlayerMoveCamera m_camera;

        bool m_landingAnimationFinished;
        bool m_turnAnimationFinished;
        Vector3 m_lastDirection;
        Vector3 m_velocity;  //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()
        

        public Default_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_velocity = new Vector3(m_playerEntity.Velocity.x, 0.0f, m_playerEntity.Velocity.z);

            if (!Camera.main.transform.TryGetComponent<GameCore.Camera.PlayerMoveCamera>(out m_camera))
            {
                Debug.LogError("Cannot get PlayerMoveCamera Component on Main Camera!");
            }

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
            //Storing a reference to this state object to transition back to after a fall
            m_playerEntity.PreviousGroundState = PlayerGroundStates.DEFAULT;

            m_playerEntity.Reticle.gameObject.SetActive(false);

            m_landingAnimationFinished = true;
            m_turnAnimationFinished = true;
            LevelManager.ShowEnchantableParticles(false);
            //m_playerEntity.StartCoroutine(WaitForLandingAnimation());
        }

        public override void Manage()
        {
        }

        public override void FixedManage()
        {
            if (m_landingAnimationFinished && m_turnAnimationFinished)
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
                        if (m_playerEntity.EnableTurnAnimations)
                        {
                            //if rotation is more than 90 degrees play the turn around animation
                            if (Vector3.Angle(m_playerEntity.Direction, m_playerEntity.transform.forward) > 170)
                            {
                                m_playerEntity.StopAllCoroutines();

                                //only trigger if the player looks like they are running
                                if (m_velocity.magnitude >= maxSpeed * m_playerEntity.RunningAnimationSpeedThreshold)
                                {
                                    //if cross of x1y2 is less than x2y1 then the rotation was counter clockwise, therefore play the left turn animation
                                    if (m_playerEntity.Direction.x * m_playerEntity.LastDirection.z < m_playerEntity.Direction.z * m_playerEntity.LastDirection.x)
                                    {
                                        m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.LEFT_TURN);
                                    }
                                    else
                                    {
                                        m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.RIGHT_TURN);
                                    }
                                    m_playerEntity.StartCoroutine(WaitForTurnAnimation());
                                    return;
                                }
                                else
                                {
                                    m_playerEntity.StartCoroutine(TurnWithoutAnim(m_playerEntity.WalkingTurnTime));
                                }
                            }
                        }
                    }
                    
                    //rotate the player to face the direction they are traveling in
                    m_playerEntity.Rotation = Quaternion.LookRotation(new Vector3(m_playerEntity.Direction.normalized.x, 0, m_playerEntity.Direction.z));

                    m_velocity = Vector3.ClampMagnitude(m_velocity, maxSpeed);

                    //setting animation state based on velocity
                    if (m_velocity.magnitude / maxSpeed < m_playerEntity.RunningAnimationSpeedThreshold)
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

                        if (m_velocity.magnitude / m_playerEntity.IceMaxSpeed < m_playerEntity.SlidingAnimationSpeedThreshold)
                        {
                            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.SLIDING);
                        }
                    }
                    else
                    {
                        m_velocity += (m_velocity * -1) * m_playerEntity.WalkingDeceleration * Time.fixedDeltaTime;
                        m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
                    }
                }
                else
                {
                    m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
                    m_velocity.x = 0.0f;
                    m_velocity.z = 0.0f;
                }

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

        IEnumerator WaitForTurnAnimation()
        {
            float time = 0;
            m_turnAnimationFinished = false;
            Vector3 rotation = m_playerEntity.transform.rotation.eulerAngles;
            Vector3 endRotation = Quaternion.LookRotation(new Vector3(m_playerEntity.Direction.normalized.x, 0, m_playerEntity.Direction.z)).eulerAngles;
            Vector3 endVelocity = m_playerEntity.Velocity / m_playerEntity.TurnVelocityDevisor;

            while (true)
            {
                time += Time.deltaTime;

                float percomp = time / (m_playerEntity.Animator.TurnRightState.length / m_playerEntity.Animator.TurningAnimSpeed);
                rotation.y = Mathf.Lerp(rotation.y, endRotation.y, percomp);
                m_playerEntity.Velocity = Vector3.Lerp(m_playerEntity.Velocity, endVelocity, percomp);


                m_playerEntity.transform.eulerAngles = rotation;
                m_playerEntity.Rotation = Quaternion.Euler(rotation);

                if (percomp > 0.99)
                {
                    m_turnAnimationFinished = true;
                    yield break;
                }
                yield return null;
            }

        }

        IEnumerator TurnWithoutAnim(float turnTimeLength)
        {
            float time = 0;

            Vector3 rotation = m_playerEntity.transform.rotation.eulerAngles;
            Vector3 endRotation = Quaternion.LookRotation(new Vector3(m_playerEntity.Direction.normalized.x, 0, m_playerEntity.Direction.z)).eulerAngles;

            while (true)
            {
                time += Time.deltaTime;

                //endRotation = Quaternion.LookRotation(new Vector3(m_playerEntity.Direction.normalized.x, 0, m_playerEntity.Direction.z)).eulerAngles;

                float percomp = time / turnTimeLength;
                rotation.y = Mathf.Lerp(rotation.y, endRotation.y, percomp);

                m_playerEntity.transform.eulerAngles = rotation;
                m_playerEntity.Rotation = Quaternion.Euler(rotation);
                
                if (percomp > 0.99)
                {
                    yield break;
                }

                yield return null;
            }
        }
    }
}
