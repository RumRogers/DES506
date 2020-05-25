using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Death_PlayerState : GameCore.System.State
    {
        PlayerMovement m_playerMovement;

        public Death_PlayerState(GameCore.System.Automaton owner) : base(owner)
        {
            m_playerMovement = (PlayerMovement)owner;
            Debug.Log("In DeathState!");
            m_playerMovement.transform.position = m_playerMovement.PlayerStartPosition;
        }

        public override void Manage()
        {
        }
    }
}