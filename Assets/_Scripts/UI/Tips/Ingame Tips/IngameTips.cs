using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameTips : MonoBehaviour
{
    [Header("Text for this tip")]
    [SerializeField]
    string m_text;

    [Header("Make sure these are set!")]
    [SerializeField]
    GameObject UI;
    [SerializeField]
    Text text;

    private void Awake()
    {
        text.text = m_text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.text = m_text;
            UI.SetActive(true);
        }
    }
   

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UI.SetActive(false);
        }
    }
}
