﻿using System.Collections;
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
        DYING,
        FREE_FALLING,
        RECOVERING,
        SLIDING
    }

    public class PlayerAnimator : GameCore.System.Automaton
    {
        Animator m_playerAnimator;

        PlayerAnimationProperties m_playerAnimProperties;

        [SerializeField] AnimationClip m_idleAnim;
        [SerializeField] AnimationClip m_runnningAnim;
        [SerializeField] AnimationClip m_jumpStartAnim;
        [SerializeField] AnimationClip m_jumpMidAnim;
        [SerializeField] AnimationClip m_jumpLandAnim;
        [SerializeField] AnimationClip m_fallingAnim;
        [SerializeField] AnimationClip m_aimingAnim;
        [SerializeField] AnimationClip m_castingAnim;
        [SerializeField] AnimationClip m_freeFallingAnim;
        [SerializeField] AnimationClip m_recoveringAmim;
        [SerializeField] AnimationClip m_slidingStartAnim;
        [SerializeField] AnimationClip m_slidingMidAnim;
        [SerializeField] AnimationClip m_slidingEndAnim;
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
        [SerializeField] float m_freeFallingAnimSpeed = 2;
        [SerializeField] float m_recoveringAnimSpeed = 2;
        [SerializeField] float m_slidingStartAnimSpeed = 2;
        [SerializeField] float m_slidingMidAnimSpeed = 2;
        [SerializeField] float m_slidingEndAnimSpeed = 2;

        //non serialized fields
        AnimationState m_idleState;
        AnimationState m_runningState;
        AnimationState m_jumpStartState;
        AnimationState m_jumpMidState;
        AnimationState m_jumpEndState;
        AnimationState m_fallingState;
        AnimationState m_aimingState;
        AnimationState m_castingState;
        AnimationState m_freeFallingState;
        AnimationState m_recoveringState;
        AnimationState m_slidingStartState;
        AnimationState m_slidingMidState;
        AnimationState m_slidingEndState;

        #region PUBLIC ACCESSORS
        public AnimationClip Idle { get => m_idleAnim; }
        public AnimationClip Running { get => m_runnningAnim; }
        public AnimationClip JumpingStart { get => m_jumpStartAnim; }
        public AnimationClip JumpingMid { get => m_jumpMidAnim; }
        public AnimationClip JumpingLand { get => m_jumpLandAnim; }
        public AnimationClip Falling { get => m_fallingAnim; }
        public AnimationClip Aiming { get => m_aimingAnim; }
        public AnimationClip Casting { get => m_castingAnim; }
        public AnimationClip FreeFalling { get => m_freeFallingAnim; }
        public AnimationClip Recovering { get => m_recoveringAmim; }
        public AnimationClip SlidingStart { get => m_slidingStartAnim; }
        public AnimationClip SlidingMid { get => m_slidingMidAnim; }
        public AnimationClip SlidingEnd { get => m_slidingEndAnim; }

        public AnimationState IdleState { get => m_idleState; }
        public AnimationState RunningState { get => m_runningState; }
        public AnimationState JumpingStartState { get => m_jumpStartState; }
        public AnimationState JumpingMidState { get => m_jumpMidState; }
        public AnimationState JumpingEndState { get => m_jumpEndState; }
        public AnimationState FallingState { get => m_fallingState; }
        public AnimationState AimingState { get => m_aimingState; }
        public AnimationState CastingState { get => m_castingState; }
        public AnimationState FreeFallingState { get => m_freeFallingState; }
        public AnimationState RecoveringState { get => m_recoveringState; }
        public AnimationState SlidingStartState { get => m_slidingStartState; }
        public AnimationState SlidingMidState { get => m_slidingMidState; }
        public AnimationState SlidingEndState { get => m_slidingEndState; }

        public float RunningAnimSpeed { get => m_runningAnimSpeed; }

        public Animation Animation { get => m_animation; }
        public PlayerAnimationProperties PlayerAnimProperties { get => m_playerAnimProperties; }
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            SetProperty(PlayerAnimationProperties.IDLE);
            m_animation.AddClip(m_idleAnim, "idle");
            m_animation.AddClip(m_runnningAnim, "running");
            m_animation.AddClip(m_jumpStartAnim, "jumpStart");
            m_animation.AddClip(m_jumpMidAnim, "jumpMid");
            m_animation.AddClip(m_jumpLandAnim, "jumpLand");
            m_animation.AddClip(m_fallingAnim, "falling");
            m_animation.AddClip(m_aimingAnim, "aiming");
            m_animation.AddClip(m_castingAnim, "casting");
            m_animation.AddClip(m_freeFallingAnim, "freeFalling");
            m_animation.AddClip(m_recoveringAmim, "recovering");
            m_animation.AddClip(m_slidingStartAnim, "slidingStart");
            m_animation.AddClip(m_slidingMidAnim, "slidingMid");
            m_animation.AddClip(m_slidingEndAnim, "slidingEnd");

            foreach (AnimationState state in m_animation)
            {
                switch (state.name)
                {
                    case "idle":
                        state.speed = m_idleAnimSpeed;
                        m_idleState = state;
                        break;
                    case "running":
                        state.speed = m_runningAnimSpeed;
                        m_runningState = state;
                        break;
                    case "jumpStart":
                        state.speed = m_jumpStartAnimSpeed;
                        m_jumpStartState = state;
                        break;
                    case "jumpMid":
                        state.speed = m_jumpMidAnimSpeed;
                        m_jumpMidState = state;
                        break;
                    case "jumpLand":
                        state.speed = m_jumpLandAnimSpeed;
                        m_jumpEndState = state;
                        break;
                    case "falling":
                        state.speed = m_fallingAnimSpeed;
                        m_fallingState = state;
                        break;
                    case "aiming":
                        state.speed = m_aimingAnimSpeed;
                        m_aimingState = state;
                        break;
                    case "casting":
                        state.speed = m_castingAnimSpeed;
                        m_castingState = state;
                        break;
                    case "freeFalling":
                        state.speed = m_freeFallingAnimSpeed;
                        m_freeFallingState = state;
                        break;
                    case "recovering":
                        state.speed = m_recoveringAnimSpeed;
                        m_recoveringState = state;
                        break;
                    case "slidingStart":
                        state.speed = m_slidingMidAnimSpeed;
                        m_slidingStartState = state;
                        break;
                    case "slidingMid":
                        state.speed = m_slidingMidAnimSpeed;
                        m_slidingStartState = state;
                        break;
                    case "slidingEnd":
                        state.speed = m_slidingEndAnimSpeed;
                        m_slidingStartState = state;
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