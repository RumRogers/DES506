using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Recovering_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Recovering_AnimationState(PlayerAnimator owner) : base(owner)
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
            m_playerAnimator.Animation.Play("recovering", PlayMode.StopAll);
            yield break;
        }
    }
}