using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameTips : MonoBehaviour
{
    [Header("Text for this tip")]
    [SerializeField]
    string m_text;

    [Header("Programmer Stuff")]
    [SerializeField]
    GameObject UI;
    [SerializeField]
    Text text;

    private void Awake()
    {
        text.text = m_text;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Fucking pls");
            UI.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
       // if (collision.gameObject.CompareTag("Player"))
        {
            UI.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            UI.SetActive(true);
        }
    }
}
