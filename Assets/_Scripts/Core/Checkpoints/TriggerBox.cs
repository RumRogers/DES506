using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Checkpoints
{

    public class TriggerBox : MonoBehaviour
    {
        Transform m_checkpointTransform;

        private void Start()
        {
            m_checkpointTransform = transform.parent.GetChild(0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                print("Checkpoint updated!");
                LevelManager.p_LastCheckpoint = m_checkpointTransform;
            }
        }
    }
}