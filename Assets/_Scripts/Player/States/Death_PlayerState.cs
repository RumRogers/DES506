using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Death_PlayerState : GameCore.System.State
    {
        PlayerEntity m_playerEntity;

        public Death_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerEntity = (PlayerEntity)owner;
            Debug.Log("In DeathState!");
            m_playerEntity.transform.position = m_playerEntity.PlayerStartPosition;
        }

        public override void Manage()
        {
        }
    }
}