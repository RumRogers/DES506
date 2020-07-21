using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Death_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;
        Vector3 m_velocity; //local velocity varable, easier to manipulate individual components, added to the players velocity at end of Manage()

        public Death_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            Debug.Log("In DeathState!");
            //m_playerEntity.transform.position = m_playerEntity.PlayerStartPosition;

            m_velocity = m_playerEntity.Velocity;
            m_playerEntity.AddEntityProperty(PlayerEntityProperties.DYING);
            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.FREE_FALLING);
            m_playerEntity.Animator.SetState(new FreeFalling_AnimationState(m_playerEntity.Animator));
        }

        public override void Manage()
        {
            //m_velocity += (Vector3.down * m_playerEntity.Gravity * 2) * Time.fixedDeltaTime;
            

            //if grounded transition to previous grounded state, or if there wasn't one, transition to the default state
            if (!m_playerEntity.Grounded)
            {
                m_playerEntity.Velocity += (Vector3.down * m_playerEntity.Gravity) * Time.fixedDeltaTime;
            }
            else
            {                
                m_playerEntity.Animator.SetState(new Recovering_AnimationState(m_playerEntity.Animator));
                m_playerEntity.StartCoroutine(OnDeathSequenceEnded());
            }
        }

        private IEnumerator OnDeathSequenceEnded()
        {
            yield return new WaitForSeconds(1.5f);
            m_playerEntity.RemoveEntityProperty(PlayerEntityProperties.DYING);
            
            m_playerEntity.SetState(new Default_PlayerState(m_playerEntity));
            m_playerEntity.Animator.SetState(new Idle_AnimationState(m_playerEntity.Animator));
            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.IDLE);
        }
    }
}