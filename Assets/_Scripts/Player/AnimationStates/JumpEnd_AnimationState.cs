using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class JumpEnd_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public JumpEnd_AnimationState(GameCore.System.Automaton owner) : base(owner)
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
                case PlayerAnimationProperties.JUMPING:
                    m_playerAnimator.SetState(new Jumping_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.FALLING:
                    m_playerAnimator.SetState(new Falling_AnimationState(m_playerAnimator));
                    break;
            }
        }

        IEnumerator Transition()
        {
            try
            {
                m_playerAnimator.Animation.CrossFade("jumpLand", 0.2f, PlayMode.StopAll);
            }
            catch
            {
                Debug.LogError("Jump Landing animation not set in editor or is null for some other reason");
            }
            while (m_playerAnimator.Animation.isPlaying)
            {
                //do nothing
                yield return null;
            }
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.WALKING:
                    m_playerAnimator.SetState(new Walking_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.RUNNING:
                    m_playerAnimator.SetState(new Running_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.SLIDING:
                    m_playerAnimator.SetState(new Sliding_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.AIMING:
                    m_playerAnimator.SetState(new Aiming_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.LEFT_TURN:
                    m_playerAnimator.SetState(new TurnLeft_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.RIGHT_TURN:
                    m_playerAnimator.SetState(new TurnRight_AnimationState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.IDLE:
                    m_playerAnimator.SetState(new Idle_AnimationState(m_playerAnimator));
                    break;
            }
            yield break;
        }
    }
}
