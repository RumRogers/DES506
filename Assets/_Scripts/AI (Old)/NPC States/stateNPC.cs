using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the NPC's states. Subject to change as more behaviours emerge, and existing ones change
/// </summary>
public abstract class stateNPC
{
    public int m_currentState = 0;
    protected Animation m_stateAnimation;
    protected float m_counter = 0.0f;

    private float m_fieldOfViewAngle = 80f;
    private bool m_objectInSight = false;
    private Transform m_playerTransform;//= Vector3.zero;
    private Transform m_npcTransform;// = Vector3.zero;
    private const float c_lineOfSight = 4f;

    /// <summary>
    /// Constructor for the base class for NPC states
    /// </summary>
    /// <param name="stateAnimation">Each state will likely have it's own animation that will be passed in upon construction.</param>
    protected stateNPC(Animation stateAnimation, int state, Transform playerTransform, Transform npcTransform)
    {
        m_stateAnimation = stateAnimation;
        m_currentState = state;
        m_playerTransform = playerTransform;
        m_npcTransform = npcTransform;
    }

    /// <summary>
    /// Called within player update phase
    /// </summary>
    public abstract void OnUpdate();

    /// <summary>
    /// In order to determin if the NPC can view the player, call this with the player position as pass variable
    /// </summary>
    protected void FieldOfView() // Pass in player reference (position should do)
    {
        Vector3 m_npcToPlayer = m_playerTransform.position - m_npcTransform.position;

        if ((Vector3.Angle(m_npcToPlayer, m_npcTransform.transform.forward) < m_fieldOfViewAngle * 0.5) && m_npcToPlayer.magnitude <= c_lineOfSight)
        {
            Debug.Log("The player is in sight");
        }

        //Note: see if you can draw with gizmos 
    }

    protected void LogError(string message)
    {
        Debug.Log("<color=red> ERROR: </color>" + message);
        Debug.Break();
    }

    public void DebugLines()
    {
        Quaternion leftRayRotation = Quaternion.AngleAxis(-80 / 2, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(80 / 2, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * m_npcTransform.forward;
        Vector3 rightRayDirection = rightRayRotation * m_npcTransform.forward;

        Gizmos.DrawRay(m_npcTransform.position, leftRayDirection * 5f);
        Gizmos.DrawRay(m_npcTransform.position, rightRayDirection * 5f);
    }
    
}
