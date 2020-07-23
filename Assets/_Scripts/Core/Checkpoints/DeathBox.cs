using GameCore.System;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Checkpoints
{
    public class DeathBox : MonoBehaviour
    {        
        PlayerEntity m_playerEntity;

        private void Start()
        {
            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform == m_playerEntity.transform)
            {
                m_playerEntity.Respawn(LevelManager.p_LastCheckpoint.position + Vector3.up * 20f);
                m_playerEntity.AddEntityProperty(PlayerEntityProperties.DYING);
                //m_playerEntity.transform.position = LevelManager.p_LastCheckpoint.position;
            }
        }
    }
}