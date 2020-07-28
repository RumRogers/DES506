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

        private void Start()
        {
            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform == m_playerEntity.transform)
            {
                m_playerEntity.Respawn(LevelManager.p_LastCheckpoint.position + m_respawningOffset);
                m_playerEntity.Velocity = Vector3.zero;
                m_playerEntity.AddEntityProperty(PlayerEntityProperties.DYING);
                //m_playerEntity.transform.position = LevelManager.p_LastCheckpoint.position;
                FMODUnity.RuntimeManager.PlayOneShot("event:/PLAYER/MOVEMENT/Respawn/Respawn_FallOut");
            }
        }
    }
}