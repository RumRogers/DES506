using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    //Current potential states for the NPC
    enum e_NPCStates  {STANDING, WALKING, TALKING, FOLLOWING};

    [SerializeField]
    private e_NPCStates m_currentState;

    //[SerializeField]
    //private Transform m_destination;

    [SerializeField]
    private bool m_checkForStateChange = true;

    [SerializeField]
    private Transform[] m_targetPositions;

    //[SerializeField]
    //private float m_playerSpeed;

    [SerializeField]
    private Transform m_player;

    private stateNPC m_npcState;
    private NavMeshAgent m_agent;
    
    // Start is called before the first frame update
    void Start()
    {
        m_agent = this.GetComponent<NavMeshAgent>();
        m_npcState = new walkingNPC(null, m_agent, m_targetPositions, m_player, this.transform);
        m_npcState.m_currentState = (int)m_currentState; //Explicit cast is probably not ideal

        if (!m_agent)
            Debug.Log("<color=red> ERROR: </color>The Nav Mesh Agent component hasn't been found in " + this.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the state has changed
        if (m_npcState.m_currentState != (int)m_currentState || Input.GetKeyDown(KeyCode.P))
        {
            m_npcState.m_currentState = 0;
            //Update the current state late, to fit the changed state
            m_currentState = (e_NPCStates)m_npcState.m_currentState;
            
            

            m_checkForStateChange = false;
            
            switch (m_currentState)
            {
                case e_NPCStates.STANDING:
                    m_npcState = new standingNPC(null, m_player.transform, this.transform);
                    break;

                case e_NPCStates.FOLLOWING:
                    m_npcState = new followingNPC(null, m_player.transform, this.transform);
                    break;

                case e_NPCStates.TALKING:
                    m_npcState = new talkingNPC(null, m_player.transform, this.transform);
                    break;

                case e_NPCStates.WALKING:
                    m_npcState = new walkingNPC(null, m_agent, m_targetPositions, m_player.transform, this.transform);
                    break;

                default:
                    Debug.Log("<color=red> ERROR: </color> Enum for current state has failed in: " + this.gameObject.name);
                    break;
            }
        }

        m_npcState.OnUpdate();
    }

    private void OnDrawGizmos()
    {
        if(m_npcState != null)
            m_npcState.DebugLines();
    }
}
