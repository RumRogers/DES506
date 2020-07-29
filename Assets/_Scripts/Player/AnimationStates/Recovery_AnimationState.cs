using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Recovery_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Recovery_AnimationState(PlayerAnimator owner) : base(owner)
        {
            m_playerAnimator = owner;

            m_playerAnimator.Animation.wrapMode = WrapMode.Once;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());
        }

        public override void Manage()
        {
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.IDLE:
                    m_playerAnimator.SetState(new Idle_AnimationState(m_playerAnimator));
                    break;

            }
        }

        IEnumerator Transition()
        {
            try
            {
                m_playerAnimator.Animation.CrossFade("recovering", 0.2f, PlayMode.StopAll);
                //setting the speed to 0 on the first frame, waiting for the animation to start playing
                m_playerAnimator.RecoveringState.speed = 0;
                m_playerAnimator.StartCoroutine(WaitForRecoverToStart());
            }
            catch
            {
                Debug.LogError("Death Recovering animation not set in editor or is null for some other reason");
            }
            yield return null;
        }

        IEnumerator WaitForRecoverToStart()
        {
            float time = 0;
            while (true)
            {
                time += Time.deltaTime;

                if (time > m_playerAnimator.TimeOnGroundBeforeRecover)
                {
                    //setting the speed to the original full speed after waiting, allowing the animation to play out
                    m_playerAnimator.RecoveringState.speed = m_playerAnimator.RecoverAnimSpeed;
                    yield break;
                }
                yield return null;
            }            
        }
    }
}