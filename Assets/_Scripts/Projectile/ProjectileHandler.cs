using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class ProjectileHandler : MonoBehaviour
    {
        [SerializeField] Transform m_projectileContainer;
        int m_projectileIndex = 0;
        List<Projectile> m_projectiles = new List<Projectile>();
        // Start is called before the first frame update
        void Start()
        {
            foreach (Projectile p in m_projectileContainer.GetComponentsInChildren<Projectile>())
            {
                m_projectiles.Add(p);
                p.gameObject.SetActive(false);
            }
        }

        public void FireProjectile(Vector3 direction, Vector3 startPosition)
        {
            m_projectiles[m_projectileIndex].Fire(direction);
            m_projectiles[m_projectileIndex].transform.position = startPosition;
            m_projectiles[m_projectileIndex].transform.rotation = Quaternion.LookRotation(direction);
            m_projectileIndex = (m_projectileIndex + 1) % m_projectiles.Count;
            Debug.Log(m_projectileIndex);
        }
    }
}