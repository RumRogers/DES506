using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class SlidingMid_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public SlidingMid_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)m_owner;

            m_playerAnimator.Animation.wrapMode = WrapMode.Loop;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());

            //Debug.Log("Sliding");
        }
        //You might be thinking "why use a switch statement here? surely it's better and more efficent to just change the state from within the player entity class!" 
        //while this might be true, this allows us to control which states can be transitioned into others.
        public override void Manage()
        {
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.JUMPING:
                    m_playerAnimator.SetState(new Jumping_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.RUNNING:
                    m_playerAnimator.SetState(new Running_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.IDLE:
                    m_playerAnimator.SetState(new SlidingEnd_AnimationState(m_playerAnimator));
                    break;
            }
        }

        IEnumerator Transition()
        {
            m_playerAnimator.Animation.Play("slidingMid", PlayMode.StopAll);
            yield break;
        }
    }
}