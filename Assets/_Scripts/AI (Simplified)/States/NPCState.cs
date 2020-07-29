using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCState
{
    private Animation m_stateAnimation;
    protected Animator m_animator;
    protected LetterBox m_letterBox;

    protected bool m_isPlayerInView = true;

    //protected Camera m_playerCamera;

    protected GameObject m_player;

    protected float m_viewRadius;

    protected Vector3 m_targetVec = Vector3.zero;

    protected bool m_endOfConvo = false;

    /// <summary>
    /// Constructor for the NPC base state 
    /// </summary>
    /// <param name="animation">Provide state animation</param>
    public NPCState(Animator animation, string animationName)
    {
        animation.Play(animationName);
    }

    /// <summary>
    /// Call each frame to update and perform NPC related operations
    /// </summary>
    public virtual void StateUpdate() { }

    public bool EndOfConversation() { return m_endOfConvo; }
}
