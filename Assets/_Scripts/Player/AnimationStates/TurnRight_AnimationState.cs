using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class TurnRight_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public TurnRight_AnimationState(PlayerAnimator owner) : base(owner)
        {
            m_playerAnimator = owner;
            m_playerAnimator.Animation.wrapMode = WrapMode.Once;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());
        }

        public override void Manage()
        {

        }

        IEnumerator Transition()
        {
            m_playerAnimator.Animation.CrossFade("turnRight", 0.2f, PlayMode.StopAll);
            m_playerAnimator.TurningPlaying = true;
            while (m_playerAnimator.Animation.isPlaying)
            {
                yield return null;
            }
            m_playerAnimator.TurningPlaying = false;
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.WALKING:
                    m_playerAnimator.SetState(new Walking_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.RUNNING:
                    m_playerAnimator.SetState(new Running_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.IDLE:
                    m_playerAnimator.SetState(new Idle_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.JUMPING:
                    m_playerAnimator.SetState(new Jumping_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.SLIDING:
                    m_playerAnimator.SetState(new Sliding_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.FALLING:
                    m_playerAnimator.SetState(new Falling_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.FREE_FALLING:
                    m_playerAnimator.SetState(new FreeFalling_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.AIMING:
                    m_playerAnimator.SetState(new Aiming_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.LEFT_TURN:
                    m_playerAnimator.SetState(new TurnLeft_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.RIGHT_TURN:
                    m_playerAnimator.SetState(new TurnRight_AnimationState(m_playerAnimator));
                    break;
            }
        }
    }
}