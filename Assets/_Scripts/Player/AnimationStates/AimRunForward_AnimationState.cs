using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class AimRunForward_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public AimRunForward_AnimationState(PlayerAnimator owner) : base(owner)
        {
            m_playerAnimator = owner;
            m_playerAnimator.Animation.wrapMode = WrapMode.Loop;

            m_playerAnimator.StartCoroutine(Transition());
        }

        public override void Manage()
        {
            //set which states can be transitioned to here
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.AIM_RUN_FORWARD:
                    m_playerAnimator.SetState(new AimRunForward_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.AIM_RUN_BACK:
                    m_playerAnimator.SetState(new AimRunBack_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.AIM_RUN_LEFT:
                    m_playerAnimator.SetState(new AimRunLeft_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.AIM_RUN_RIGHT:
                    m_playerAnimator.SetState(new AimRunRight_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.AIMING:
                    m_playerAnimator.SetState(new Aiming_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.WALKING:
                    m_playerAnimator.SetState(new Walking_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.RUNNING:
                    m_playerAnimator.SetState(new Running_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.JUMPING:
                    m_playerAnimator.SetState(new Jumping_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.IDLE:
                    m_playerAnimator.SetState(new Idle_AnimationState(m_playerAnimator));
                    break;
            }
        }

        IEnumerator Transition()
        {
            m_playerAnimator.Animation.CrossFade("aimRunForward", 0.2f, PlayMode.StopAll);
            yield return null;
        }
    }
}