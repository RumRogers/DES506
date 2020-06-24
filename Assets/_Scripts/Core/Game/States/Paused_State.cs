using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameState
{

    public class Paused_State : GameCore.System.State
    { 
        GameStateController controller;

        public Paused_State(GameCore.System.Automaton owner) : base(owner)
        {
            Time.timeScale = 0;

            Cursor.visible = true;

            if (!owner.TryGetComponent<GameStateController>(out controller))
            {
                Debug.Log("Failed to find GameStateController script from owner");
            }

            controller.GetPauseMenu().SetActive(true);

            Debug.Log("Game is paused");
        }

        public override void Manage()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_owner.SetState(new Playing_State(m_owner));
            }
        }
    }
}