using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class FreeFalling_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public FreeFalling_AnimationState(PlayerAnimator owner) : base(owner)
        {
            m_playerAnimator = owner;

            m_playerAnimator.Animation.wrapMode = WrapMode.Loop;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());
            m_playerAnimator.SetExpression(PlayerFacialExpression.SCARED);
        }

        public override void Manage()
        {
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.RECOVERING:
                    m_playerAnimator.SetState(new Recovery_AnimationState(m_playerAnimator));
                    break;

            }
        }

        IEnumerator Transition()
        {
            try
            {
                m_playerAnimator.Animation.Play("freeFalling", PlayMode.StopAll);
            }
            catch
            {
                Debug.LogError("Aiming animation not set in editor or is null for some other reason");
            }
            yield return null;
        }
    }
}