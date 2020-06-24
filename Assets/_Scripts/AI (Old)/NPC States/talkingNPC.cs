using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkingNPC : stateNPC
{
    /*
     * This will require some tie in with the dialog system
     */
    public talkingNPC(Animation stateAnimation, Transform player, Transform NPC) : base(stateAnimation, 2, player, NPC)
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    private void RotateToPlayer()
    {
        //At start rotate to player to make sure they are facing them
        throw new System.NotImplementedException();
    }

    private void ChangeState()
    {
        throw new System.NotImplementedException();
    }
}
