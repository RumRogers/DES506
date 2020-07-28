using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateDefault : NPCState
{
    private Transform m_NPCTransform; 

    /// <summary>
    /// Constructor for switching states
    /// </summary>
    /// <param name="animation"></param>
    public NPCStateDefault(Animator anim) : base(anim)
    {
        m_letterBox.TurnOff();
    }

    /// <summary>
    /// This is the constructor just for start up, not for switching states
    /// </summary>
    /// <param name="animation">State related animation</param>
    /// <param name="rotSpeed">The NPCs rotation speed</param>
    /// <param name="player">A reference to the player game object</param>
    /// <param name="viewRad">The radius/distance that the NPC can spot the player in</param>
    /// <param name="letterBox">The current active letter box script attached to camera</param>
    public NPCStateDefault(Animator animation, GameObject player, LetterBox letterBox) : base(animation)
    {
        m_letterBox = letterBox;
        m_NPCTransform = transform;
    }

    /// <summary>
    /// Call each frame to update and perform NPC related operations (Default state override)
    /// </summary>
    public override void StateUpdate()
    {
        base.StateUpdate();

    }
}