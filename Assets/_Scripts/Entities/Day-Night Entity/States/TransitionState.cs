#define DEBUG

using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionState : State
{
    public TransitionState(Automaton owner) : base(owner)
    {
        #if DEBUG
                Debug.Log("I am transitioning");
        #endif
    }
    public override void Manage() 
    { 
        
    }
}
