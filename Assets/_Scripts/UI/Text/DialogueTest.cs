using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTest : MonoBehaviour
{
    [SerializeField]
    float m_textUpdateTime = 0.05f;
    float m_deltaTime;

    [SerializeField]
    Canvas dialogueCanvas;
    [SerializeField]
    Text dialogueObject;
    [SerializeField]
    string hello;

    bool canTalk = false;

    private void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
        dialogueObject.text = "";
    }

    private void Update()
    {
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                startDialogue();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canTalk = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            canTalk = false;
        }
    }

    void startDialogue()
    {
        dialogueCanvas.gameObject.SetActive(true);
    }
}
