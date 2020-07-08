using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeight : MonoBehaviour
{
    [SerializeField]
    private float m_playerWeight = 10;

    [SerializeField]
    private Rigidbody m_platRigidBody;

    private bool playerOn = false;
    float m_playerDistanceFromCenter;

//    BoxCollider box;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOn = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //float m_playerDistanceFromCenter = Vector3.Distance(m_platRigidBody.position, transform.position);

        if (playerOn)
        {
            m_playerDistanceFromCenter = Vector3.Distance(m_platRigidBody.position, transform.position);
            m_platRigidBody.AddForceAtPosition(Vector3.down * m_playerWeight, transform.position);
        }
    }
}
