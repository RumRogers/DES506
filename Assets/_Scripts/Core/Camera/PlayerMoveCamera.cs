using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore.System;
using System;
namespace GameCore.Camera
{
    public class PlayerMoveCamera : Automaton
    {
        const string FADEIMAGE_PATH = "Prefabs/UI/CameraFade/CameraEffectCanvas";

        [SerializeField]
        Transform m_cameraTarget;
        Quaternion m_defaultRotation;
        [Header("Default")]
        [SerializeField] float m_defaultDistance = 5;
        [SerializeField] float m_defaultStartingAngle = 33;
        [SerializeField] float m_defaultMinAngle = -45; //min pitch
        [SerializeField] float m_defaultMaxAngle = 45; //max pitch
        [SerializeField] float m_defaultMovementSpeed = 10;
        [SerializeField] float m_defaultLerpSpeed = 1f;
        [SerializeField] float m_defaultFOV = 45f;
        [Header("Aiming")]
        [SerializeField] float m_aimingDistance = 1;
        [SerializeField] float m_aimingMinAngle = -90;  //min pitch
        [SerializeField] float m_aimingMaxAngle = 90;  //max pitch
        [SerializeField] Vector2 m_aimingOffset = new Vector2(1, 1);
        [SerializeField] float m_aimingMovementSpeed = 10;
        [SerializeField] float m_aimingLerpSpeed = 0.3f;
        [SerializeField] float m_autoAimStrength = 10f;
        [SerializeField] bool m_autoAimOn = true;
        [SerializeField] float m_aimingFOV = 40f;
        [Header("Dialogue")]
        [SerializeField] float m_dialogueDistance = 1;
        [SerializeField] float m_dialogueAngle = 33;
        [Header("General")]
        [SerializeField] AnimationCurve m_lerpCurve;
        [SerializeField] float m_smoothFactor = 0.8f;
        [SerializeField] float m_collisionHitCameraOffsetDistance = 0.1f;
        [Header("Collision")]
        [SerializeField] float m_minDistanceFromPlayer = 0.3f;

        //temp (I think)
        [Header("TEMP")]
        [SerializeField] public bool m_DefaultCanRotateVertically;

        Vector3 m_cameraOffset;
        Transform m_aimedAtTransform;
        Vector3 m_position;   //the position where the camera should end up, modified in the camera states
        Quaternion m_rotation;
        Vector3 m_oldPosition;
        Vector2 m_input;

        //Camera fade vars
        Image m_cameraFadeImage;
        GameObject m_cameraFadeObject;
        bool m_fadeOngoing = false;

        //Public stuff, get only
        public bool p_AutoAimOn { get => m_autoAimOn; }
        public float p_SmoothFactor { get => m_smoothFactor; }
        public float p_DefaultMovementSpeed { get => m_defaultMovementSpeed; }
        public float p_DefaultLerpSpeed { get => m_defaultLerpSpeed; }
        public float p_DefaultDistance { get => m_defaultDistance; }
        public float p_DefaultStartingAngle { get => m_defaultStartingAngle; }
        public float p_DefaultMinAngle { get => m_defaultMinAngle; }
        public float p_DefaultMaxAngle { get => m_defaultMaxAngle; }
        public float p_DefaultFOV { get => m_defaultFOV; }
        public float p_AimingDistance { get => m_aimingDistance; }
        public float p_AimingMinAngle { get => m_aimingMinAngle; }
        public float p_AimingMaxAngle { get => m_aimingMaxAngle; }
        public float p_AimingMovementSpeed { get => m_aimingMovementSpeed; }
        public float p_AimingLerpSpeed { get => m_aimingLerpSpeed; }
        public float p_AutoAimStrength { get => m_autoAimStrength; }
        public float p_AimingFOV { get => m_aimingFOV; }
        public float p_DialogueDistance { get => m_dialogueDistance; }
        public float p_DialogueAngle { get => m_dialogueAngle; }
        public Transform p_CameraTarget { get => m_cameraTarget; }
        public Transform p_AimedAtTransform { get => m_aimedAtTransform; set => m_aimedAtTransform = value; }
        public Quaternion p_DefaultRotation { get => m_defaultRotation; }
        public Quaternion p_Rotation { get => m_rotation; set => m_rotation = value; }
        public Vector3 p_CameraOffset { get => m_cameraOffset; }
        public Vector3 p_AimingOffset { get => m_aimingOffset; }
        public Vector3 p_Position { get => m_position; set => m_position = value; }
        public Vector3 p_Input { get => m_input; }
        public AnimationCurve p_LerpCurve { get => m_lerpCurve; }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            m_defaultRotation = transform.rotation;
            m_cameraOffset = transform.position - m_cameraTarget.position;
            SetState(new Default_CameraState(this));
            m_oldPosition = transform.position;

            m_cameraFadeObject = Instantiate(Resources.Load<GameObject>(FADEIMAGE_PATH));
            m_cameraFadeImage = m_cameraFadeObject.GetComponentInChildren<Image>();
            m_cameraFadeImage.raycastTarget = false;
            m_cameraFadeImage.color = new Color(0, 0, 0, 0);

        }

        protected override void Update()
        {
            //base.Update();
        }

        private void LateUpdate()
        {
            m_input = new Vector2(Input.GetAxis("Camera Y"), Input.GetAxis("Camera X"));
            m_state.Manage();

            transform.position = m_position;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);//never want the camera to roll
            CheckCollision();
        }

        //Casts ray from target to camera and checks if it hits, if so moves camera to hit position
        public void CheckCollision()
        {
            Vector3 targetToCamera = transform.position - m_cameraTarget.position;
            float distance = targetToCamera.magnitude;
            Vector3 direction = targetToCamera.normalized;
            Vector3 offset = transform.forward * m_collisionHitCameraOffsetDistance;
            RaycastHit hit;

            LayerMask layer = LayerMask.GetMask("IgnoreCamera");

            Debug.DrawRay(m_cameraTarget.position, direction * distance);

            if (Physics.Raycast(m_cameraTarget.position, direction, out hit, distance, ~layer)) //flipped the bit operator here to hit everything other than that layer
            {
                //do not need to check if it is not a trigger as our project does not allow raycasts to hit triggers
                //Get distance
                Vector3 targetToWall = hit.point - m_cameraTarget.position;
                float distanceToWall = targetToWall.magnitude;
                //normalise vector
                targetToWall = targetToWall / distanceToWall;
                //clamp distance between min and max camera distance values
                distanceToWall = Mathf.Clamp(distanceToWall, m_minDistanceFromPlayer, m_defaultDistance);
                //multiply normalised vector by distance
                targetToWall *= distanceToWall;
                //set transform to that point
                transform.position = m_cameraTarget.position + (targetToWall + offset);
                
            }
        }

        public IEnumerator FadeToColour(Color colour, float timeToColour)
        {
            if (colour == m_cameraFadeImage.color || m_fadeOngoing)
                yield break;

            float time = 0;
            Color startingColour = m_cameraFadeImage.color;
            m_fadeOngoing = true;
            while (true)
            {
                time += Time.deltaTime;

                float percomp = time / timeToColour;

                m_cameraFadeImage.color = Color.Lerp(startingColour, colour, percomp);

                if (time > timeToColour)
                {
                    m_fadeOngoing = false;
                    m_cameraFadeImage.color = colour;
                    yield break;
                }
                yield return null;
            }
        }

        //overloaded function that takes a lambda callback
        public IEnumerator FadeToColour(Color colour, float timeToColour, Func<bool> callback)
        {
            if (colour == m_cameraFadeImage.color || m_fadeOngoing)
                yield break;

            float time = 0;
            Color startingColour = m_cameraFadeImage.color;
            m_fadeOngoing = true;
            while (true)
            {
                time += Time.deltaTime;

                float percomp = time / timeToColour;

                m_cameraFadeImage.color = Color.Lerp(startingColour, colour, percomp);

                if (time > timeToColour)
                {
                    m_fadeOngoing = false;
                    m_cameraFadeImage.color = colour;
                    callback();
                    yield break;
                }
                yield return null;
            }
        }
    }
}

