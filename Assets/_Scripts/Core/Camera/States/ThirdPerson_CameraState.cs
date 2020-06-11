using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCore.Camera
{
    public class ThirdPerson_CameraState : GameCore.System.State
    {
        PlayerMoveCamera m_playerMoveCamera;
        Vector3 m_rotation;
        Vector3 m_offset;

        bool m_transitioned = false;
        Vector3 m_startingPos;
        Vector3 m_endingPos;

        public ThirdPerson_CameraState(GameCore.System.Automaton owner) : base (owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;

            m_rotation = m_playerMoveCamera.transform.eulerAngles;

            //Getting initial start and offset
            m_startingPos = m_playerMoveCamera.transform.position;
            m_offset = (m_playerMoveCamera.transform.right * m_playerMoveCamera.p_AimingOffset.x) + (m_playerMoveCamera.transform.up * m_playerMoveCamera.p_AimingOffset.y);

            //Camera transition between current position and expected distance without changing angle.
            m_playerMoveCamera.StartCoroutine(Transition());
        }

        public override void Manage()
        {
            //Only movable after the transition
            if (m_transitioned)
            {
                m_rotation.x = Mathf.Clamp(m_rotation.x - (Input.GetAxis("Mouse Y") * m_playerMoveCamera.p_AimingMovementSpeed), -m_playerMoveCamera.p_AimingMaxAngle, m_playerMoveCamera.p_AimingMaxAngle);
                m_rotation.y += Input.GetAxis("Mouse X") * m_playerMoveCamera.p_AimingMovementSpeed;

                m_playerMoveCamera.transform.eulerAngles = m_rotation;

                m_offset = (m_playerMoveCamera.transform.right * m_playerMoveCamera.p_AimingOffset.x) + (m_playerMoveCamera.transform.up * m_playerMoveCamera.p_AimingOffset.y);

                Vector3 targetPosition = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_AimingDistance) - m_offset);

                m_playerMoveCamera.transform.position = targetPosition;

            }
        }

        IEnumerator Transition()
        {
            float time = 0;
            while (true)
            {
                m_offset = (m_playerMoveCamera.transform.right * m_playerMoveCamera.p_AimingOffset.x) + (m_playerMoveCamera.transform.up * m_playerMoveCamera.p_AimingOffset.y);    //updating offset as the player may be moving
                m_endingPos = m_playerMoveCamera.p_CameraTarget.position - ((m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_AimingDistance) - m_offset);
                m_playerMoveCamera.transform.position = Vector3.Lerp(m_startingPos, m_endingPos, time);

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
