using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Death_PlayerState : GameCore.System.State
    {
        GameCore.Camera.PlayerMoveCamera m_camera;
        PlayerEntity m_playerEntity;
        bool m_animFinished = false;
        bool m_waitingForAnim = false;
        float m_distanceFromGroundBeforeHover = 2;
        bool m_finishedHover = false;
        bool m_hovering = false;
        Vector3 m_respawnPoint;

        public Death_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_respawnPoint = GameCore.System.LevelManager.p_LastCheckpoint.position;

            m_playerEntity = (PlayerEntity)owner;
            m_playerEntity.Velocity = Vector3.zero; //making sure we're actually at 0 velocity for some reason it's not enough to do it in the respawn function

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.FREE_FALLING);

            if (!Camera.main.transform.TryGetComponent(out m_camera))
            {
                Debug.LogError("Cannot get PlayerMoveCamera Component on Main Camera!");
            }

            m_camera.SetState(new GameCore.Camera.Respawn_CameraState(m_camera, m_respawnPoint));

            //start falling
            FMODUnity.RuntimeManager.PlayOneShot(m_playerEntity.FallingAudioEvent, m_playerEntity.transform.position);
        }

        public override void Manage()
        {
            
        }

        public override void FixedManage()
        {
            if (!m_playerEntity.Grounded)
            {
                //Checking to see if they should start hovering
                if (Vector3.Distance(m_playerEntity.transform.position + m_playerEntity.Velocity, m_respawnPoint) < m_distanceFromGroundBeforeHover && !m_finishedHover && !m_hovering)
                {
                    // start hover coroutine
                    FMODUnity.RuntimeManager.PlayOneShot(m_playerEntity.HoveringAudioEvent, m_playerEntity.transform.position);
                    m_playerEntity.Position = m_respawnPoint + Vector3.up * m_distanceFromGroundBeforeHover;
                    m_playerEntity.StartCoroutine(Hover(1.0f, 0.5f , 6.0f));                    
                }

                if (!m_hovering && !m_finishedHover)
                {
                    m_playerEntity.Velocity -= Vector3.up * m_playerEntity.Gravity * Time.fixedDeltaTime;
                }
                else
                {
                    m_playerEntity.Velocity -= Vector3.up * m_playerEntity.Gravity * Time.fixedDeltaTime;
                }

            }
            else if (!m_waitingForAnim)
            {
                m_waitingForAnim = true;
                //hitting ground
                FMODUnity.RuntimeManager.PlayOneShot(m_playerEntity.HitGroundAudioEvent, m_playerEntity.transform.position);
                m_playerEntity.Velocity = Vector3.zero;
                m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.RECOVERING);
                m_playerEntity.Animator.SetExpression(PlayerFacialExpression.NATURAL);
                m_playerEntity.StartCoroutine(WaitForAnimFinish());
            }
        }

        IEnumerator WaitForAnimFinish()
        {
            while (m_playerEntity.Animator.Animation.isPlaying)
            {
                yield return null;
            }

            m_playerEntity.RemoveEntityProperty(PlayerEntityProperties.DYING);
            m_playerEntity.transform.GetChild(0).GetChild(0).transform.localEulerAngles = Vector3.zero;
            m_playerEntity.SetState(new Default_PlayerState(m_playerEntity));
            m_camera.SetState(new GameCore.Camera.Default_CameraState(m_camera));

            m_animFinished = true;
            yield break;
        }

        IEnumerator Decelerate(float timeToDecel)
        {
            float time = 0;
            while(true)
            {
                time += Time.fixedDeltaTime;
                float percomp = time / timeToDecel;

                m_playerEntity.Velocity = Vector3.Lerp(m_playerEntity.Velocity, Vector3.zero, percomp);

                if (time > timeToDecel)
                {
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        IEnumerator Hover(float timeToHover, float bobbingAmount, float bobbingFreqency)
        {
            float time = 0;
            m_playerEntity.Velocity = Vector3.zero;
            
            Vector3 startPosition = m_playerEntity.transform.position;
            m_hovering = true;

            while (true)
            {
                time += Time.deltaTime;

                m_playerEntity.Position = startPosition + ((Vector3.up) * Mathf.Sin(Time.time * bobbingFreqency) * bobbingAmount); //Sin(t * freq) * mag

                if (time > timeToHover)
                {
                    m_finishedHover = true;
                    m_hovering = false;
                    yield break;
                }

                yield return null;
            }

        }
    }
}