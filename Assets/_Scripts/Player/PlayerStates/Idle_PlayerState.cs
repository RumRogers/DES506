using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Idle_PlayerState : State
    {
        

        public Idle_PlayerState(PlayerEntity owner) : base(owner)
        {
            owner.Animator.SetProperty(PlayerAnimationProperties.IDLE);
            owner.Velocity = Vector3.zero;
        }

        public override void Manage() { }
    }
}