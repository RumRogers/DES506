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

            m_endRotation = Quaternion.LookRotation(new Vector3(m_playerMoveCamera.transform.forward.x, 0, m_playerMoveCamera.transform.forward.z));

            m_startRotation = m_playerMoveCamera.transform.rotation;
            m_startDistance = (m_playerMoveCamera.p_CameraTarget.position - m_playerMoveCamera.transform.position).magnitude;

            //Camera transition between current position and expected distance without changing angle.
            m_playerMoveCamera.StopAllCoroutines();
            m_playerMoveCamera.StartCoroutine(Transition());
        }

        //Gets mouse input and rotates around the player by making the camera face the player, the subtracting it's forward vector by the desired distance
        public override void Manage()
        {
            Vector2 input = new Vector2(Input.GetAxis("Camera Y"), Input.GetAxis("Camera X"));
            //if what the camera is pointing at is of interest (enchantable)
            if (m_playerMoveCamera.p_AimedAtTransform != null)
            {
                input /= 2; //half the input, potentially expose this to the editor
                //rotation direction fro aim assist (not normalised yet)
                Quaternion rotationDirection = Quaternion.LookRotation(m_playerMoveCamera.p_AimedAtTransform.position - m_playerMoveCamera.transform.position, Vector3.up);
                m_rotation = Quaternion.RotateTowards(Quaternion.Euler(m_rotation), rotationDirection, Time.deltaTime * m_playerMoveCamera.p_AutoAimStrength).eulerAngles;

            }
            if (m_transitioned)
            {
                m_rotation.x = Mathf.Clamp(m_rotation.x - (input.x * (m_playerMoveCamera.p_AimingMovementSpeed * Time.deltaTime)), m_playerMoveCamera.p_AimingMinAngle, m_playerMoveCamera.p_AimingMaxAngle);
            }
            m_rotation.y += input.y * (m_playerMoveCamera.p_AimingMovementSpeed * Time.deltaTime);

            m_playerMoveCamera.transform.eulerAngles = m_rotation;

            m_offset = (m_playerMoveCamera.transform.right * m_playerMoveCamera.p_AimingOffset.x) + (m_playerMoveCamera.transform.up * m_playerMoveCamera.p_AimingOffset.y);

            Vector3 targetPosition = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_distance) - m_offset);

            m_playerMoveCamera.transform.position = targetPosition;

        }

        IEnumerator Transition()
        {
            float time = 0;
            float distance = (m_playerMoveCamera.transform.position - m_playerMoveCamera.p_CameraTarget.transform.position).magnitude;
            while (true)
            {
                m_distance = Mathf.Lerp(m_startDistance, m_playerMoveCamera.p_AimingDistance, time);
                m_rotation.x = Quaternion.Lerp(m_startRotation, m_endRotation, time).eulerAngles.x;

                m_camera.fieldOfView = Mathf.Lerp(m_startFOV, m_playerMoveCamera.p_AimingFOV, time);

                time += Time.deltaTime * m_playerMoveCamera.p_AimingLerpSpeed;
                time = m_playerMoveCamera.p_LerpCurve.Evaluate(time);

                if (time > 0.99f)
                {
                    m_transitioned = true;
                    m_distance = m_playerMoveCamera.p_AimingDistance;
                    m_rotation.x = m_endRotation.eulerAngles.x;
                    m_camera.fieldOfView = m_playerMoveCamera.p_AimingFOV;
                    yield break;
                }

                yield return null;
            }
        }
    }
}
