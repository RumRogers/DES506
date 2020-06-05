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
        PUSHING,
        DYING
    }

    public class PlayerAnimator : GameCore.System.Automaton
    {
        Animator m_playerAnimator;

        PlayerAnimationProperties m_playerAnimProperties;

        [SerializeField] Animation m_idleAnim;
        [SerializeField] Animation m_runnningAnim;
        [SerializeField] Animation m_jumpingAnim;
        [SerializeField] Animation m_fallingAnim;
        [SerializeField] Animation m_pushingAnim;

        #region PUBLIC ACCESSORS
        public Animation Idle { get => m_idleAnim; }
        public Animation Running { get => m_runnningAnim; }
        public Animation Jumping { get => m_jumpingAnim; }
        public Animation Falling { get => m_fallingAnim; }
        public Animation Pushing { get => m_pushingAnim; }
        public PlayerAnimationProperties PlayerAnimProperties { get => m_playerAnimProperties; }
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            SetProperty(PlayerAnimationProperties.IDLE);
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