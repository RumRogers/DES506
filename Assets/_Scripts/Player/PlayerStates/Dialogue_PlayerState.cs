using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Dialogue_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        public Dialogue_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            Debug.Log("In Dialogue");
        }

        public override void Manage()
        {
        }
    }
}