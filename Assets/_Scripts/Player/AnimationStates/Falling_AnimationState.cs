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

            m_playerAnimator.Animation.wrapMode = WrapMode.Once;
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
                case PlayerAnimationProperties.JUMPING:
                    //can actually transition to the jump start state if the player has jumped within the grace period
                    m_playerAnimator.SetState(new Jumping_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.FREE_FALLING:
                    m_playerAnimator.SetState(new FreeFalling_AnimationState(m_playerAnimator));
                    break;
                default:
                    m_playerAnimator.SetState(new JumpEnd_AnimationState(m_playerAnimator));
                    break;
            }
        }

        IEnumerator Transition()
        {
            try
            {
                m_playerAnimator.Animation.CrossFade("falling", 0.2f, PlayMode.StopAll);
                m_playerAnimator.Animation.PlayQueued("falling");
            }
            catch
            {
                Debug.LogError("Falling animation not set in editor or is null for some other reason");
            }
            yield break;
        }
    }
}
