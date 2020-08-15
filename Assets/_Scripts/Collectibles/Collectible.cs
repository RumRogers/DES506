using GameCore.Spells;
using GameCore.System;
using GameUI.SpellBook;
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
        private const string SPELLBOOK_TAG = "SpellBook";

        [SerializeField]
        float m_rotationSpeed = .5f;
        Quaternion m_rotation = Quaternion.Euler(0f, 1f, 0f);
        SpellBook m_spellBook;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            gameObject.tag = COLLECTIBLE_TAG;
            m_rotation = Quaternion.Euler(0f, m_rotationSpeed, 0f);
            m_spellBook = GameObject.FindGameObjectWithTag(SPELLBOOK_TAG).GetComponent<SpellBook>();
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
            if (other.CompareTag(PLAYER_TAG) && !m_spellBook.p_IsActive)
            {
                TriggerHandler(other);
                Destroy(gameObject);
            }
        }

        protected abstract void TriggerHandler(Collider other);        
    }
}
