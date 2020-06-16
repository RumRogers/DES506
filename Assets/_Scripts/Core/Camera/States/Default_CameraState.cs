using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Default_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;
        UnityEngine.Camera m_camera;
        //Variables for if design wants camera rotation like third person
        Vector3 m_rotation;
        Vector3 m_offset;

        Vector3 m_transitionStart;
        Vector3 m_transitionEnd;
        bool m_transitioned = false;

        float m_defaultFOV = 50;
        float m_startFOV;
        
        public Default_CameraState(Automaton owner) : base(owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;

            if(!m_playerMoveCamera.p_FixedDefaultCamera)
            {
                m_transitionStart = m_playerMoveCamera.transform.position;
                m_rotation = m_playerMoveCamera.transform.eulerAngles;
                m_rotation.x = m_playerMoveCamera.p_DefaultStartingAngle;

                m_offset = (m_playerMoveCamera.transform.up) * 1.5f;
                m_transitionEnd = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_DefaultDistance) - m_offset);

                if (!m_playerMoveCamera.TryGetComponent<UnityEngine.Camera>(out m_camera))
                {
                    Debug.LogError("Camera component not found! Camera movement script is not attached to a Camera!");
                }
                m_startFOV = m_camera.fieldOfView;

                //start coroutine
                m_playerMoveCamera.StopAllCoroutines();
                m_playerMoveCamera.StartCoroutine(Transition());
            }
            else
            {
                m_transitioned = true;
            }
        }

        public override void Manage()
        {
            if (m_transitioned)
            {
                //I never lose because I'm playing everyside B-)
                //Code for if design prefered the fixed perspective
                if (m_playerMoveCamera.p_FixedDefaultCamera)
                {
                    if (Input.GetKeyDown(KeyCode.F12))
                    {
                        m_owner.SetState(new Controlling_CameraState(m_owner));
                        return;
                    }

                    if (m_playerMoveCamera.p_SmoothMovement)
                    {
                        m_owner.transform.position = Vector3.Lerp(
                            m_owner.transform.position,
                            m_playerMoveCamera.p_CameraTarget.position + m_playerMoveCamera.p_CameraOffset,
                            Time.deltaTime * m_playerMoveCamera.p_LerpSpeed);
                    }
                    else
                    {
                        m_playerMoveCamera.transform.position = m_playerMoveCamera.p_CameraTarget.position + m_playerMoveCamera.p_CameraOffset;
                    }
                }
                //Code for if design wants to be able to rotate the camera
                else
                {
                    //m_rotation.x =  Mathf.Clamp(m_rotation.x - (Input.GetAxis("Camera Y") * m_playerMoveCamera.p_MovementSpeed), m_playerMoveCamera.p_DefaultStartingAngle - m_playerMoveCamera.p_DefaultMaxAngle, m_playerMoveCamera.p_DefaultStartingAngle + m_playerMoveCamera.p_DefaultMaxAngle);
                    m_rotation.x = m_playerMoveCamera.p_DefaultStartingAngle;
                    m_rotation.y += Input.GetAxis("Camera X") * m_playerMoveCamera.p_MovementSpeed;

                    m_playerMoveCamera.transform.eulerAngles = m_rotation;

                    m_offset = (m_playerMoveCamera.transform.up) * 1.5f;

                    Vector3 targetPosition = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_DefaultDistance) - m_offset);

                    m_playerMoveCamera.transform.position = targetPosition;
                }
            }

        }


        IEnumerator Transition()
        {
            float time = 0;
            while (true)
            {
                m_offset = (m_playerMoveCamera.transform.up) * 1.5f;
                m_transitionEnd = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_DefaultDistance) - m_offset);
                m_playerMoveCamera.transform.position = Vector3.Lerp(m_transitionStart, m_transitionEnd, time);
                m_playerMoveCamera.transform.rotation = Quaternion.Lerp(m_playerMoveCamera.transform.rotation, Quaternion.Euler(m_rotation), time);

                m_camera.fieldOfView = Mathf.Lerp(m_startFOV, m_defaultFOV, time);

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