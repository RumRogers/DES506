using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Projectile : MonoBehaviour
    {
        Vector3 m_direction;
        float m_speed = 10;

        // Update is called once per frame
        void Update()
        {
            if (gameObject.activeSelf)
            {
                transform.position += (m_direction * m_speed) * Time.deltaTime;
            }
        }

        public void Fire(Vector3 direction)
        {
            m_direction = direction;
            gameObject.SetActive(true);
        }

        //if colliding, deactivate    
        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
        }
    }
}