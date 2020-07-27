using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateDefault : NPCState
{
    private Vector3 m_playerToTarget = Vector3.zero;
    private Vector3 m_defaultDirection = Vector3.zero;
    private Transform m_NPCTransform; 
    private float m_rotationSpeed;

    /// <summary>
    /// Constructor for switching states
    /// </summary>
    /// <param name="animation"></param>
    public NPCStateDefault(Animation animation) : base(animation)
    {
        m_letterBox.TurnOff();

        m_targetVec = transform.forward;

        m_defaultDirection = transform.forward;

    }

    /// <summary>
    /// This is the constructor just for start up, not for switching states
    /// </summary>
    /// <param name="animation">State related animation</param>
    /// <param name="rotSpeed">The NPCs rotation speed</param>
    /// <param name="player">A reference to the player game object</param>
    /// <param name="viewRad">The radius/distance that the NPC can spot the player in</param>
    /// <param name="letterBox">The current active letter box script attached to camera</param>
    public NPCStateDefault(Animation animation, float rotSpeed, GameObject player, float viewRad, LetterBox letterBox, Transform transform) : base(animation)
    {
        m_rotationSpeed = rotSpeed;
        m_player = player;
        m_viewRadius = viewRad;
        m_letterBox = letterBox;
        m_NPCTransform = transform;
    }

    /// <summary>
    /// Call each frame to update and perform NPC related operations (Default state override)
    /// </summary>
    public override void StateUpdate()
    {
        //Currently an empty function
        base.StateUpdate();

        m_playerToTarget = m_player.transform.position - m_NPCTransform.position;
        m_playerToTarget.y = 0;

        float angle = Vector3.Angle(m_NPCTransform.forward, m_playerToTarget); //Needed for the turning speed

        //Check if the player is close enough to see
        if (Vector3.Distance(m_player.transform.position, m_NPCTransform.position) <= m_viewRadius)
        {
            m_targetVec = Vector3.RotateTowards(m_targetVec, m_playerToTarget, Time.deltaTime * (angle / 20) * m_rotationSpeed, 0.0f);
            transform.rotation = Quaternion.LookRotation(m_targetVec);
        }
        else
        {
            //Update turning speed to be inverse of angle, as it will be moving away, and as such start slow
            m_targetVec = Vector3.RotateTowards(m_targetVec, m_defaultDirection, Time.deltaTime * (angle / 20) * m_rotationSpeed, 0.0f);
            m_NPCTransform.rotation = Quaternion.LookRotation(m_targetVec);
        }
    }
}