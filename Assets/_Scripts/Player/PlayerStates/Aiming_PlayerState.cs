﻿using GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Aiming_PlayerState : GameCore.System.State
    {

        PlayerEntity m_playerEntity;
        GameCore.Camera.PlayerMoveCamera m_camera;
        Vector3 m_velocity;

        RaycastHit m_rayHitInfo;
        Transform m_aimedAt = null;
        Renderer m_aimedAtRenderer = null;
        Shader m_highlightedOldShader = null;

        public Aiming_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            m_velocity = m_playerEntity.Velocity;
            if (!Camera.main.transform.TryGetComponent<GameCore.Camera.PlayerMoveCamera>(out m_camera))
            {
                Debug.LogError("Cannot get PlayerMoveCamera Component on Main Camera!");
            }
            m_camera.SetState(new GameCore.Camera.ThirdPerson_CameraState(m_camera));
            //Storing a reference to this state object to transition back to after a fall
            m_playerEntity.PreviousGroundState = this;

            //temp
            m_playerEntity.m_reticle.gameObject.SetActive(true);
        }

        public override void Manage()
        {

            //if button "aim" up get out of this state, axis is for joystick trigger buttons
            if (!Input.GetButton("Aim") && Input.GetAxisRaw("Aim") == 0)
            {
                if (m_aimedAtRenderer != null)
                {
                    m_aimedAtRenderer.material.shader = m_highlightedOldShader;
                }

                m_owner.SetState(new Default_PlayerState(m_owner));

                m_playerEntity.m_reticle.gameObject.SetActive(false);                //TEMP REMOVE LATER
                return;
            }

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * m_playerEntity.Projectile.Range);
            //Casting ray forward from the camera to check if there is an enchantable object where the player is aiming
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out m_rayHitInfo, m_playerEntity.Projectile.Range + (m_camera.transform.position - m_playerEntity.transform.position).magnitude))
            {
                if (m_rayHitInfo.transform.tag == "Enchantable")
                {
                    if (m_aimedAt == null || m_aimedAt != m_rayHitInfo.transform)
                    {
                        m_aimedAt = m_rayHitInfo.transform;
                        SpellWheel.SetTargetEnchantable(m_rayHitInfo.transform);
                        if (!m_aimedAt.TryGetComponent<Renderer>(out m_aimedAtRenderer))
                        {
                            Debug.LogError($"Object {m_rayHitInfo.transform.name} doesn't have a renderer component!");
                            return;
                        }
                        m_highlightedOldShader = m_aimedAtRenderer.material.shader;
                        m_aimedAtRenderer.material.shader = m_playerEntity.HighlightShader;
                    }
                }
                //if the hit object is not enchantable and there is a currently selected enchantable, set to null
                else if (m_aimedAt)
                {
                    m_aimedAtRenderer.material.shader = m_highlightedOldShader;
                    m_highlightedOldShader = null;
                    m_aimedAtRenderer = null;
                    m_aimedAt = null;
                    SpellWheel.SetTargetEnchantable(null);
                }
            }
            //not sure about this condition as it is similar to the one above...
            //if it doesn't hit and there is an aimed at transform, set to null also
            else if (m_aimedAt)
            {
                m_aimedAtRenderer.material.shader = m_highlightedOldShader;
                m_highlightedOldShader = null;
                m_aimedAtRenderer = null;
                m_aimedAt = null;
                SpellWheel.SetTargetEnchantable(null);
            }


            if ((Input.GetButtonDown("Fire") || Input.GetAxisRaw("Fire") != 0))
            {
                //casting spell logic to be triggered from within here
                Vector3 direction = Vector3.zero;

                if (m_rayHitInfo.point != Vector3.zero)
                {
                    direction = m_rayHitInfo.point - m_playerEntity.transform.position;
                }
                else
                {
                    direction = (Camera.main.transform.position + (Camera.main.transform.forward * m_playerEntity.Projectile.Range)) - m_playerEntity.transform.position;
                }
                m_playerEntity.Projectile.FireProjectile(direction.normalized, m_playerEntity.transform.position + (m_playerEntity.transform.forward * 2f));
            }

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

            //update movement relative to the camera
            if (m_playerEntity.HasProperty(PlayerEntityProperties.PLAYABLE))
            {
                //Directional input
                Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxis("Vertical"); // removing the y component from the camera's forward vector
                Vector3 rightMovement = Camera.main.transform.right * Input.GetAxis("Horizontal");
                m_playerEntity.Direction = (forwardMovement + rightMovement).normalized;

                //rotate the player to face the direction they are aiming in
                m_playerEntity.transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));

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
                    if (m_playerEntity.HasProperty(PlayerEntityProperties.SLIDING))
                    {
                        m_velocity += (m_playerEntity.Direction * m_playerEntity.IceAcceleration) * Time.deltaTime;
                        m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerEntity.IceMaxSpeed);
                    }
                    else
                    {
                        m_velocity += (m_playerEntity.Direction * m_playerEntity.AimingAcceleration) * Time.deltaTime;
                        m_velocity = Vector3.ClampMagnitude(m_velocity, m_playerEntity.AimingMaxSpeed);
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
                        m_velocity += (((m_velocity.normalized) * -1) * m_playerEntity.AimingDeceleration) * Time.deltaTime;
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