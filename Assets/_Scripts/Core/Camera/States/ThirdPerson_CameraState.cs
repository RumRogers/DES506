using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class ThirdPerson_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;
        UnityEngine.Camera m_camera;
        Vector3 m_rotation;
        Vector3 m_offset;

        bool m_transitioned = false;
        Vector3 m_startingPos;
        Vector3 m_endingPos;

        float m_aimFOV = 45;
        float m_startFOV;

        public ThirdPerson_CameraState(Automaton owner) : base (owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;

            if (!m_playerMoveCamera.TryGetComponent<UnityEngine.Camera>(out m_camera))
            {
                Debug.LogError("Camera component not found! Camera movement script is not attached to a Camera!");
            }
            m_startFOV = m_camera.fieldOfView;

            m_rotation = m_playerMoveCamera.transform.eulerAngles;

            //Getting initial start and offset
            m_startingPos = m_playerMoveCamera.transform.position;
            m_offset = (m_playerMoveCamera.transform.right * m_playerMoveCamera.p_AimingOffset.x) + (m_playerMoveCamera.transform.up * m_playerMoveCamera.p_AimingOffset.y);

            //Camera transition between current position and expected distance without changing angle.
            m_playerMoveCamera.StopAllCoroutines();
            m_playerMoveCamera.StartCoroutine(Transition());
        }

        //Gets mouse input and rotates around the player by making the camera face the player, the subtracting it's forward vector by the desired distance
        public override void Manage()
        {
            m_rotation.x = Mathf.Clamp(m_rotation.x - (Input.GetAxis("Camera Y") * m_playerMoveCamera.p_AimingMovementSpeed), m_playerMoveCamera.p_AimingMinAngle, m_playerMoveCamera.p_AimingMaxAngle);
            m_rotation.y += Input.GetAxis("Camera X") * m_playerMoveCamera.p_AimingMovementSpeed;

            m_playerMoveCamera.transform.eulerAngles = m_rotation;

            m_offset = (m_playerMoveCamera.transform.right * m_playerMoveCamera.p_AimingOffset.x) + (m_playerMoveCamera.transform.up * m_playerMoveCamera.p_AimingOffset.y);

            Vector3 targetPosition;

            if (m_transitioned)
            {
                //after transition move at a fixed distance for two reasons, 1. should be more reliable in cases where the transition lerp might not calculate final distance consistently 2. camera collision detection will move the camera
                targetPosition = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_AimingDistance) - m_offset);
            }
            else
            {
                //if not transitioned, use the current distance rather than the aiming distance, allows the player to move camera while transitioning
                targetPosition = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * (m_playerMoveCamera.transform.position - m_playerMoveCamera.p_CameraTarget.position).magnitude) - m_offset);
            }
            m_playerMoveCamera.transform.position = targetPosition;
        }

        IEnumerator Transition()
        {
            float time = 0;
            while (true)
            {
                m_offset = (m_playerMoveCamera.transform.right * m_playerMoveCamera.p_AimingOffset.x) + (m_playerMoveCamera.transform.up * m_playerMoveCamera.p_AimingOffset.y);    //updating offset as the player may be moving
                m_endingPos = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_AimingDistance) - m_offset);
                m_playerMoveCamera.transform.position = Vector3.Lerp(m_startingPos, m_endingPos, time);

                m_camera.fieldOfView = Mathf.Lerp(m_startFOV, m_aimFOV, time);

                time += Time.deltaTime * m_playerMoveCamera.p_ComebackSpeed;

                if (time > 1)
                {
                    m_transitioned = true;
                    yield break;
                }

                yield return null;
            }
        }
    }
}
