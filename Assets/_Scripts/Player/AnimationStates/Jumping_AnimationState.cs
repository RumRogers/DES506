using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Jumping_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Jumping_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)owner;

            m_playerAnimator.Animation.wrapMode = WrapMode.Once;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());
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
                case PlayerAnimationProperties.FALLING:
                    m_owner.SetState(new JumpMid_AnimationState(m_owner));
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
                m_playerAnimator.Animation.Play("jumpStart", PlayMode.StopAll);
            }
            catch
            {
                Debug.LogError("Jump Start animation not set in editor or is null for some other reason");
            }
            while (m_playerAnimator.Animation.isPlaying)
            {
                //do nothing
                yield return null;
            }
            m_playerAnimator.SetState(new JumpMid_AnimationState(m_playerAnimator));
            yield break;
        }
    }
}
