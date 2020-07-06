using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Falling_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Falling_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)m_owner;
            //m_playerAnimator.Falling.wrapMode = WrapMode.Loop;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());

            //Debug.Log("Falling");
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
                case PlayerAnimationProperties.AIMING:
                    m_owner.SetState(new Aiming_AnimationState(m_owner));
                    break;
                default:
                    m_owner.SetState(new Idle_AnimationState(m_owner));    //if no state, return to idle
                    break;
            }
        }

        IEnumerator Transition()
        {
            //m_playerAnimator.Falling.Play();
            yield break;
        }
    }
}
