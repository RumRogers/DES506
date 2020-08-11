using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Aiming_AnimationState : GameCore.System.State
    {
        PlayerAnimator m_playerAnimator;

        public Aiming_AnimationState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerAnimator = (PlayerAnimator)owner;

            m_playerAnimator.Animation.wrapMode = WrapMode.Loop;
            m_playerAnimator.StopAllCoroutines();
            m_playerAnimator.StartCoroutine(Transition());
        }

        public override void Manage()
        {
            switch (m_playerAnimator.PlayerAnimProperties)
            {
                case PlayerAnimationProperties.AIMING:
                    m_owner.SetState(new Aiming_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.RUNNING:
                    m_owner.SetState(new Running_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.JUMPING:
                    m_owner.SetState(new Jumping_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.FALLING:
                    m_owner.SetState(new Falling_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.FREE_FALLING:
                    m_playerAnimator.SetState(new Falling_PlayerState(m_playerAnimator));
                    break;
                case PlayerAnimationProperties.IDLE:
                    m_owner.SetState(new Idle_AnimationState(m_owner));
                    break;
                case PlayerAnimationProperties.CASTING:
                    m_owner.SetState(new Casting_AnimationState(m_owner));
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
            }
        }

        IEnumerator Transition()
        {
            try
            {
                if (m_playerAnimator.Player.EquipedItem == PlayerEquipableItems.SPELL_QUILL)
                {
                    m_playerAnimator.Animation.CrossFade("aiming", 0.2f, PlayMode.StopAll);
                }
                else
                {
                    m_playerAnimator.Animation.CrossFade("aimingEraser", 0.2f, PlayMode.StopAll);
                }
                //m_playerAnimator.Animation.PlayQueued("aiming");
            }
            catch
            {
                Debug.LogError("Aiming animation not set in editor or is null for some other reason");
            }
            yield break;
        }
    }
}