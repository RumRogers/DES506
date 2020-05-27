using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerEntity))]
    public class PlayerMovement : GameCore.System.Automaton
    {
        PlayerEntity m_playerEntity;

        //Player stats (editor variables)
        [Header("Pushing")]
        [SerializeField] AnimationCurve m_pushMovementCurve = new AnimationCurve();
        [SerializeField] float m_pushingSpeed = 2.0f;
        [Header("Ground Movement")]
        [SerializeField] float m_maxSpeed = 2.0f;
        [SerializeField] float m_walkingAcceleration = 15.0f;
        [SerializeField] float m_walkingDeceleration = 15.0f;
        [Header("Air Movement")]
        [SerializeField] float m_aerialAccelleration = 5.0f;
        [SerializeField] float m_gravity = 9.81f;
        [SerializeField] float m_jumpVelocity = 4.5f;
        [SerializeField] float m_highJumpVelocity = 9.5f;
        [Header("Collision")]
        [SerializeField] float m_maxClimbableIncline = 45.0f;
        [SerializeField] float m_groundPadding = 0.1f;  //How far from the floor the ray should start
        [SerializeField] float m_collisionRayLengthMultiplyer = 0.7f;   //Determines what percentage of the player's bounds to use as ray length
        [SerializeField] float m_groundOverlapPadding = 0.1f;   //How far the player can sink before overlap recovery takes place
        [Header("Properties (Debug)")]
        [SerializeField] bool m_drawDebugRays = false;
        //player stats (not editor accessible)
        Vector3 m_playerStartPosition;
        Vector3 m_velocity = Vector3.zero;
        Vector3 m_direction;
        Collider m_playerCollider;

        //Collision variables
        RaycastHit m_groundedHitInfo;
        RaycastHit m_collisionHitInfo;

        //Interacting transforms 
        List<Transform> m_interactablesInRange = new List<Transform>();
        Transform m_closestInteractable = null;

        #region PUBLIC ACCESSORS
        //player stats, Mutable
        public Vector3 Velocity { get => m_velocity; set => m_velocity = value; }
        public Vector3 Direction { get => m_direction; set => m_direction = value; }
        //player stats, getters only
        public PlayerEntity PlayerEntity { get => m_playerEntity; }
        public float MaxSpeed { get => m_maxSpeed; }
        public float WalkingAcceleration { get => m_walkingAcceleration; }
        public float WalkingDeceleration { get => m_walkingDeceleration; }
        public float AerialAccelleration { get => m_aerialAccelleration; }
        public float PushSpeed { get => m_pushingSpeed; }
        public float Gravity { get => m_gravity; }
        public float JumpVelocity { get => m_jumpVelocity; }
        public float HighJumpVelocity { get => m_highJumpVelocity; }
        public float MaxClimableAngle { get => m_maxClimbableIncline; }
        public float GroundOverlapPadding { get => m_groundOverlapPadding; }
        public Collider PlayerCollider { get => m_playerCollider; }
        public RaycastHit GroundHitInfo { get => m_groundedHitInfo; }
        public Vector3 PlayerStartPosition { get => m_playerStartPosition; }
        //Pushing Getters
        public Transform ClosestInteractable { get => m_closestInteractable; set => m_closestInteractable = value; }
        public List<Transform> InteractablesInRange { get => m_interactablesInRange; set => m_interactablesInRange = value; }
        public AnimationCurve PushMovementCurve { get => m_pushMovementCurve; }
        #endregion

        private void Awake()
        {
            SetState(new Default_PlayerState(this));
            m_playerCollider = GetComponent<Collider>();
            m_playerEntity = transform.GetComponent<PlayerEntity>();

            //setting properties
            m_playerEntity.AddEntityProperty(PlayerEntityProperties.CAN_JUMP);
            m_playerEntity.AddEntityProperty(PlayerEntityProperties.JUMP_NORMAL);

            //Setting start position for death
            m_playerStartPosition = transform.position;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Override of automaton update function for extended functionality
        override protected void Update()
        {
            if (m_playerEntity.HasProperty(PlayerEntityProperties.DYING))
            {
                SetState(new Death_PlayerState(this));
            }

            base.Update();

            // do stuff we want to do in all states here (e.g. collsion / adding velocity / death states)
            if (IsColliding())
            {
                m_velocity = new Vector3(0.0f, m_velocity.y, 0.0f);
            }

            transform.position += m_velocity * Time.deltaTime;
        }

        bool IsColliding()
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (m_playerCollider.bounds.extents.y - m_groundPadding), transform.position.z),
            new Vector3(m_velocity.x, 0.0f, m_velocity.z).normalized, out m_collisionHitInfo, m_playerCollider.bounds.extents.x * m_collisionRayLengthMultiplyer))
            {
                return m_collisionHitInfo.collider.isTrigger ? false : true;    //if collider is a trigger, ignore it and return false
            }
            return false;
        }

        //Public because it will only be called in certain states
        public bool IsGrounded()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out m_groundedHitInfo, m_playerCollider.bounds.extents.y))
            {
                return m_groundedHitInfo.collider.isTrigger ? false : true; //if stood on trigger, ignore it and return false
            }
            return false;
        }

        public void OnBoxFinishedMoving()
        {

        }

        #region UNITY COLLISIONS
        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Movable")
            {
                m_interactablesInRange.Add(other.transform);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Movable")
            {
                foreach (Transform t in m_interactablesInRange)
                {
                    if (t.GetInstanceID() == other.transform.GetInstanceID())
                    {
                        if (t == m_closestInteractable && m_state.GetType() == typeof(Pushing_PlayerState))
                        {
                            SetState(new Default_PlayerState(this));
                        }
                        m_interactablesInRange.Remove(t);
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
