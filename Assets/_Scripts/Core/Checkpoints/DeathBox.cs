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
            m_playerMoveCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMoveCamera>();
            m_playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform == m_playerEntity.transform)
            {

                m_playerMoveCamera.StartCoroutine(m_playerMoveCamera.FadeToColour(Color.black, 0.2f,
                    () =>
                    {
                        m_playerEntity.Respawn(LevelManager.p_LastCheckpoint.position + m_respawningOffset);
                        m_playerEntity.AddEntityProperty(PlayerEntityProperties.DYING);
                        return true;
                    }));

                //m_playerEntity.transform.position = LevelManager.p_LastCheckpoint.position;
                FMODUnity.RuntimeManager.PlayOneShot("event:/PLAYER/MOVEMENT/Respawn/Respawn_FallOut");
            }
        }
    }
}