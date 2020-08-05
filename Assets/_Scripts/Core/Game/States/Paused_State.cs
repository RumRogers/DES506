using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using GameCore.System;
using GameCore.Camera;

namespace GameCore.GameState
{

    public class Paused_State : GameCore.System.State
    { 
        GameStateController controller;
        PlayerEntity m_playerEntity;
        PlayerMoveCamera m_playerMoveCamera;

        State m_previousState;
        State m_playerMoveCameraPreviousState;

        public Paused_State(GameStateController owner) : base(owner)
        {
            Time.timeScale = 0;
            
            Cursor.visible = true;

            controller = owner;

            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
            m_playerMoveCamera = UnityEngine.Camera.main.GetComponent<PlayerMoveCamera>();

            m_previousState = m_playerEntity.GetState();
            m_playerMoveCameraPreviousState = m_playerMoveCamera.GetState();

            m_playerEntity.SetState(new Idle_PlayerState(m_playerEntity));
            m_playerMoveCamera.SetState(new Idle_CameraState(m_playerMoveCamera));

            controller.SetPrevState(m_previousState, m_playerMoveCameraPreviousState);
            controller.GetPauseMenu().SetActive(true);

            controller.SetIngameUIActive(false);

            Debug.Log("Game is paused");
        }

        public override void Manage()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_owner.SetState(new Playing_State(controller, m_previousState));
            }
            //oof
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}