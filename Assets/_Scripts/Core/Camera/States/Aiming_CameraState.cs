using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Aiming_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;
        UnityEngine.Camera m_camera;
        Vector3 m_offset;
        Vector3 m_rotation;

        bool m_transitioned = false;

        Quaternion m_startRotation;
        Quaternion m_endRotation;

        float m_startFOV;

        float m_startDistance;
        float m_distance;

        public Aiming_CameraState(Automaton owner) : base (owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;

            if (!m_playerMoveCamera.TryGetComponent<UnityEngine.Camera>(out m_camera))
            {
                Debug.LogError("Camera component not found! Camera movement script is not attached to a Camera!");
            }
            m_startFOV = m_camera.fieldOfView;

            m_rotation = m_playerMoveCamera.transform.eulerAngles;

            m_startDistance = (m_playerMoveCamera.p_CameraTarget.position - m_playerMoveCamera.transform.position).magnitude;

            //Camera transition between current position and expected distance without changing angle.
            m_playerMoveCamera.StopAllCoroutines();
            m_playerMoveCamera.StartCoroutine(Transition());
        }

        //Gets mouse input and rotates around the player by making the camera face the player, the subtracting it's forward vector by the desired distance
        public override void Manage()
        {
            Vector2 input = m_playerMoveCamera.p_Input;
            //if what the camera is pointing at is of interest (enchantable)
            if (m_playerMoveCamera.p_AimedAtTransform != null && m_playerMoveCamera.p_AutoAimOn)
            {
                input /= 2; //half the input, potentially expose this to the editor
                //rotation direction for aim assist
                Quaternion rotationDirection = Quaternion.LookRotation((m_playerMoveCamera.p_AimedAtTransform.position - m_playerMoveCamera.transform.position).normalized, m_playerMoveCamera.transform.up);

                Vector3 rotation = Quaternion.RotateTowards(Quaternion.Euler(m_rotation), rotationDirection, Time.fixedDeltaTime * m_playerMoveCamera.p_AutoAimStrength).eulerAngles;

                //kind of a bandaid solution, kinda bummed I couldn't find a better way but quaternions wrap around back to 360 if the angle is too low. Making aiming at things overhead cause issues with clamping rotation later 
                if (rotation.x > m_playerMoveCamera.p_AimingMaxAngle)
                {
                    rotation.x = 0 - (360 - rotation.x);
                }
                m_rotation = rotation;
            }

            m_rotation.x -= input.x * (m_playerMoveCamera.p_AimingMovementSpeed);
            m_rotation.x = Mathf.Clamp(m_rotation.x, m_playerMoveCamera.p_AimingMinAngle, m_playerMoveCamera.p_AimingMaxAngle);

            m_rotation.y += input.y * (m_playerMoveCamera.p_AimingMovementSpeed);


            m_playerMoveCamera.transform.rotation = Quaternion.Lerp(m_playerMoveCamera.transform.rotation, Quaternion.Euler(m_rotation), m_playerMoveCamera.p_SmoothFactor);

            m_offset = (m_playerMoveCamera.transform.right * m_playerMoveCamera.p_AimingOffset.x) + (m_playerMoveCamera.transform.up * m_playerMoveCamera.p_AimingOffset.y);

            Vector3 targetPosition = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_distance) - m_offset);

            m_playerMoveCamera.p_Position = targetPosition;
        }

        IEnumerator Transition()
        {
            float time = 0;
            float distance = (m_playerMoveCamera.transform.position - m_playerMoveCamera.p_CameraTarget.transform.position).magnitude;
            while (true)
            {
                m_distance = Mathf.Lerp(m_startDistance, m_playerMoveCamera.p_AimingDistance, time);
                m_camera.fieldOfView = Mathf.Lerp(m_startFOV, m_playerMoveCamera.p_AimingFOV, time);

                time += Time.deltaTime * m_playerMoveCamera.p_AimingLerpSpeed;
                time = m_playerMoveCamera.p_LerpCurve.Evaluate(time);

                if (time > 0.99f)
                {
                    m_transitioned = true;
                    m_distance = m_playerMoveCamera.p_AimingDistance;
                    m_camera.fieldOfView = m_playerMoveCamera.p_AimingFOV;
                    yield break;
                }
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
