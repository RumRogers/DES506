using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeight : MonoBehaviour
{
    public Rigidbody testing;
    public Transform pointOfForce;
    private bool playerOn = false;
    // Start is called before the first frame update
    void Start()
    {

    }
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
        if(playerOn)
            testing.AddForceAtPosition(Vector3.down * 10, pointOfForce.position);
    }
}
