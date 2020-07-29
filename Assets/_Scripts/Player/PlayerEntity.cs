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

    //probably don't need an enum for just two items, however it does make it more expandable in future
    public enum PlayerEquipableItems
    {
        SPELL_QUILL = 0,
        ERASER
    }

    public enum PlayerGroundStates
    {
        DEFAULT         = 0,
        AIMING          = 1
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
        [SerializeField] Shader m_highlightShader;
        [Header("Modified Movment")]
        [SerializeField] float m_iceAcceleration = 1f;
        [SerializeField] float m_iceDeceleration = 1f;
        [SerializeField] float m_maxIceSpeed = 2.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float m_slidingAnimationSpeedThreshold = 0.4f;
        [Header("Air Movement")]
        [SerializeField] float m_aerialAcceleration = 5.0f;
        [SerializeField] float m_aerialDeceleration = 5.0f;
        [SerializeField] float m_gravity = 9.81f;
        [SerializeField] float m_jumpVelocity = 4.5f;
        [SerializeField] float m_highJumpVelocity = 9.5f;
        [SerializeField] float m_jumpHeldMultiplier = 1.5f;   //the amount of velocity added per second to the y axis if jump is held down, added over time, not set like jump vel
        [SerializeField]float m_canJumpTimerLimit = 0.25f; //the time the player can be off the ground for and still jump
        [Header("Collision")]
        [SerializeField] float m_playerRadius = 1;
        [SerializeField] float m_playerHeight = 4;
        [SerializeField] float m_maxClimbableIncline = 45.0f;
        [SerializeField] float m_heightPadding = 0.1f;  //How far from the floor the ray should start
        [SerializeField] float m_groundOverlapPadding = 0.1f;   //How far the player can sink before overlap recovery takes place
        [SerializeField] int m_numHorizontalRays = 3;
        [SerializeField] int m_numVerticalRays = 3;
        [SerializeField] int m_numGroundedRays = 3;
        [SerializeField] float m_skinWidth = 0.2f; // the distance from the outside of the object the rays start
        [SerializeField] float m_additionalRayLength = 0.3f;
        [SerializeField] [Range(0.0f, 1.0f)] float m_groundRayStartHeight = 0.25f; //percentage of the player colider height where ground check rays will start
        [Header("Audio")]
        [SerializeField] string m_fallingEvent;
        [SerializeField] string m_hoveringEvent;
        [SerializeField] string m_hittingGroundEvent;
        [Header("Properties")]
        [SerializeField] PlayerEntityProperties m_playerEntityProperties;
        [SerializeField] public RFX4_EffectEvent m_spellSfx;

        //For Dialogue states to access
        [Header("UI Elements to Hide in Dialogue")]
        [SerializeField]
        GameObject[] UIsToHideInDialogue;

        //player stats (not editor accessible)
        bool m_grounded = true;
        bool m_canJump = true;
        bool m_hasJumped = false;
        float m_canJumpTimer = 0;
        
        Vector3 m_playerStartPosition;
        Vector3 m_velocity = Vector3.zero;
        Vector3 m_groundAddedVelocity = Vector3.zero;
        Vector3 m_direction;
        Vector3 m_position;
        Vector3 m_positionLastFrame;
        float m_time;
        PlayerGroundStates m_previousGroundState;
        PlayerEquipableItems m_equipedItem;
        GameUI.SpellWheel m_spellWheel;
        Transform m_reticle;

        //aim state / projectiles
        Projectile.ProjectileHandler m_projectileHandler;

        //Collision variables
        RaycastHit m_groundedHitInfo;
        RaycastHit m_collisionHitInfo;
        //transform the player is stood on
        Transform m_ground;
        Vector3 m_oldGroundPosition;
        Vector3 m_newGroundPosition;

        //Interacting transforms 
        List<Transform> m_interactablesInRange = new List<Transform>();
        Transform m_closestInteractable = null;

        //Player anim
        PlayerAnimator m_animator;

        //Dialogue
        List<Transform> m_speakersInRange = new List<Transform>();

        #region PUBLIC ACCESSORS
        //player stats, Mutable
        public Vector3 Velocity { get => m_velocity; set => m_velocity = value; }
        public Vector3 Direction { get => m_direction; set => m_direction = value; }
        public Vector3 Position { get => transform.position; set { m_position = value; m_positionLastFrame = value; transform.position = value; } }
        //player stats, getters only
        public bool Grounded { get => m_grounded; }
        public bool CanJump { get => m_canJump; }
        public bool HasJumped { get => m_hasJumped; set => m_hasJumped = value; }
        public float MaxSpeed { get => m_maxSpeed; }
        public float WalkingAcceleration { get => m_walkingAcceleration; }
        public float WalkingDeceleration { get => m_walkingDeceleration; }
        public float AimingMaxSpeed { get => m_aimingMaxSpeed; }
        public float AimingAcceleration { get => m_aimingAcceleration; }
        public float AimingDeceleration { get => m_aimingDeceleration; }
        public Shader HighlightShader { get => m_highlightShader; }
        public float IceAcceleration { get => m_iceAcceleration; }
        public float IceDeceleration { get => m_iceDeceleration; }
        public float IceMaxSpeed { get => m_maxIceSpeed; }
        public float AerialAcceleration { get => m_aerialAcceleration; }
        public float AerialDeceleration { get => m_aerialDeceleration; }
        public float JumpHeldMutliplier { get => m_jumpHeldMultiplier; }
        public float CanJumpTimer { get => m_canJumpTimer; set => m_canJumpTimer = value; }
        public float PushSpeed { get => m_pushingSpeed; }
        public float Gravity { get => m_gravity; }
        public float JumpVelocity { get => m_jumpVelocity; }
        public float HighJumpVelocity { get => m_highJumpVelocity; }
        public float MaxClimableAngle { get => m_maxClimbableIncline; }
        public float GroundOverlapPadding { get => m_groundOverlapPadding; }
        public RaycastHit GroundHitInfo { get => m_groundedHitInfo; }
        public Vector3 PlayerStartPosition { get => m_playerStartPosition; }
        public PlayerAnimator Animator { get => m_animator; }
        public Projectile.ProjectileHandler Projectile { get => m_projectileHandler; }
        public GameUI.SpellWheel SpellWheel { get => m_spellWheel; }
        //Audio stats for respawning
        public string FallingAudioEvent { get => m_fallingEvent; }
        public string HoveringAudioEvent { get => m_hoveringEvent; }
        public string HitGroundAudioEvent { get => m_hittingGroundEvent; }
        //Get and Settable
        public PlayerGroundStates PreviousGroundState { get => m_previousGroundState; set => m_previousGroundState = value; }
        public Transform ClosestInteractable { get => m_closestInteractable; set => m_closestInteractable = value; }
        public Transform Reticle { get => m_reticle; }
        public List<Transform> InteractablesInRange { get => m_interactablesInRange; set => m_interactablesInRange = value; }   //to be removed, but unsure if we'll have other interactibles so may as well leave it
        public List<Transform> SpeakersInRange { get => m_speakersInRange; set => m_speakersInRange = value; }
        public PlayerEquipableItems EquipedItem { get => m_equipedItem; set { m_equipedItem = value; m_projectileHandler.ChangeProjectileStatsBasedOnItem(value); } }   //changes projectile stats too
        public AnimationCurve PushMovementCurve { get => m_pushMovementCurve; }
        public Vector3 GroundAddedVelocity { get => m_groundAddedVelocity; } //the velocity of the thing we're stood on
        public float SlidingAnimationSpeedThreshold { get => m_slidingAnimationSpeedThreshold; }
        #endregion

        static string s_spellWheelTag = "UI_SpellWheel";
        const string CROSSHAIR_FILEPATH = "Prefabs/UI/SpellWheel/UI_Reticle";

        private void Awake()
        {
            Transform canvas = GameObject.Find("Canvas").transform;

            GameObject reticle = Instantiate(Resources.Load<GameObject>(CROSSHAIR_FILEPATH), canvas);

            m_reticle = reticle.transform;

            m_spellWheel = GameObject.FindGameObjectWithTag(s_spellWheelTag).GetComponentInChildren<GameUI.SpellWheel>();
            m_animator = GetComponent<PlayerAnimator>(); //requried component, should be safe
            m_projectileHandler = GetComponent<Projectile.ProjectileHandler>(); //requried component, should be safe

            EquipedItem = PlayerEquipableItems.SPELL_QUILL;

            SetState(new Default_PlayerState(this));
            //Setting start position for death
            m_playerStartPosition = transform.position;
            m_position = transform.position;
            m_positionLastFrame = transform.position;
        }

        // Override of automaton update function for extended functionality
        override protected void Update()
        {
            //Directional input
            Vector3 forwardMovement = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * Input.GetAxis("Vertical"); // removing the y component from the camera's forward vector
            Vector3 rightMovement = Camera.main.transform.right * Input.GetAxis("Horizontal");
            m_direction = (forwardMovement + rightMovement).normalized;

            //First check if death state is triggered to save time / ensure the player cannot do something if they are alread dead
            if (HasProperty(PlayerEntityProperties.DYING) && m_state.GetType() != typeof(Death_PlayerState))
            {
                SetState(new Death_PlayerState(this));
            }

            if (IsFalling())
            {
                SetState(new Falling_PlayerState(this));
            }

            if (IsAbleToJump() && Input.GetButtonDown("Jump"))
            {
                m_hasJumped = true;
                if (m_state.GetType() == typeof(Aiming_PlayerState))
                {
                    ((Aiming_PlayerState)m_state).ResetAimedAt();
                }
                SetState(new Jumping_PlayerState(this));
            }

            if (IsAbleToAim() && (Input.GetButtonDown("Aim") || Input.GetAxisRaw("Aim") == 1))
            {
                SetState(new Aiming_PlayerState(this, m_spellSfx));
            }

            //Dialogue trigger
            if (Input.GetButtonDown("Interact") && m_speakersInRange.Count > 0 && m_state.GetType() != typeof(Dialogue_PlayerState))
            {
                SetState(new Dialogue_PlayerState(this));
            }

            //Here we lerp from the position at the start of fixed update to the position after we update phyiscs by a step based on the time till the next fixed update
            m_time += Time.deltaTime;
            transform.position = Vector3.Lerp(m_positionLastFrame, m_position, m_time / Time.fixedDeltaTime);
        }

        private void FixedUpdate()
        {
            base.Update();

            m_grounded = IsGrounded();
            CheckCollisions();

            if (m_ground != null && m_grounded)
            {
                m_newGroundPosition = m_ground.transform.position;

                //if the ground has moved since the last frame, add that movement to the player
                if (m_newGroundPosition != m_oldGroundPosition)
                {
                    //some times objects moving will add a large amount of velocity to the player, if this is the case, the player likely wouldn't be able to stay on them anyway
                    //also fixes a bug where the way this extra movment was calculated with the seesaw would shoot the player upwards
                    m_groundAddedVelocity = m_newGroundPosition - m_oldGroundPosition;
                    if (m_groundAddedVelocity.magnitude < 20)
                    {
                        m_velocity += (m_newGroundPosition - m_oldGroundPosition);
                    }
                }
                m_oldGroundPosition = m_newGroundPosition;
            }
            else
            {
                m_newGroundPosition = Vector3.zero;
                m_oldGroundPosition = Vector3.zero;
            }

            m_position += m_velocity;
            m_positionLastFrame = transform.position;
            m_time = 0;
        }

        public void CheckCollisions()
        {
            Vector3 rayStart = transform.position;
            Vector3 rayDirection = new Vector3(m_velocity.x, 0, m_velocity.z).normalized;
            Vector3 horizontalRaySpacing = Vector3.Cross(rayDirection, transform.up);    // get perpendicular vector to our direction for spacing
            Vector3 verticalRaySpacing = new Vector3(0, m_playerHeight / m_numVerticalRays, 0);

            //rayStart = transform.position + ((m_playerCollider.bounds.extents.z /2) * rayDirection);  //enable if you want rays to start at the outer edge of the player
            horizontalRaySpacing *= (m_playerRadius * 2);
           
            horizontalRaySpacing /= m_numHorizontalRays;
            horizontalRaySpacing -= horizontalRaySpacing * m_skinWidth; //adding a variable width from the outer edge of the collider (should help with the player "sticking" to things

            rayStart -= (verticalRaySpacing * (m_numHorizontalRays/ 2)) + ((horizontalRaySpacing) * (m_numVerticalRays / 2)); //ray start is at center of the object and needs to be offset
            rayStart.y += m_heightPadding;  //Rasing off the ground by padding value

            for (int x = 0; x < m_numHorizontalRays; ++x)
            {
                for (int y = 0; y < m_numVerticalRays; ++y)
                {

                    Debug.DrawLine(rayStart, (rayStart + (rayDirection * (m_playerRadius + m_additionalRayLength))));  //Uncomment for debug rays

                    if (Physics.Raycast(rayStart, rayDirection, out m_collisionHitInfo, m_playerRadius + m_additionalRayLength))
                    {

                        //if (!m_grounded)
                        //{
                        //    m_velocity = new Vector3(0, m_velocity.y, 0);
                        //}
                        //only count the ray as hit if it is above the max climable incline
                        //if it's an acceptable angle it'll be handled by the movement state
                        if (Vector3.Angle(m_collisionHitInfo.normal, Vector3.up) > m_maxClimbableIncline)
                        {
                            Debug.DrawRay(m_collisionHitInfo.point, m_collisionHitInfo.normal * 10, Color.cyan);
                            // if hit, modify movement to use the perpendicular vector (-up because we want the players right, not the walls right)
                            Vector3 wallCross = Vector3.Cross(m_collisionHitInfo.normal, -Vector3.up).normalized;

                            Vector3 horizontalWallRaySpacing = -m_collisionHitInfo.normal;
                            horizontalWallRaySpacing = (horizontalWallRaySpacing * (m_playerRadius * 2)) / m_numHorizontalRays;

                            Vector3 wallRayStart = transform.position - ((horizontalWallRaySpacing * (m_numHorizontalRays / 2)) + (verticalRaySpacing * (m_numVerticalRays / 2)));

                            if (Vector3.Angle(wallCross, m_velocity.normalized) > 90)
                                wallCross *= -1;

                            for (int j = 0; j < m_numHorizontalRays; ++j)
                            {
                                for (int k = 0; k < m_numVerticalRays; ++k)
                                {
                                    Debug.DrawRay(wallRayStart, wallCross * (m_playerRadius + m_additionalRayLength), Color.red);
                                    //if we come to a corner, stop completely
                                    if (Physics.Raycast(wallRayStart, wallCross, m_playerRadius + m_additionalRayLength))
                                    {
                                        m_velocity = new Vector3(0, m_velocity.y, 0);
                                        return;
                                    }
                                    wallRayStart += verticalRaySpacing;
                                }
                                wallRayStart += horizontalWallRaySpacing;
                                wallRayStart.y = (transform.position.y - verticalRaySpacing.y);
                            }
                            //don't want to take the y velocity into account in these calculations
                            Vector3 nonVerticalMovement = new Vector3(m_velocity.x, 0, m_velocity.y);
                            m_velocity.x = wallCross.x * (nonVerticalMovement.magnitude * (Vector3.Angle(-m_collisionHitInfo.normal, transform.forward) / 90));  //should be a faster movement if the angle is lower
                            m_velocity.z = wallCross.z * (nonVerticalMovement.magnitude * (Vector3.Angle(-m_collisionHitInfo.normal, transform.forward) / 90));  //dividing by 90 as if we're at 90 or greater it should be parallel and therefore moving at max speed

                            return;
                        }
                    }
                    rayStart += verticalRaySpacing;
                }
                rayStart += horizontalRaySpacing;
                rayStart.y = (transform.position.y - verticalRaySpacing.y) + m_heightPadding;
            }
        }

        //Public because it will only be called in certain states
        /// <summary>
        /// Casts rays out underneath the player collider and returns true if any hit, if they are hit and the player is close to the  ground or overlapping, they will be moved to the point hit + y extents.
        /// also moves the player if the ground moves
        /// </summary>
        /// <returns>ground was hit</returns>
        public bool IsGrounded()
        {
            if (m_state.GetType() == typeof(Jumping_PlayerState))
            {
                m_grounded = false;
                return false;
            }
            bool collided = false;
            //sets ray start point to a percentage of the total height based on the value m_groundRayStartHeight
            Vector3 rayStart = (transform.position - (Vector3.up * (m_playerHeight / 2)) + (m_groundRayStartHeight * (Vector3.up * (m_playerHeight))));
            //ray length is equal to the distance from the ray start point to the players feet (lowest point of the collider) minus our y velocity
            float rayLength = (rayStart.y - (transform.position.y - m_playerHeight/ 2) + m_groundOverlapPadding) - m_velocity.y;
            Vector3 xRaySpacing = transform.right * ((m_playerRadius * 2) / m_numGroundedRays);
            Vector3 zRaySpacing = transform.forward * ((m_playerRadius * 2) / m_numGroundedRays);

            //rayStart.y = transform.position.y;
            RaycastHit collisionInfo;
            float distance = 100;

            rayStart -= (xRaySpacing * (m_numGroundedRays / 2)) + (zRaySpacing * (m_numGroundedRays / 2));


            for (int x = 0; x < m_numGroundedRays; ++x)
            {
                for (int z = 0; z < m_numGroundedRays; ++z)
                {
                    Debug.DrawLine(rayStart, rayStart + (-transform.up * rayLength));
                    //Subtracting velocity y from the length (velocity y is negative when we're falling) to check if we would land next frame, if so count it as landing this frame.
                    if (Physics.Raycast(rayStart, -transform.up, out collisionInfo, rayLength))
                    {
                        //if it's the first ray, set it to the first result regardless, as we cannot compare null variables
                        if (x == 0 && z == 0)
                        {
                            m_groundedHitInfo = collisionInfo;
                        }
                        //for slope detection we want the shortest ray to be the hit info 
                        else if (collisionInfo.distance < distance)
                        {
                            m_groundedHitInfo = collisionInfo;
                            distance = collisionInfo.distance;

                            m_position = new Vector3(m_position.x, m_groundedHitInfo.point.y + (m_playerHeight / 2), m_position.z);
                            m_velocity.y = 0;
                        }
                        collided = true;
                    }
                    rayStart += zRaySpacing;
                }
                rayStart += xRaySpacing;
                rayStart -= zRaySpacing * m_numGroundedRays;
            }

            //I realise this means this function does more than one thing, which surely is a cardinal sin, however I couldn't find a better place to execute this just yet
            //Nor could I think of a better name for this function
            if (collided)
            {
                GameCore.Spells.Enchantable enchantable;

                if (m_groundedHitInfo.transform.TryGetComponent<GameCore.Spells.Enchantable>(out enchantable))
                {
                    if (m_groundedHitInfo.transform.GetComponent<GameCore.Spells.MagicProfile>().GetMagicFingerprint().magicState.temperature == GameCore.Spells.SpellState.COUNTERSPELLED)
                    {
                        AddEntityProperty(PlayerEntityProperties.SLIDING);
                    }
                    else
                    {
                        //don't worry, it does check to make sure it has the property before removing 
                        RemoveEntityProperty(PlayerEntityProperties.SLIDING);
                    }
                }
                //if we didn't get the component, must not be frozen
                else
                {
                    RemoveEntityProperty(PlayerEntityProperties.SLIDING);
                }

                //update the position to the point on the ground hit + half of the player's height
                transform.position = new Vector3(transform.position.x, m_groundedHitInfo.point.y + (m_playerHeight / 2), transform.position.z);

                //if the ground has changed, change the ground and the old position
                if (m_ground == null || m_ground != m_groundedHitInfo.transform)
                {
                    m_ground = m_groundedHitInfo.transform;
                    m_oldGroundPosition = m_ground.position;
                    m_newGroundPosition = m_ground.position;
                }
            }
            else
            {
                m_ground = null;
            }
            m_grounded = collided;
            return collided;
        }

        public void Respawn(Vector3 position)
        {
            m_velocity = Vector3.zero;
            m_position = position;
            m_positionLastFrame = position;
            transform.position = position;
        }

        //for Dialogue state to access
        public void ShowOtherUIsInDialogue(bool val)
        {
            foreach(GameObject g in UIsToHideInDialogue)
            {
                g.SetActive(val);
            }
        }

        public bool IsAbleToJump()
        {
            if (m_grounded && !HasProperty(PlayerEntityProperties.DYING))
                return true;
            if (m_state.GetType() != typeof(Falling_PlayerState) || m_hasJumped)
                return false;

            m_canJumpTimer += Time.deltaTime;
            if (m_canJumpTimer > m_canJumpTimerLimit)
                return false;
            else
                return true;
        }

        bool IsAbleToAim()
        {
            return m_state.GetType() == typeof(Default_PlayerState);
        }

        bool IsFalling()
        {
            return !m_ground && (m_state.GetType() != typeof(Falling_PlayerState) && m_state.GetType() != typeof(Jumping_PlayerState)) && !HasProperty(PlayerEntityProperties.DYING);
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

        public void OnTriggerStay(Collider other)
        {
            if (!other.isTrigger && (other.transform.position - transform.position).magnitude > m_playerRadius)
            {
                //m_velocity = Vector3.zero;
                Position += (transform.position - other.transform.position) * Time.deltaTime;
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
