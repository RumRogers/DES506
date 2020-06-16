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
            //m_playerAnimator.Running.wrapMode = WrapMode.Loop;
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
                default:
                    m_owner.SetState(new Idle_AnimationState(m_owner));    //if no state, return to idle
                    break;
            }
        }

        IEnumerator Transition()
        {
            //m_playerAnimator.Running.Play();
            yield break;
        }
    }
}
