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
        [Header("Default")]
        [SerializeField] float m_defaultDistance = 5;
        [SerializeField] float m_defaultStartingAngle = 33;
        [SerializeField] float m_defaultMinAngle = -45; //min pitch
        [SerializeField] float m_defaultMaxAngle = 45; //max pitch
        [SerializeField] float m_defaultMovementSpeed;
        [SerializeField] float m_defaultLerpSpeed = 1f;
        [Header("Aiming")]
        [SerializeField] float m_aimingDistance = 1;
        [SerializeField] float m_aimingMinAngle = -90;  //max pitch
        [SerializeField] float m_aimingMaxAngle = 90;  //max pitch
        [SerializeField] Vector2 m_aimingOffset = new Vector2(1, 1);
        [SerializeField] float m_aimingMovementSpeed;
        [SerializeField] float m_aimingLerpSpeed = 0.3f;
        [SerializeField] float m_autoAimStrength = 10f;
        [Header("Dialogue")]
        [SerializeField] float m_dialogueDistance = 1;
        [SerializeField] float m_dialogueAngle = 33;
        [Header("General")]
        [SerializeField] AnimationCurve m_lerpCurve;

        //temp (I think)
        [Header("TEMP")]
        [SerializeField] public bool m_DefaultCanRotateVertically;

        Vector3 m_cameraOffset;
        Transform m_aimedAtTransform;

        //Public stuff, get only
        public float p_DefaultMovementSpeed { get => m_defaultMovementSpeed; }
        public float p_DefaultLerpSpeed { get => m_defaultLerpSpeed; }
        public float p_DefaultDistance { get => m_defaultDistance; }
        public float p_DefaultStartingAngle { get => m_defaultStartingAngle; }
        public float p_DefaultMinAngle { get => m_defaultMinAngle; }
        public float p_DefaultMaxAngle { get => m_defaultMaxAngle; }
        public float p_AimingDistance { get => m_aimingDistance; }
        public float p_AimingMinAngle { get => m_aimingMinAngle; }
        public float p_AimingMaxAngle { get => m_aimingMaxAngle; }
        public float p_AimingMovementSpeed { get => m_aimingMovementSpeed; }
        public float p_AimingLerpSpeed { get => m_aimingLerpSpeed; }
        public float p_AutoAimStrength { get => m_autoAimStrength; }
        public float p_DialogueDistance { get => m_dialogueDistance; }
        public float p_DialogueAngle { get => m_dialogueAngle; }
        public Transform p_CameraTarget { get => m_cameraTarget; }
        public Transform p_AimedAtTransform { get => m_aimedAtTransform; set => m_aimedAtTransform = value; }
        public Quaternion p_DefaultRotation { get => m_defaultRotation; }
        public Vector3 p_CameraOffset { get => m_cameraOffset; }
        public Vector3 p_AimingOffset { get => m_aimingOffset; }
        public AnimationCurve p_LerpCurve { get => m_lerpCurve; }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            m_defaultRotation = transform.rotation;
            m_cameraOffset = transform.position - m_cameraTarget.position;
            SetState(new Default_CameraState(this));
        }

        protected override void Update()
        {
            base.Update();
            CheckCollision();
        }

        //Casts ray from target to camera and checks if it hits, if so moves camera to hit position
        private void CheckCollision()
        {
            Vector3 targetToCamera = transform.position - m_cameraTarget.position;
            float distance = targetToCamera.magnitude;
            Vector3 direction = targetToCamera.normalized;
            RaycastHit hit;

            LayerMask layer = LayerMask.GetMask("IgnoreCamera");

            if (Physics.Raycast(m_cameraTarget.position, direction, out hit, distance, ~layer)) //flipped the bit operator here to hit everything other than that layer
            {
                //do not need to check if it is not a trigger as our project does not allow raycasts to hit triggers
                transform.position = hit.point;
            }
        }
    }
}

