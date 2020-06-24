using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameState
{

    public class GameStateController : GameCore.System.Automaton
    {
        [SerializeField]
        GameObject m_pauseMenu;

        void Awake()
        {
            SetState(new Playing_State(this));
        }

        override protected void Update()
        {
            m_state.Manage();
        }

        public GameObject GetPauseMenu()
        {
            return m_pauseMenu;
        }

        //Ugly as all hell but you can't just call SetState straight from the button apparently, so we have
        //to deal with this ugliness.
        public void ResumeButtonPress() 
        {
            SetState(new Playing_State(this));
        }

        public bool IsPaused()
        {
            if (m_state.ToString() == "GameCore.GameState.Paused_State") //oof must change soon
            {
                return true;
            }

            return false;
        }

    }
}