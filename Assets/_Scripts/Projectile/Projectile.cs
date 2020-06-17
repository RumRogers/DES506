using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Projectile : MonoBehaviour
    {
        Vector3 m_direction;
        float m_speed;
        float m_lifetime = 5;
        float m_time = 0;
        float m_range = 0;
        // Update is called once per frame
        void Update()
        {
            m_time += Time.deltaTime;

            if (m_time > m_lifetime)
            {
                gameObject.SetActive(false);
                m_time = 0;
            }

            if (gameObject.activeSelf)
            {
                transform.position += (m_direction * m_speed) * Time.deltaTime;
            }
        }

        public void Fire(Vector3 direction, float speed, float range)
        {
            m_direction = direction;
            m_speed = speed;
            m_range = range;
            m_lifetime = range / speed; //lifetime is decided by range and speed given
            gameObject.SetActive(true);
        }

        //if colliding, deactivate    
        private void OnTriggerEnter(Collider other)
        {
            m_time = 0;
            gameObject.SetActive(false);
        }
    }
}