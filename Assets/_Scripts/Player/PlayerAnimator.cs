using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    //You might be wondering why this class even exists when the Animation state machine already exists in unity
    //Well something about searching for parameters by string rubbed me the wrong way, so now this exists :)

    public enum PlayerAnimationProperties
    {
        IDLE,     
        RUNNING,     
        JUMPING,
        FALLING,
        AIMING,
        CASTING,
        PUSHING,
        DYING
    }

    public class PlayerAnimator : GameCore.System.Automaton
    {
        Animator m_playerAnimator;

        PlayerAnimationProperties m_playerAnimProperties;

        [SerializeField] AnimationClip m_idleAnim;
        [SerializeField] AnimationClip m_runnningAnim;
        [SerializeField] AnimationClip m_jumpStart;
        [SerializeField] AnimationClip m_jumpMid;
        [SerializeField] AnimationClip m_jumpLand;
        [SerializeField] AnimationClip m_fallingAnim;
        [SerializeField] AnimationClip m_aimingAnim;
        [SerializeField] AnimationClip m_castingAnim;
        [SerializeField] Animation m_animation;
        [Header("Playback Speeds")]
        [SerializeField] float m_idleAnimSpeed = 2;
        [SerializeField] float m_runningAnimSpeed = 2;
        [SerializeField] float m_jumpStartAnimSpeed = 2;
        [SerializeField] float m_jumpMidAnimSpeed = 2;
        [SerializeField] float m_jumpLandAnimSpeed = 2;
        [SerializeField] float m_fallingAnimSpeed = 2;
        [SerializeField] float m_aimingAnimSpeed = 2;
        [SerializeField] float m_castingAnimSpeed = 2;

        #region PUBLIC ACCESSORS
        public AnimationClip Idle { get => m_idleAnim; }
        public AnimationClip Running { get => m_runnningAnim; }
        public AnimationClip JumpingStart { get => m_jumpStart; }
        public AnimationClip JumpingMid { get => m_jumpMid; }
        public AnimationClip JumpingLand { get => m_jumpLand; }
        public AnimationClip Falling { get => m_fallingAnim; }
        public AnimationClip Aiming { get => m_aimingAnim; }
        public AnimationClip Casting { get => m_castingAnim; }
        public Animation Animation { get => m_animation; }
        public PlayerAnimationProperties PlayerAnimProperties { get => m_playerAnimProperties; }
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            SetProperty(PlayerAnimationProperties.IDLE);
            m_animation.AddClip(m_idleAnim, "idle");
            m_animation.AddClip(m_runnningAnim, "running");
            m_animation.AddClip(m_jumpStart, "jumpStart");
            m_animation.AddClip(m_jumpMid, "jumpMid");
            m_animation.AddClip(m_jumpLand, "jumpLand");
            m_animation.AddClip(m_fallingAnim, "falling");
            m_animation.AddClip(m_aimingAnim, "aiming");
            m_animation.AddClip(m_castingAnim, "casting");

            foreach (AnimationState state in m_animation)
            {
                switch (state.name)
                {
                    case "idle":
                        state.speed = m_idleAnimSpeed;
                        break;
                    case "running":
                        state.speed = m_runningAnimSpeed;
                        break;
                    case "jumpStart":
                        state.speed = m_jumpStartAnimSpeed;
                        break;
                    case "jumpMid":
                        state.speed = m_jumpMidAnimSpeed;
                        break;
                    case "jumpLand":
                        state.speed = m_jumpLandAnimSpeed;
                        break;
                    case "falling":
                        state.speed = m_fallingAnimSpeed;
                        break;
                    case "aiming":
                        state.speed = m_aimingAnimSpeed;
                        break;
                    case "casting":
                        state.speed = m_castingAnimSpeed;
                        break;
                }
            }

            SetState(new Idle_AnimationState(this));
        }

        // Update is called once per frame
        protected override void Update()
        {
            //base.Update(); //Does not call manage on update as we only want to manage when a state changes
        }

        public void SetProperty(PlayerAnimationProperties property)
        {
            m_playerAnimProperties = property;
            if (m_state != null)
            {
                m_state.Manage();
            }
        }
    }
}