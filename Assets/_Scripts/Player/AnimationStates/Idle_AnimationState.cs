using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Idle_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Idle_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)m_owner;
            m_playerAnimator.Animation.wrapMode = WrapMode.Loop;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());

            //Debug.Log("Idle");
        }
        //You might be thinking "why use a switch statement here? surely it's better and more efficent to just change the state from within the player entity class!" 
        //while this might be true, this allows us to control which states can be transitioned into others.
        public override void Manage()
        {
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.RUNNING:
                    m_owner.SetState(new Running_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.SLIDING:
                    m_playerAnimator.SetState(new Sliding_AnimationState(m_owner));
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
                case PlayerAnimationProperties.AIMING:
                    m_owner.SetState(new Aiming_AnimationState(m_owner));
                    break;
                default:
                    break;
            }
        }

        IEnumerator Transition()
        {
            try
            {
                m_playerAnimator.Animation.CrossFade("idle", 0.2f, PlayMode.StopAll);
                //m_playerAnimator.Animation.PlayQueued("idle");
            }
            catch
            {
                Debug.LogError("Idle animation not set in editor or is null for some other reason");
            }
            yield break;
        }
    }
}