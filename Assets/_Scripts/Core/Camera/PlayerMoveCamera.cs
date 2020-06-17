using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.System;

namespace GameCore.Camera
{
    public class PlayerMoveCamera : Automaton
    {
        [SerializeField]
        Transform m_cameraTarget;
        Quaternion m_defaultRotation;
        [SerializeField]
        float m_aimingDistance = 1;
        [SerializeField]
        float m_defaultDistance = 5;
        [SerializeField]
        float m_defaultStartingAngle = 33;
        [SerializeField]
        float m_defaultMaxAngle = 45; //max pitch
        [SerializeField]
        float m_aimingMaxAngle = 90;  //max pitch
        [SerializeField]
        float m_dialogueDistance = 1;
        [SerializeField]
        float m_dialogueAngle = 33;
        [SerializeField]
        Vector2 m_aimingOffset = new Vector2(1,1);
        [SerializeField]
        float m_movementSpeed;
        [SerializeField]
        float m_aimingMovementSpeed;
        [SerializeField]
        float m_comebackSpeed;
        Vector3 m_cameraOffset;
        [SerializeField]
        bool m_smoothMovement = false;
        [SerializeField]
        float m_lerpSpeed = 1f;
        [SerializeField]
        float m_aimingLerpSpeed = 0.3f;

        //temp (I think)
        [Header("TEMP")]
        [SerializeField] bool m_fixedDefaultCamera;
        [SerializeField] public bool m_DefaultCanRotateVertically;

        public float p_MovementSpeed { get => m_movementSpeed; }
        public float p_ComebackSpeed { get => m_comebackSpeed; }
        public float p_LerpSpeed { get => m_lerpSpeed; }
        public float p_DefaultDistance { get => m_defaultDistance; }
        public float p_DefaultStartingAngle { get => m_defaultStartingAngle; }
        public float p_DefaultMaxAngle { get => m_defaultMaxAngle; }
        public float p_AimingDistance { get => m_aimingDistance; }
        public float p_AimingMaxAngle { get => m_aimingMaxAngle; }
        public float p_AimingMovementSpeed { get => m_aimingMovementSpeed; }
        public float p_AimingLerpSpeed { get => m_aimingLerpSpeed; }
        public float p_DialogueDistance { get => m_dialogueDistance; }
        public float p_DialogueAngle { get => m_dialogueAngle; }
        public Transform p_CameraTarget { get => m_cameraTarget; }
        public Quaternion p_DefaultRotation { get => m_defaultRotation; }
        public Vector3 p_CameraOffset { get => m_cameraOffset; }
        public Vector3 p_AimingOffset { get => m_aimingOffset; }
        public bool p_SmoothMovement { get => m_smoothMovement; }
        public bool p_FixedDefaultCamera { get => m_fixedDefaultCamera; }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            m_defaultRotation = transform.rotation;
            m_cameraOffset = transform.position - m_cameraTarget.position;
            SetState(new Default_CameraState(this));
        }
    }
}

