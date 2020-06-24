using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class followingNPC : stateNPC
{
    private Vector3 m_target = Vector3.zero;
    private NavMeshAgent m_navAgent;

    public followingNPC(Animation stateAnimation, Transform player, Transform NPC) : base(stateAnimation, 3, player, NPC)
    {
        throw new System.NotImplementedException();

        //Set and check the target (the player)
    }
    public override void OnUpdate()
    {
        throw new System.NotImplementedException();

        //if NPC is within circle of player
        //Delete destination

        //else if NPC has reached target
        //Update target

    }

    private bool IsNPCClose()
    {
        //Determine a radius around player
        //if NPC is within that area
        //Return true

        throw new System.NotImplementedException();

        return false;
    }

}
