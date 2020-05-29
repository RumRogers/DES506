using System.Collections;
using System.Collections.Generic;
using GameCore.Rules;
using UnityEngine;

namespace Player
{
    [System.Flags]
    public enum PlayerEntityProperties
    {
        MOVEABLE        = 1 << 0,
        PLAYABLE        = 1 << 1,
        DYING           = 1 << 2,
        CAN_JUMP        = 1 << 3,
        JUMP_NORMAL     = 1 << 4,
        JUMP_HIGH       = 1 << 5,
        CAN_DROWN       = 1 << 6,
        SLIDING         = 1 << 7
    }

    public class PlayerEntity : GameCore.Rules.MutableEntity
    {
        //Player stats (editor variables)
        [Header("Pushing")]
        [SerializeField] AnimationCurve m_pushMovementCurve = new AnimationCurve();
        [SerializeField] float m_pushingSpeed = 2.0f;
        [Header("Ground Movement")]
        [SerializeField] float m_maxSpeed = 2.0f;
        [SerializeField] float m_walkingAcceleration = 15.0f;
        [SerializeField] float m_walkingDeceleration = 15.0f;
        [Header("Modified Movment")]
        [SerializeField] float m_iceAcceleration = 1f;
        [SerializeField] float m_iceDeceleration = 1f;
        [Header("Air Movement")]
        [SerializeField] float m_aerialAccelleration = 5.0f;
        [SerializeField] float m_gravity = 9.81f;
        [SerializeField] float m_jumpVelocity = 4.5f;
        [SerializeField] float m_highJumpVelocity = 9.5f;
        [Header("Collision")]
        [SerializeField] float m_maxClimbableIncline = 45.0f;
        [SerializeField] float m_groundPadding = 0.1f;  //How far from the floor the ray should start
        [SerializeField] float m_collisionRayLengthMultiplyer = 0.7f;   //Determines what percentage of the player's bounds to use as ray length
        [SerializeField] float m_groundOverlap = 0.1f;   //How far the player can sink before overlap recovery takes place
        [Header("'Complex' Collision")]
        [SerializeField] bool m_useComplexCollisions = true;
        [SerializeField] int m_numHorizontalRays = 3;
        [SerializeField] int m_numVerticalRays = 3;
        [SerializeField] float m_rayLength = 0.1f;
        [SerializeField] float m_skinWidth = 0.2f; // the distance from the outside of the object the rays start

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

        //Player Entity
        [SerializeField]PlayerEntityProperties m_playerEntityProperties;

        //Player anim
        PlayerAnimator m_Animator;

        #region PUBLIC ACCESSORS
        //player stats, Mutable
        public Vector3 Velocity { get => m_velocity; set => m_velocity = value; }
        public Vector3 Direction { get => m_direction; set => m_direction = value; }
        //player stats, getters only
        public float MaxSpeed { get => m_maxSpeed; }
        public float WalkingAcceleration { get => m_walkingAcceleration; }
        public float WalkingDeceleration { get => m_walkingDeceleration; }
        public float IceAcceleration { get => m_iceAcceleration; }
        public float IceDeceleration { get => m_iceDeceleration; }
        public float AerialAccelleration { get => m_aerialAccelleration; }
        public float PushSpeed { get => m_pushingSpeed; }
        public float Gravity { get => m_gravity; }
        public float JumpVelocity { get => m_jumpVelocity; }
        public float HighJumpVelocity { get => m_highJumpVelocity; }
        public float MaxClimableAngle { get => m_maxClimbableIncline; }
        public float GroundOverlapPadding { get => m_groundOverlap; }
        public Collider PlayerCollider { get => m_playerCollider; }
        public RaycastHit GroundHitInfo { get => m_groundedHitInfo; }
        public Vector3 PlayerStartPosition { get => m_playerStartPosition; }
        public PlayerAnimator Animator { get => m_Animator; }
        //Pushing Getters
        public Transform ClosestInteractable { get => m_closestInteractable; set => m_closestInteractable = value; }
        public List<Transform> InteractablesInRange { get => m_interactablesInRange; set => m_interactablesInRange = value; }
        public AnimationCurve PushMovementCurve { get => m_pushMovementCurve; }
        #endregion

        private void Awake()
        {
            SetState(new Default_PlayerState(this));
            m_playerCollider = GetComponent<Collider>();
            m_Animator = GetComponent<PlayerAnimator>();

            //setting properties
            AddEntityProperty(PlayerEntityProperties.CAN_JUMP);
            AddEntityProperty(PlayerEntityProperties.JUMP_NORMAL);

            //Setting start position for death
            m_playerStartPosition = transform.position;
        }

        // Override of automaton update function for extended functionality
        override protected void Update()
        {
            //First check if death state is triggered to save time / ensure the player cannot do something if they are alread dead
            if (HasProperty(PlayerEntityProperties.DYING))
            {
                SetState(new Death_PlayerState(this));
            }

            base.Update();

            if (IsColliding())
            {
                m_velocity = new Vector3(0.0f, m_velocity.y, 0.0f);
            }

            transform.position += m_velocity * Time.deltaTime;
        }

        public bool IsColliding()
        {
            if (!m_useComplexCollisions)
            {
                //Debug.DrawLine(transform.position, transform.position + (new Vector3(m_velocity.x, 0.0f, m_velocity.z).normalized * m_playerCollider.bounds.extents.x * m_collisionRayLengthMultiplyer)); //Uncomment for debug rays
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (m_playerCollider.bounds.extents.y - m_groundPadding), transform.position.z),
                new Vector3(m_velocity.x, 0.0f, m_velocity.z).normalized, out m_collisionHitInfo, m_playerCollider.bounds.extents.x * m_collisionRayLengthMultiplyer))
                {
                    return m_collisionHitInfo.collider.isTrigger ? false : true;    //if collider is a trigger, ignore it and return false
                }
                return false;
            }

            Vector3 rayStart = transform.position;
            Vector3 rayDirection = new Vector3(m_velocity.x, 0, m_velocity.z).normalized;
            Vector3 horizontalRaySpacing = Vector3.Cross(rayDirection, transform.up);    // get perpendicular vector to our direction for spacing
            Vector3 verticalRaySpacing = new Vector3(0, m_playerCollider.bounds.extents.y / m_numVerticalRays, 0);

            if (m_velocity.x != 0)
            {
                //rayStart = transform.position + ((m_playerCollider.bounds.extents.x /2) * (rayDirection));
                horizontalRaySpacing *= (m_playerCollider.bounds.extents.z * 2);
            }
            if (m_velocity.z != 0)
            {
                //rayStart = transform.position + ((m_playerCollider.bounds.extents.z /2) * rayDirection);  //enable if you want rays to start at the outer edge of the player
                horizontalRaySpacing *= (m_playerCollider.bounds.extents.x * 2);
            }
            if (m_velocity.x != 0 && m_velocity.z != 0)
            {
                horizontalRaySpacing /= horizontalRaySpacing.magnitude;
            }
            horizontalRaySpacing /= m_numHorizontalRays;
            horizontalRaySpacing -= horizontalRaySpacing * m_skinWidth; //adding a variable width from the outer edge of the collider (should help with the player "sticking" to things

            rayStart -= (verticalRaySpacing * (m_numHorizontalRays/ 2)) + ((horizontalRaySpacing) * (m_numVerticalRays / 2)); //ray start is currently at center of the object and needs to be offset
            rayStart.y += m_groundPadding;  //Rasing off the ground by padding value


            for (int x = 0; x < m_numHorizontalRays; ++x)
            {
                for (int y = 0; y < m_numVerticalRays; ++y)
                {

                    //Debug.DrawLine(rayStart, (rayStart + (rayDirection * m_rayLength)));  //Uncomment for debug rays

                    if (Physics.Raycast(rayStart, rayDirection, out m_collisionHitInfo, m_rayLength))
                    {
                        if(!m_collisionHitInfo.collider.isTrigger)
                        {
                            return true;
                        }
                    }
                    rayStart += verticalRaySpacing;
                }
                rayStart += horizontalRaySpacing;
                rayStart.y = (transform.position.y - verticalRaySpacing.y) + m_groundPadding;
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

        #region MUTABLE ENTITY IMPLEMENTATION

        public override void Can(string ruleObject)
        {
            Debug.LogError("IMPLEMENT PLAYER CAN (Josh hasn't impmented this yet)");
        }

        public override void Has(string ruleObject)
        {
            Debug.LogError("IMPLEMENT PLAYER HAS (Josh hasn't impmented this yet)");
        }

        public override void Is(string ruleObject)
        {
            Debug.LogError("IMPLEMENT PLAYER IS (Josh hasn't impmented this yet)");
        }

        public override void UndoCan(string lexeme)
        {
            base.UndoCan(lexeme);
        }

        public override void UndoHas(string lexeme)
        {
            base.UndoHas(lexeme);
        }

        public override void UndoIs(string lexeme)
        {
            base.UndoIs(lexeme);
        }
        #endregion

        #region PLAYER ENTITY PROPERTIES
        //CHANGING / CHECKING PROPERTIES

        public void ClearEntityProperties()
        {
            m_playerEntityProperties = 0;
        }

        /// <summary>
        /// Uses bit mask logic. Adds entity property to the player entity, Does not remove other properties when a new one is assigned. Player can have more than one property.
        /// </summary>
        /// <param name="property"></param>
        public void AddEntityProperty(PlayerEntityProperties property)
        {
            if (!HasProperty(property))
            {
                m_playerEntityProperties |= property;
            }
        }

        /// <summary>
        /// Uses bit mask logic. Removes entity property to the player entity, Does not modify other properties when one is removed. Player can have more than one property.
        /// </summary>
        /// <param name="property"></param>
        public void RemoveEntityProperty(PlayerEntityProperties property)
        {
            if (HasProperty(property))
            {
                m_playerEntityProperties &= ~property;
            }
        }

        /// <summary>
        /// Uses bit mask logic. Removes to remove and adds to add, does not modify other properties. Player can have more than one property.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <param name="toAdd"></param>
        public void ReplaceEntityProperty(PlayerEntityProperties toRemove, PlayerEntityProperties toAdd)
        {
            if (!HasProperty(toAdd) && HasProperty(toRemove))
            {
                m_playerEntityProperties &= ~toRemove;
                m_playerEntityProperties |= toAdd;
            }
        }

        public bool HasProperty(PlayerEntityProperties property)
        {
            return m_playerEntityProperties.HasFlag(property);
        }
        #endregion

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
