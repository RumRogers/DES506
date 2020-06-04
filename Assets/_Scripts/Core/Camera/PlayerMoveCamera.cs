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
        [SerializeField]
        float m_movementSpeed;
        [SerializeField]
        float m_comebackSpeed;
        Vector3 m_cameraOffset;
        [SerializeField]
        bool m_smoothMovement = false;
        [SerializeField]
        float m_lerpSpeed = 1f;

        public float p_MovementSpeed { get => m_movementSpeed; }
        public float p_ComebackSpeed { get => m_comebackSpeed; }
        public float p_LerpSpeed { get => m_lerpSpeed; }
        public Transform p_CameraTarget { get => m_cameraTarget; }
        public Vector3 p_CameraOffset { get => m_cameraOffset; }
        public bool p_SmoothMovement { get => m_smoothMovement; }

        private void Start()
        {
            m_cameraOffset = transform.position - m_cameraTarget.position;
            SetState(new Default_CameraState(this));
        }
    }
}

