using Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCollectibles
{
    public class PowerUp : Collectible
    {
        [SerializeField]
        private float m_targetSpellRange = 40f;
        [SerializeField]
        private float m_targetMeleeRange = 40f;

        ProjectileHandler m_projectileHandler;

        protected override void Start()
        {
            base.Start();
            m_projectileHandler = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<ProjectileHandler>();
        }
        protected override void TriggerHandler(Collider other)
        {
            m_projectileHandler.SpellRange = m_targetSpellRange;
            m_projectileHandler.MeleeRange = m_targetMeleeRange;
        }
    }
}

