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

    [RequireComponent(typeof(PlayerAnimator))]
    [RequireComponent(typeof(Projectile.ProjectileHandler))]
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
        [Header("Aiming")]
        [SerializeField] float m_aimingMaxSpeed = 2.0f;
        [SerializeField] float m_aimingAcceleration = 15.0f;
        [SerializeField] float m_aimingDeceleration = 15.0f;
        [Header("Modified Movment")]
        [SerializeField] float m_iceAcceleration = 1f;
        [SerializeField] float m_iceDeceleration = 1f;
        [SerializeField] float m_maxIceSpeed = 2.0f;
        [Header("Air Movement")]
        [SerializeField] float m_aerialAccelleration = 5.0f;
        [SerializeField] float m_gravity = 9.81f;
        [SerializeField] float m_jumpVelocity = 4.5f;
        [SerializeField] float m_highJumpVelocity = 9.5f;
        [Header("Collision")]
        [SerializeField] float m_maxClimbableIncline = 45.0f;
        [SerializeField] float m_heightPadding = 0.1f;  //How far from the floor the ray should start
        [SerializeField] float m_groundOverlapPadding = 0.1f;   //How far the player can sink before overlap recovery takes place
        [SerializeField] int m_numHorizontalRays = 3;
        [SerializeField] int m_numVerticalRays = 3;
        [SerializeField] float m_skinWidth = 0.2f; // the distance from the outside of the object the rays start

        //player stats (not editor accessible)
        Vector3 m_playerStartPosition;
        Vector3 m_velocity = Vector3.zero;
        Vector3 m_direction;
        Collider m_playerCollider;
        GameCore.System.State m_previousGroundState;

        //aim state / projectiles
        Projectile.ProjectileHandler m_projectileHandler;

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

        //Dialogue
        List<Transform> m_speakersInRange = new List<Transform>();

        #region PUBLIC ACCESSORS
        //player stats, Mutable
        public Vector3 Velocity { get => m_velocity; set => m_velocity = value; }
        public Vector3 Direction { get => m_direction; set => m_direction = value; }
        //player stats, getters only
        public float MaxSpeed { get => m_maxSpeed; }
        public float WalkingAcceleration { get => m_walkingAcceleration; }
        public float WalkingDeceleration { get => m_walkingDeceleration; }
        public float AimingMaxSpeed { get => m_aimingMaxSpeed; }
        public float AimingAcceleration { get => m_aimingAcceleration; }
        public float AimingDeceleration { get => m_aimingDeceleration; }
        public float IceAcceleration { get => m_iceAcceleration; }
        public float IceDeceleration { get => m_iceDeceleration; }
        public float IceMaxSpeed { get => m_maxIceSpeed; }
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
        public PlayerAnimator Animator { get => m_Animator; }
        public Projectile.ProjectileHandler Projectile { get => m_projectileHandler; }
        //Get and Settable
        public GameCore.System.State PreviousGroundState { get => m_previousGroundState; set => m_previousGroundState = value; }
        public Transform ClosestInteractable { get => m_closestInteractable; set => m_closestInteractable = value; }
        public List<Transform> InteractablesInRange { get => m_interactablesInRange; set => m_interactablesInRange = value; }   //to be removed, but unsure if we'll have other interactibles so may as well leave it
        public List<Transform> SpeakersInRange { get => m_speakersInRange; set => m_speakersInRange = value; }
        public AnimationCurve PushMovementCurve { get => m_pushMovementCurve; }
        #endregion

        private void Awake()
        {
            SetState(new Default_PlayerState(this));
            if (!TryGetComponent<Collider>(out m_playerCollider))
            {
                Debug.LogError("No collider attached to the player!");
            }
            m_Animator = GetComponent<PlayerAnimator>(); //requried component, should be safe
            m_projectileHandler = GetComponent<Projectile.ProjectileHandler>(); //requried component, should be safe

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

            //Dialogue trigger
            if (Input.GetButtonDown("Interact") && m_speakersInRange.Count > 0 && m_state.GetType() != typeof(Dialogue_PlayerState))
            {
                SetState(new Dialogue_PlayerState(this));
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
            Vector3 rayStart = transform.position;
            Vector3 rayDirection = m_velocity.normalized; //new Vector3(m_velocity.x, 0, m_velocity.z).normalized;
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
            else if (m_velocity.x != 0 && m_velocity.z != 0)
            {
                horizontalRaySpacing /= horizontalRaySpacing.magnitude;
            }
            horizontalRaySpacing /= m_numHorizontalRays;
            horizontalRaySpacing -= horizontalRaySpacing * m_skinWidth; //adding a variable width from the outer edge of the collider (should help with the player "sticking" to things

            rayStart -= (verticalRaySpacing * (m_numHorizontalRays/ 2)) + ((horizontalRaySpacing) * (m_numVerticalRays / 2)); //ray start is currently at center of the object and needs to be offset
            rayStart.y += m_heightPadding;  //Rasing off the ground by padding value

            for (int x = 0; x < m_numHorizontalRays; ++x)
            {
                for (int y = 0; y < m_numVerticalRays; ++y)
                {

                    Debug.DrawLine(rayStart, (rayStart + (rayDirection * m_playerCollider.bounds.extents.z)));  //Uncomment for debug rays

                    if (Physics.Raycast(rayStart, rayDirection, out m_collisionHitInfo, m_playerCollider.bounds.extents.z))
                    {
                        return true;
                    }
                    rayStart += verticalRaySpacing;
                }
                rayStart += horizontalRaySpacing;
                rayStart.y = (transform.position.y - verticalRaySpacing.y) + m_heightPadding;
            }

            return false; 
        }

        //Public because it will only be called in certain states
        public bool IsGrounded()
        {
            bool collided = false;
            Vector3 rayStart = transform.position;
            Vector3 xRaySpacing = transform.right * (m_playerCollider.bounds.extents.x / 3);
            Vector3 zRaySpacing = transform.forward * (m_playerCollider.bounds.extents.z / 3);

            rayStart.y = transform.position.y;
            RaycastHit collisionInfo;

            rayStart -= (xRaySpacing * (3 / 2)) + (zRaySpacing * (3 / 2));

            for (int x = 0; x < 3; ++x)
            {
                for (int z = 0; z < 3; ++z)
                {
                    //Debug.DrawLine(rayStart, rayStart + (-transform.up * m_playerCollider.bounds.extents.y));
                    if (Physics.Raycast(rayStart, -transform.up, out collisionInfo, m_playerCollider.bounds.extents.y + m_groundOverlapPadding))
                    {
                        //if it's the first ray, set it to the first result regardless, as we cannot compare null variables
                        if (x == 0 && z == 0)
                        {
                            m_groundedHitInfo = collisionInfo;
                        }
                        //for slope detection we want the shortest ray to be the hit info 
                        else if (collisionInfo.distance < m_groundedHitInfo.distance)
                        {
                            m_groundedHitInfo = collisionInfo;
                        }
                        collided = true;
                    }
                    rayStart += zRaySpacing;
                }
                rayStart += xRaySpacing;
                rayStart -= zRaySpacing * (3);
            }

            return collided;

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
            if (other.tag == "NPC")
            {
                m_speakersInRange.Add(other.transform);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "NPC")
            {
                foreach (Transform t in m_speakersInRange)
                {
                    if (t.GetInstanceID() == other.transform.GetInstanceID())
                    {
                        m_speakersInRange.Remove(t);
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
