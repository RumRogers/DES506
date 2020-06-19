using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PrototypeReactor : MonoBehaviour
{
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(new Vector3(0f, .5f, 0f));
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
