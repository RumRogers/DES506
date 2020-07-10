using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Jumping_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        Vector3 m_velocity = Vector3.zero;
        float m_additionalJumpForce = 10f;
        float m_maxButtonHoldTime = 0.5f;
        float m_time = 0;
        bool m_animationFinished = false;
        float m_entrySpeed = 0;

        public Jumping_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)m_owner;
            
            //get the speed when entering the state before the jump vel is added. used to store the speed for multiplying in the direction held after jump is finished
            m_entrySpeed = m_velocity.magnitude;

            //Adds velocity based on entity property flags
            if (m_playerEntity.HasProperty(PlayerEntityProperties.JUMP_NORMAL))
                m_velocity = new Vector3(0, m_playerEntity.JumpVelocity, 0);

            if (m_playerEntity.HasProperty(PlayerEntityProperties.JUMP_HIGH))
                m_velocity = new Vector3(0, m_playerEntity.HighJumpVelocity, 0);

            m_additionalJumpForce = m_playerEntity.Velocity.y * m_playerEntity.JumpHeldMutliplier;

            m_playerEntity.Animator.SetProperty(PlayerAnimationProperties.JUMPING);

            m_playerEntity.StopAllCoroutines();
            m_playerEntity.StartCoroutine(WaitForJumpAnimation());
        }

        public override void Manage()
        {            
            if (m_animationFinished)
            {
                //not functional while waiting for jump for some reason. need to fix
                if (Input.GetButton("Jump") && m_time < m_maxButtonHoldTime)
                {
                    //adding additional jump force gained by holding the jump button
                    m_playerEntity.Velocity += (Vector3.up * m_additionalJumpForce) * Time.deltaTime;
                    m_time += Time.deltaTime;
                }
                else if (Input.GetButtonUp("Jump") || m_time > m_maxButtonHoldTime)
                {
                    m_time = m_maxButtonHoldTime;// setting it to max time so you cannot press jump again to gain height once you've already released it
                }
                //subtracting gravity from upwards velocity until forces equalise
                m_velocity += (Vector3.down * m_playerEntity.Gravity) * Time.deltaTime;

                //Directional input, slower in mid air
                Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
                Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
                m_playerEntity.Direction = (forwardMovement + rightMovement).normalized;

                if (m_playerEntity.Direction != Vector3.zero)
                {
                    m_playerEntity.transform.rotation = Quaternion.LookRotation(new Vector3(m_playerEntity.Velocity.normalized.x, 0, m_playerEntity.Velocity.normalized.z));
                    m_velocity += (m_playerEntity.Direction * m_playerEntity.AerialAccelleration) * Time.deltaTime;
                   
                }
                else if (Mathf.Abs(m_velocity.x) > 0.1f || Mathf.Abs(m_velocity.z) > 0.1f)
                {
                    m_velocity += (((m_velocity.normalized) * -1) * m_playerEntity.WalkingDeceleration) * Time.deltaTime;
                }
                m_playerEntity.Velocity = new Vector3(m_velocity.x, m_velocity.y, m_velocity.z);

                if (m_playerEntity.Velocity.y < 0)
                {
                    m_owner.SetState(new Falling_PlayerState(m_owner));
                    return;
                }
            }
        }

        IEnumerator WaitForJumpAnimation()
        {
            while (m_playerEntity.Animator.GetState().GetType() == typeof(Jumping_AnimationState))
            {
                yield return null;
            }
            Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxisRaw("Vertical"); // removing the y component from the camera's forward vector
            Vector3 rightMovement = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");

            m_velocity += (forwardMovement + rightMovement).normalized * m_entrySpeed;

            m_animationFinished = true;
        }
    }
}
