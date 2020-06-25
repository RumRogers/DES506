using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{

    //TODO: clean up class once a desired behaviour for the camera has been decided on, right now this class is built to do several different behaviours and is unclean
    public class Default_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;
        UnityEngine.Camera m_camera;
        //Variables for if design wants camera rotation like third person
        Vector3 m_rotation;
        Vector3 m_offset;

        //Transition vars
        Vector3 m_startingPos;
        Vector3 m_endingPos;
        Quaternion m_startRotation;
        float m_startDistance;
        bool m_transitioned = false;

        float m_defaultFOV = 50;
        float m_startFOV;

        public Default_CameraState(Automaton owner) : base(owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;

            m_rotation = m_playerMoveCamera.transform.eulerAngles;
            m_rotation.x = m_playerMoveCamera.p_DefaultStartingAngle;

            m_startDistance = (m_playerMoveCamera.p_CameraTarget.position - m_playerMoveCamera.transform.position).magnitude;
            m_startRotation = m_playerMoveCamera.transform.rotation;

            if (!m_playerMoveCamera.TryGetComponent<UnityEngine.Camera>(out m_camera))
            {
                Debug.LogError("Camera component not found! Camera movement script is not attached to a Camera!");
            }
            m_startFOV = m_camera.fieldOfView;

            //start coroutine
            m_playerMoveCamera.StopAllCoroutines();
            m_playerMoveCamera.StartCoroutine(Transition());
        }

        public override void Manage()
        {
            //Togglable for debug / testing purposes, may be changed to make this behaviour hard set
            if (m_playerMoveCamera.m_DefaultCanRotateVertically)
            {
                m_rotation.x = Mathf.Clamp(m_rotation.x - (Input.GetAxis("Camera Y") * m_playerMoveCamera.p_DefaultMovementSpeed), m_playerMoveCamera.p_DefaultStartingAngle + m_playerMoveCamera.p_DefaultMinAngle, m_playerMoveCamera.p_DefaultStartingAngle + m_playerMoveCamera.p_DefaultMaxAngle);
            }
            else
            {
                m_rotation.x = m_playerMoveCamera.p_DefaultStartingAngle;
            }
            m_rotation.y += Input.GetAxis("Camera X") * m_playerMoveCamera.p_DefaultMovementSpeed;

            m_playerMoveCamera.transform.eulerAngles = m_rotation;

            m_offset = (m_playerMoveCamera.transform.up) * 1.5f;

            Vector3 targetPosition;

            if (m_transitioned)
            {
                targetPosition = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_DefaultDistance) - m_offset);
                m_playerMoveCamera.transform.position = targetPosition;
            }
        }


        IEnumerator Transition()
        {
            float time = 0;
            while (true)
            {
                m_offset = (m_playerMoveCamera.transform.up) * 1.5f;
                //actually dependent on the state we just came from... might need to keep track of last state
                m_startingPos = m_playerMoveCamera.p_CameraTarget.position - (m_playerMoveCamera.transform.forward * m_startDistance);
                m_endingPos = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_DefaultDistance) - m_offset);
                m_playerMoveCamera.transform.position = Vector3.Lerp(m_startingPos, m_endingPos, time);
                m_playerMoveCamera.transform.rotation = Quaternion.Lerp(m_startRotation, Quaternion.Euler(m_rotation), time);

                m_camera.fieldOfView = Mathf.Lerp(m_startFOV, m_defaultFOV, time);

                time += Time.deltaTime * m_playerMoveCamera.p_DefaultLerpSpeed;
                time = m_playerMoveCamera.p_LerpCurve.Evaluate(time);

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