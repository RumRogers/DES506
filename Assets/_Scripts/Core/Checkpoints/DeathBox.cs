using GameCore.System;
using GameCore.Camera;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Checkpoints
{
    public class DeathBox : MonoBehaviour
    {
        [Header("Respawning offset Vector")]
        [SerializeField]
        Vector3 m_respawningOffset = new Vector3(0f, 150f, 0f);

        PlayerEntity m_playerEntity;
        PlayerMoveCamera m_playerMoveCamera;

        private void Start()
        {
            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
            if (!UnityEngine.Camera.main.transform.TryGetComponent<GameCore.Camera.PlayerMoveCamera>(out m_playerMoveCamera))
            {
                Debug.LogError("Cannot get PlayerMoveCamera Component on Main Camera!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform == m_playerEntity.transform)
            {
                m_playerMoveCamera.SetState(new Respawn_CameraState(m_playerMoveCamera, LevelManager.p_LastCheckpoint.position));
                m_playerEntity.Respawn(LevelManager.p_LastCheckpoint.position + m_respawningOffset);
                m_playerEntity.Velocity = Vector3.zero;
                m_playerEntity.AddEntityProperty(PlayerEntityProperties.DYING);
                //m_playerEntity.transform.position = LevelManager.p_LastCheckpoint.position;
            }
        }
    }
}