using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    //stores a pool of transforms for projectiles and cycles through them whenever the player fires (in aim mode and has pressed the left mouse button)
    public class ProjectileHandler : MonoBehaviour
    {
        [SerializeField] Transform m_projectileContainer;
        [SerializeField] float m_timeBetweenShots = 1;
        [SerializeField] float m_range = 5f;
        [SerializeField] float m_speed = 1f;
        [SerializeField] float m_spellRange = 5f;
        [SerializeField] float m_meleeRange = 1f;
        int m_projectileIndex = 0;
        bool m_canFire = true;
        List<Projectile> m_projectiles = new List<Projectile>();

        Player.PlayerEquipableItems m_projectileType;   //Will be usful later on when we need to change effects based on the item

        //Public Vars
        public float TimeBetweenShots { get => m_timeBetweenShots; }
        public float Range { get => m_range; }
        public float Speed { get => m_speed; }

        // Start is called before the first frame update
        void Start()
        {
            foreach (Projectile p in m_projectileContainer.GetComponentsInChildren<Projectile>())
            {
                m_projectiles.Add(p);
                p.gameObject.SetActive(false);
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