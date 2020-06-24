#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class walkingNPC : stateNPC
{
    #region //Variables
    //External passed
    private NavMeshAgent m_navAgent;
    private Transform[] m_destinations;
    private Animation m_stateAnimation; //To be implemented

    //Internals 
    private bool m_directionForward = true;
    private float m_waitTime = Random.Range(0.5f, 1.7f);
    private int m_index = 0;

    //Constant probabilities 
    private const float c_probabilityOfChange = 0.15f;
    private float c_probabilityOfStanding = 0.1f;
    #endregion

    /// <summary>
    /// The constructor for the walking NPC class. It is a subclass of stateNPC
    /// </summary>
    /// <param name="stateAnimation">Associated animation with this state, passed through to the base constructor</param>
    /// <param name="agent">Information needed for the navigational mesh for the NPC to walkt to target</param>
    /// <param name="destinations">An array of target points for the NPC to walk to. Must be greater than one.</param>
    public walkingNPC(Animation stateAnimation, NavMeshAgent agent, Transform[] destinations, Transform player, Transform NPC) : base(stateAnimation, 1, player, NPC)
    {
        m_navAgent = agent;
        m_destinations = destinations;
        m_stateAnimation = stateAnimation;

        if (!m_navAgent)
        {
            LogError("The nav agent component is missing from an NPC");
        }

        if (m_destinations.Length <= 1)
        {
            LogError("There are insufficient targets for an NPC. Please ensure there are more than one");
        }

        m_navAgent.SetDestination(m_destinations[m_index].position);
    }

    public override void OnUpdate()
    {
        //if destination reached
        if (m_navAgent.remainingDistance <= 1.0f)
        {
            //If the npc has waited long enough
            if (m_counter >= m_waitTime)
            {
                if (IsStateChanged())
                {
                    m_currentState = 0; //Switch to standing
                }

                else
                {
                    SetNewTarget();
                }
            }

            else
            {
                m_counter += Time.deltaTime;
            }
        }
    }

    private bool IsStateChanged()
    {
        return Random.Range(0.0f, 1.0f) <= c_probabilityOfStanding ? true : false;
    }

    void SetNewTarget()
    {
        if (Random.Range(0.0f, 1.0f) <= c_probabilityOfChange)
        {
            m_directionForward = !m_directionForward;
        }

        if (m_directionForward)
        {
            m_index = (m_index + 1) % m_destinations.Length;
        }
        else
        {
            m_index--;

            //Prevent going out of array limits
            if (m_index < 0)
                m_index = m_destinations.Length - 1;
        }

        m_navAgent.SetDestination(m_destinations[m_index].position);
        m_counter = 0.0f;
    }
}

