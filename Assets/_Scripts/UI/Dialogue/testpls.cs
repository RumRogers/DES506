using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testpls : MonoBehaviour
{
    const string m_dialogueHUDTag = "UI_DialogueHUD";

    Player.PlayerEntity m_player = null;
    GameObject m_dialogueUI;

    bool canTalk = false;
    public bool p_isTalking { get; set; }
    public bool p_isDialogueHUDActive { get => m_dialogueUI.activeSelf; }
    private void Start()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag(m_dialogueHUDTag);
        foreach(Transform t in tmp.transform)
        {
            if(t.CompareTag(m_dialogueHUDTag))
            {
                m_dialogueUI = t.gameObject;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canTalk = true;
     
            if (!m_player)
            {
                m_player = other.gameObject.GetComponent<Player.PlayerEntity>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canTalk = false;
        }
    }

    private void Update()
    {
        if (canTalk && !p_isTalking && m_player.IsGrounded())
        {
            if (Input.GetKeyUp(KeyCode.E) && !m_dialogueUI.activeSelf)
            {
                p_isTalking = true;
                m_player.SetState(new Player.Dialogue_PlayerState(m_player, this.gameObject));
            }
        }
    }
}
