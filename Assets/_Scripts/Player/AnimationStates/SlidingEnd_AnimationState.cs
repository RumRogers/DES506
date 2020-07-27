using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class SlidingEnd_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public SlidingEnd_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)m_owner;

            m_playerAnimator.Animation.wrapMode = WrapMode.Once;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());

            //Debug.Log("Sliding");
        }
        //You might be thinking "why use a switch statement here? surely it's better and more efficent to just change the state from within the player entity class!" 
        //while this might be true, this allows us to control which states can be transitioned into others.
        public override void Manage()
        {

        }

        IEnumerator Transition()
        {
            try
            {
                m_playerAnimator.Animation.CrossFade("slidingEnd", 0.2f, PlayMode.StopAll);
                //m_playerAnimator.Animation.Play("slidingEnd", PlayMode.StopAll);
            }
            catch
            {
                Debug.LogError("Sliding End animation not set in editor or is null for some other reason");
            }
            while (m_playerAnimator.Animation.isPlaying)
            {
                yield return null;
            }
            //return to idle after finishing anim
            m_playerAnimator.SetState(new Idle_AnimationState(m_playerAnimator));
            yield break;
        }
    }
}