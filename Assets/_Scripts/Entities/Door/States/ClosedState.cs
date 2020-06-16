using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedState : State
{
    // Start is called before the first frame update
    public ClosedState(Automaton owner) : base(owner)
    {

    }

    public override void Manage()  //Current behaviour is just the animation from night to day
    {

    }
}
