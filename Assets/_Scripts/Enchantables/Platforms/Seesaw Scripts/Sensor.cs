using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private bool isTriggered = false;

    public bool IsTriggered() {  return isTriggered; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            isTriggered = false;
    }
}
