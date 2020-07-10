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
        bool m_transitioned = false;

        Quaternion m_startRotation;
        Quaternion m_endRotation;

        float m_startDistance;
        float m_distance;

        float m_startFOV;

        public Default_CameraState(Automaton owner) : base(owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;

            m_startDistance = (m_playerMoveCamera.p_CameraTarget.position - m_playerMoveCamera.transform.position).magnitude;

            m_startRotation = m_playerMoveCamera.transform.rotation;
            m_endRotation = Quaternion.Euler(new Vector3(m_playerMoveCamera.p_DefaultStartingAngle, m_playerMoveCamera.transform.eulerAngles.y, m_playerMoveCamera.transform.eulerAngles.z));
            m_rotation = m_startRotation.eulerAngles;

            m_offset = (Vector3.up) * 1.5f;

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
            if (m_transitioned)
            {
                m_rotation.x = Mathf.Clamp(m_rotation.x - (Input.GetAxis("Camera Y") * (m_playerMoveCamera.p_DefaultMovementSpeed * Time.deltaTime)), m_playerMoveCamera.p_DefaultStartingAngle + m_playerMoveCamera.p_DefaultMinAngle, m_playerMoveCamera.p_DefaultStartingAngle + m_playerMoveCamera.p_DefaultMaxAngle);
            }
            m_rotation.y = m_rotation.y + Input.GetAxis("Camera X") * (m_playerMoveCamera.p_DefaultMovementSpeed * Time.deltaTime);

            m_playerMoveCamera.transform.rotation = Quaternion.Lerp(m_playerMoveCamera.transform.rotation, Quaternion.Euler( m_rotation), 0.2f);

            //needs clamping else the camera will move further and further away from the player when the button is pressed repeatedly due to the offset being addded
            Vector3 targetPosition = m_playerMoveCamera.p_CameraTarget.position - Vector3.ClampMagnitude((m_playerMoveCamera.transform.forward * m_distance) - m_offset, m_playerMoveCamera.p_DefaultDistance);
            m_playerMoveCamera.p_Position = targetPosition;
        } 


        IEnumerator Transition()
        {
            float time = 0;
            while (true)
            {
                //since camera will move in Manage() all that is needed is to lerp the distance and rotation.x here, the rest takes care of itself 
                m_distance = Mathf.Lerp(m_startDistance, m_playerMoveCamera.p_DefaultDistance, time);   
                m_rotation.x = Quaternion.Lerp(m_startRotation, m_endRotation, time).eulerAngles.x;

                m_camera.fieldOfView = Mathf.Lerp(m_startFOV, m_playerMoveCamera.p_DefaultFOV, time);

                time += Time.deltaTime * m_playerMoveCamera.p_DefaultLerpSpeed;
                time = m_playerMoveCamera.p_LerpCurve.Evaluate(time);

                if (time > 0.99f)
                {
                    m_distance = m_playerMoveCamera.p_DefaultDistance;
                    m_rotation.x = m_endRotation.eulerAngles.x;
                    m_camera.fieldOfView = m_playerMoveCamera.p_DefaultFOV;
                    m_transitioned = true;
                    yield break;
                }

                yield return null;
            }
        }
    }
}