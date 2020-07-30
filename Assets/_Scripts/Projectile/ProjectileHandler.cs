﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    //stores a pool of transforms for projectiles and cycles through them whenever the player fires (in aim mode and has pressed the left mouse button)
    public class ProjectileHandler : MonoBehaviour
    {
        [SerializeField] GameObject m_projectileContainerPrefab;
        [SerializeField] float m_timeBetweenShots = 1;
        [SerializeField] float m_speed = 1f;
        [SerializeField] float m_spellRange = 5f;
        [SerializeField] float m_meleeRange = 1f;


        Transform m_projectileContainer;
        int m_projectileIndex = 0;
        float m_range = 5f;
        bool m_canFire = true;
        List<Projectile> m_projectiles = new List<Projectile>();

        Player.PlayerEquipableItems m_projectileType;   //Will be usful later on when we need to change effects based on the item

        //Public Vars
        public float TimeBetweenShots { get => m_timeBetweenShots; }
        public float Range { get => m_range; }
        public float Speed { get => m_speed; }
        public float SpellRange { get => m_spellRange; set { m_spellRange = value; } }
        public float MeleeRange { get => m_meleeRange; set { m_meleeRange = value; } }

        // Start is called before the first frame update
        void Start()
        {
            m_projectileContainer = GameObject.Instantiate(m_projectileContainerPrefab).transform;
            foreach (Projectile p in m_projectileContainer.GetComponentsInChildren<Projectile>())
            {
                m_projectiles.Add(p);
                p.gameObject.SetActive(false);
            }
        }


        private void Update()
        {
            //TEMP only here for prototyping / changing the value in editor at runtime. to be removed in final build
            switch (m_projectileType)
            {
                case Player.PlayerEquipableItems.ERASER:
                    m_range = m_meleeRange;
                    break;
                case Player.PlayerEquipableItems.SPELL_QUILL:
                    m_range = m_spellRange;
                    break;
            }
        }

        /// <summary>
        /// Takes in a direction of travel, position to start from and an equipped item type for determining range and which spell effect should be used
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="startPosition"></param>
        /// <param name="equipedItem"></param>
        public void FireProjectile(Vector3 direction, Vector3 startPosition)
        {
            if (m_canFire)
            {
                //Currently still fires the particle system regardless of which item is equipped
                m_projectiles[m_projectileIndex].Fire(direction, m_speed, m_range);
                m_projectiles[m_projectileIndex].transform.position = startPosition;
                m_projectiles[m_projectileIndex].transform.rotation = Quaternion.LookRotation(direction);
                m_projectileIndex = (m_projectileIndex + 1) % m_projectiles.Count;

                StartCoroutine(RecoverFromFiring());
            }
        }

        public void ChangeProjectileStatsBasedOnItem(Player.PlayerEquipableItems equipedItem)
        {
            switch (equipedItem)
            {
                case Player.PlayerEquipableItems.ERASER:
                    m_range = m_meleeRange;
                    m_projectileType = equipedItem;
                    break;
                case Player.PlayerEquipableItems.SPELL_QUILL:
                    m_range = m_spellRange;
                    m_projectileType = equipedItem;
                    break;
            }
        }

        IEnumerator RecoverFromFiring()
        {
            m_canFire = false;
            float time = 0;
            while (true)
            {
                time += Time.deltaTime;
                if(time > m_timeBetweenShots)
                {
                    m_canFire = true;
                    yield break;
                }
                yield return null;
            }
        }
    }
}