using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Respawn_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;

        Vector3 m_endPosition;
        Vector3 m_startPosition;


        public Respawn_CameraState(PlayerMoveCamera owner, Vector3 checkpointPosition) : base(owner)
        {
            m_playerMoveCamera = owner;
            m_startPosition = m_playerMoveCamera.transform.position;
            m_endPosition = checkpointPosition;
            m_endPosition -= m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_DefaultDistance;

            m_playerMoveCamera.StopAllCoroutines();
            m_playerMoveCamera.StartCoroutine(Transition());
            m_playerMoveCamera.StartCoroutine(m_playerMoveCamera.FadeToColour(Color.black, 0.2f));
        }

        public override void Manage()
        {
            //do nothing
        }

        IEnumerator Transition()
        {
            float time = 0;

            while (true)
            {
                time += Time.deltaTime * m_playerMoveCamera.p_DefaultLerpSpeed; //should change to respawn lerp speed

                //m_playerMoveCamera.p_Position = Vector3.Lerp(m_startPosition, m_endPosition, time);

                if (time > 0.99f)
                {
                    m_playerMoveCamera.p_Position = m_endPosition;
                    m_playerMoveCamera.StartCoroutine(m_playerMoveCamera.FadeToColour(new Color(0, 0, 0, 0), 0.2f));
                    yield break;
                }

                yield return null;
            }
        }
    }
}