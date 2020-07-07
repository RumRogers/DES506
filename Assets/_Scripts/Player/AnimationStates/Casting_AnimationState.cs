using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Casting_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Casting_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)owner;

            m_playerAnimator.Animation.wrapMode = WrapMode.Once;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());
        }

        public override void Manage()
        {
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.RUNNING:
                    m_owner.SetState(new Running_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.JUMPING:
                    m_owner.SetState(new Jumping_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.FALLING:
                    m_owner.SetState(new Falling_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.IDLE:
                    m_owner.SetState(new Idle_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.AIMING:
                    m_owner.SetState(new Aiming_AnimationState(m_owner));
                    break;
            }
        }

        IEnumerator Transition()
        {
            m_playerAnimator.Animation.Play("casting", PlayMode.StopAll);
            while (m_playerAnimator.Animation.isPlaying)
            {
                //do nothing
                yield return null;
            }
            //if it is still casting after it has finished playing, then transition back to aiming as the state has not been changed
            if (m_playerAnimator.PlayerAnimProperties == PlayerAnimationProperties.CASTING)
            {
                m_playerAnimator.SetProperty(PlayerAnimationProperties.AIMING);
            }

            yield break;
        }
    }

}