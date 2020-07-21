using GameCore.System;
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
                // uncomment this for crappy unstable behavior
                //m_playerEntity.SetState(new Death_PlayerState(m_playerEntity));
                //m_playerEntity.Respawn(LevelManager.p_LastCheckpoint.position + m_respawningOffset);
                m_playerEntity.Respawn(LevelManager.p_LastCheckpoint.position);
            }
        }
    }
}