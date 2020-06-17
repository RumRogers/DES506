using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameState
{

    public class GameStateController : GameCore.System.Automaton
    {
        GameObject m_pauseMenu;

        void Awake()
        {
            m_pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");

            SetState(new Playing_State(this));
        }

        override protected void Update()
        {
            m_state.Manage();
        }
    }
}