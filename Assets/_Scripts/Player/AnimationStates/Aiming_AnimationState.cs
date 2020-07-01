using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Aiming_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Aiming_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)owner;

            m_playerAnimator.Animation.wrapMode = WrapMode.Loop;
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
                case PlayerAnimationProperties.CASTING:
                    m_owner.SetState(new Casting_AnimationState(m_owner));
                    break;
                default:
                    m_owner.SetState(new Idle_AnimationState(m_owner));
                    break;
            }
        }

        IEnumerator Transition()
        {
            m_playerAnimator.Animation.Play("aiming", PlayMode.StopAll);
            yield break;
        }
    }
}