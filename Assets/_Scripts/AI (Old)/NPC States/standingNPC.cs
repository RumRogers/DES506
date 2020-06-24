using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class standingNPC : stateNPC
{
    /*
        Player reference (likely just pos data)
        probability of walking


     */
    public standingNPC(Animation stateAnimation, Transform player, Transform NPC) : base(stateAnimation, 0, player, NPC)
    {
        
    }
    public override void OnUpdate()
    {
        FieldOfView();
    }

    private void UpdateState()
    {
        //if probablity is met for walking
        //set to walk

        //if player interacts with NPC
        //set to talking
        throw new System.NotImplementedException();
    }

    private void RotateToPlayer()
    {
        //if player enters field of view
        //Set the NPC to be constantly facing the player
        throw new System.NotImplementedException();
    }

}
