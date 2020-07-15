using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using GameCore.System;

namespace GameCore.GameState
{

    public class Paused_State : GameCore.System.State
    { 
        GameStateController controller;
        PlayerEntity m_playerEntity;
        State m_previousState;

        public Paused_State(GameCore.System.Automaton owner) : base(owner)
        {
            Time.timeScale = 0;
            
            Cursor.visible = true;

            if (!owner.TryGetComponent<GameStateController>(out controller))
            {
                Debug.Log("Failed to find GameStateController script from owner");
            }

            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();

            m_previousState = m_playerEntity.GetState();

            m_playerEntity.SetState(new Idle_PlayerState(m_playerEntity));
            

            controller.GetPauseMenu().SetActive(true);

            Debug.Log("Game is paused");
        }

        public override void Manage()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_owner.SetState(new Playing_State(m_owner, m_previousState));
            }
            //oof
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}