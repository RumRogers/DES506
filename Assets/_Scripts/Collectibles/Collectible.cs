using GameCore.Spells;
using GameCore.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCollectibles
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public abstract class Collectible : MonoBehaviour
    {
        protected const string COLLECTIBLE_TAG = "Collectible";
        protected const string PLAYER_TAG = "Player";

        [SerializeField]
        float m_rotationSpeed = .5f;
        Quaternion m_rotation = Quaternion.Euler(0f, 1f, 0f);

        // Start is called before the first frame update
        protected virtual void Start()
        {
            gameObject.tag = COLLECTIBLE_TAG;
            m_rotation = Quaternion.Euler(0f, m_rotationSpeed, 0f);
        }

        // Update is called once per frame
        void Update()
        {
            Rotate();
        }

        void Rotate()
        {
            transform.rotation *= m_rotation;
        }

        private void OnTriggerEnter(Collider other)
        {            
            if (other.CompareTag(PLAYER_TAG))
            {
                TriggerHandler(other);
                Destroy(gameObject);
            }
        }

        protected abstract void TriggerHandler(Collider other);        
    }
}
