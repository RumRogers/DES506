using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Running_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Running_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)m_owner;
            m_playerAnimator.Animation.wrapMode = WrapMode.Loop;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());
            //Debug.Log("Running");
        }
        //You might be thinking "why use a switch statement here? surely it's better and more efficent to just change the state from within the player entity class!" 
        //while this might be true, this allows us to control which states can be transitioned into others.
        public override void Manage()
        {
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.WALKING:
                    m_playerAnimator.SetState(new Walking_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.SLIDING:
                    m_playerAnimator.SetState(new Sliding_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.JUMPING:
                    m_owner.SetState(new Jumping_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.FALLING:
                    m_owner.SetState(new Falling_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.PUSHING:
                    m_owner.SetState(new Pushing_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.RUNNING: //if already running do nothing
                    break;
                case PlayerAnimationProperties.AIMING:
                    m_owner.SetState(new Aiming_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.LEFT_TURN:
                    m_playerAnimator.SetState(new TurnLeft_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.RIGHT_TURN:
                    m_playerAnimator.SetState(new TurnRight_AnimationState(m_playerAnimator));
                    break;
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
                default:
                    m_owner.SetState(new Idle_AnimationState(m_owner));    //if no state, return to idle
                    break;
            }
        }

        IEnumerator Transition()
        {
            try
            {
                m_playerAnimator.Animation.CrossFade("running", 0.1f, PlayMode.StopAll);
                //m_playerAnimator.Animation.PlayQueued("running");
            }
            catch
            {
                Debug.LogError("Running animation not set in editor or is null for some other reason");
            }
            yield break;
        }
    }
}
