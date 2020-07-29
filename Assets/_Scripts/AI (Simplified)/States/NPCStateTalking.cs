using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateTalking : NPCState
{

    /// <summary>
    /// This is the constructor just for start up, not for switching states
    /// </summary>
    public NPCStateTalking(Animator animator, string anim, LetterBox letterBox) : base(animator, anim)
    {
        animator.SetBool("isTalking", true);
        letterBox.TurnOn();
    }

    /// <summary>
    /// Call each frame to update and perform NPC related operations (Default state override)
    /// </summary>
    public override void StateUpdate()
    {
        base.StateUpdate();

    }
}
