using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI.Effects
{
    public class Billboard : MonoBehaviour
    {
        Transform m_cameraTransform;

        void Start()
        {
            m_cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            transform.rotation = Quaternion.LookRotation(-m_cameraTransform.forward, m_cameraTransform.up);
        }
    }
}
