using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class Dialogue_CameraState : State
    {
        PlayerMoveCamera m_playerMoveCamera;
        Transform m_speakerTarget;
        Vector3 m_centralPoint; //the point between the player and the speaker
        Vector3 m_startPosition;
        Vector3 m_endPosition;    // the ideal position between the two targets (player and speaker) with camera offset
        Quaternion m_startRotation;
        Quaternion m_endRotation;

        float m_dialogueFOV = 10;

        bool m_transitioned = false;

        public Dialogue_CameraState(Automaton owner, Transform speaker) : base(owner)
        {
            m_playerMoveCamera = (PlayerMoveCamera)owner;
            m_speakerTarget = speaker;

            m_centralPoint = m_speakerTarget.position + ((m_playerMoveCamera.p_CameraTarget.position - m_speakerTarget.position) / 2);

            m_startPosition = m_playerMoveCamera.transform.position;
            m_endPosition = (m_centralPoint - (m_playerMoveCamera.transform.forward * m_playerMoveCamera.p_DialogueDistance));

            m_startRotation = m_playerMoveCamera.transform.rotation;

            Vector3 eularRotation = m_playerMoveCamera.transform.eulerAngles;
            eularRotation.x = m_playerMoveCamera.p_DialogueAngle;
            m_endRotation = Quaternion.Euler(eularRotation);

            m_playerMoveCamera.StopAllCoroutines();
            m_playerMoveCamera.StartCoroutine(Transition());
        }

        public override void Manage()
        {

        }

        IEnumerator Transition()
        {
            float time = 0;
            while(true)
            {
                m_endRotation = Quaternion.LookRotation(m_centralPoint - m_playerMoveCamera.transform.position);

                m_playerMoveCamera.transform.position = Vector3.Lerp(m_startPosition, m_endPosition, time);
                m_playerMoveCamera.transform.rotation = Quaternion.Lerp(m_startRotation, m_endRotation, time);

                time += Time.deltaTime * m_playerMoveCamera.p_ComebackSpeed;
                time = m_playerMoveCamera.p_LerpCurve.Evaluate(time);   //applying motion curve to time

                if (time > 1)
                {
                    yield break;
                }
                yield return null;
            }
        }
    }
}
