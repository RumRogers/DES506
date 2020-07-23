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
            m_playerAnimator.Animation.Play("recovering", PlayMode.StopAll);
            yield return null;
        }
    }
}